using KvizHub.Api.Data;
using KvizHub.Api.Data.Entities;
using KvizHub.Api.Identity;
using KvizHub.Api.Services;
using KvizHub.Shared.Venues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Controllers
{
    [ApiController]
    [Route("api/venues")]
    [Authorize(Policy = Policies.Quizmaster)]
    public class VenuesController(KvizHubDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<VenueDto>>> GetAll(string? search)
        {
            var query = db.Venues.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(v => EF.Functions.ILike(v.Name, $"%{search}%"));

            return await query
                .OrderBy(v => v.Name)
                .Select(v => new VenueDto(v.Id, v.Name, v.Street, v.PostalCode, v.City, v.DefaultCapacity))
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<VenueDto>> Create(CreateVenueRequest request)
        {
            var normalizedAddress = AddressNormalizer.NormalizeAddress(request.Street, request.PostalCode, request.City);
            var venueKey = AddressNormalizer.VenueKey(normalizedAddress, request.Name);

            // Same name at the same address is always a duplicate
            if (await db.Venues.AnyAsync(v => v.NormalizedAddress == venueKey))
                return Problem(detail: "Ta lokal na tem naslovu že obstaja.", statusCode: StatusCodes.Status409Conflict);

            // Another venue at the same address needs confirmation, because it is usually the same venue
            if (!request.ConfirmSameAddress)
            {
                var prefix = AddressNormalizer.AddressPrefix(normalizedAddress);
                var existingName = await db.Venues
                    .Where(v => v.NormalizedAddress.StartsWith(prefix))
                    .Select(v => v.Name)
                    .FirstOrDefaultAsync();

                if (existingName is not null)
                    return Problem(
                        title: "SameAddress",
                        detail: $"Lokal „{existingName}“ že obstaja na tem naslovu. Ste prepričani, da ste vpisali pravi naslov in želite nadaljevati?",
                        statusCode: StatusCodes.Status409Conflict);
            }

            var venue = new Venue
            {
                Name = request.Name.Trim(),
                Street = request.Street.Trim(),
                PostalCode = request.PostalCode.Trim(),
                City = request.City.Trim(),
                NormalizedAddress = venueKey,
                DefaultCapacity = request.DefaultCapacity,
                CreatedById = User.GetUserId()
            };

            db.Venues.Add(venue);
            await db.SaveChangesAsync();

            return Ok(new VenueDto(venue.Id, venue.Name, venue.Street, venue.PostalCode, venue.City, venue.DefaultCapacity));
        }
    }
}