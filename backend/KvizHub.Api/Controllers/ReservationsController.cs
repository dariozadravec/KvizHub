using KvizHub.Api.Data;
using KvizHub.Api.Identity;
using KvizHub.Api.Services;
using KvizHub.Shared.Reservations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class ReservationsController(KvizHubDbContext db, ReservationService reservationService) : ControllerBase
    {
        [HttpGet("teams/mine")]
        [Authorize(Policy = Policies.TeamLeader)]
        public async Task<List<TeamDto>> GetMyTeams()
        {
            var userId = User.GetUserId();

            return await db.TeamMembers
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Team.Name)
                .Select(m => new TeamDto(m.TeamId, m.Team.Name))
                .ToListAsync();
        }

        [HttpPost("events/{eventId:int}/reservations")]
        [Authorize(Policy = Policies.TeamLeader)]
        public async Task<ActionResult<ReservationDto>> Create(int eventId, CreateReservationRequest request)
        {
            var outcome = await reservationService.ReserveAsync(eventId, User.GetUserId(), request);
            if (outcome.Reservation is null)
                return Problem(detail: outcome.Error, statusCode: outcome.StatusCode);

            return await ToDtoAsync(outcome.Reservation.Id);
        }

        private Task<ReservationDto> ToDtoAsync(int reservationId) =>
            db.Reservations
                .Where(r => r.Id == reservationId)
                .Select(r => new ReservationDto(r.Id, r.EventId, r.Event.Title, r.Event.StartsAt, r.Team.Name, r.TeamSize, r.Status))
                .SingleAsync();
    }
}