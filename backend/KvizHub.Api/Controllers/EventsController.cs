using KvizHub.Api.Data;
using KvizHub.Api.Data.Entities;
using KvizHub.Api.Identity;
using KvizHub.Api.Services;
using KvizHub.Shared.Enums;
using KvizHub.Shared.Events;
using KvizHub.Shared.Venues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController(KvizHubDbContext db, EventStatusService statusService) : ControllerBase
    {
        [HttpGet("mine")]
        [Authorize(Policy = Policies.Quizmaster)]
        public async Task<List<MyEventDto>> GetMine()
        {
            var userId = User.GetUserId();

            return await db.QuizEvents
                .Where(q => q.OrganizerId == userId)
                .OrderByDescending(q => q.StartsAt)
                .Select(q => new MyEventDto(q.Id, q.Title, q.StartsAt, q.Status, q.Venue.Name, q.Venue.City))
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EventDto>> Get(int id)
        {
            var ev = await db.QuizEvents
                .Include(q => q.Venue)
                .Include(q => q.Organizer)
                .SingleOrDefaultAsync(q => q.Id == id);

            // Drafts are visible only to their organizer
            if (ev is null || (ev.Status == EventStatus.Draft && ev.OrganizerId != User.GetUserId()))
                return NotFound();

            var sections = await db.Sections
                .Where(s => s.EventId == id)
                .OrderBy(s => s.Position)
                .ToListAsync();

            return ToDto(ev, sections);
        }

        [HttpPost]
        [Authorize(Policy = Policies.Quizmaster)]
        public async Task<ActionResult<EventDto>> Create(SaveEventRequest request)
        {
            if (!await db.Venues.AnyAsync(v => v.Id == request.VenueId))
                return VenueNotFound();

            var ev = new QuizEvent
            {
                Title = request.Title.Trim(),
                OrganizerId = User.GetUserId(),
                Status = EventStatus.Draft
            };
            Apply(ev, request);

            db.QuizEvents.Add(ev);
            db.Sections.AddRange(CreateSections(ev, request));
            await db.SaveChangesAsync();

            return await Get(ev.Id);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = Policies.Quizmaster)]
        public async Task<ActionResult<EventDto>> Update(int id, SaveEventRequest request)
        {
            var ev = await db.QuizEvents.FindAsync(id);
            if (ev is null)
                return NotFound();
            if (ev.OrganizerId != User.GetUserId())
                return Forbid();

            // After publishing, sections and the joker setting can no longer change
            if (ev.Status != EventStatus.Draft)
                return Problem(detail: "Dogodek je mogoče urejati samo, dokler je osnutek.", statusCode: StatusCodes.Status409Conflict);

            if (!await db.Venues.AnyAsync(v => v.Id == request.VenueId))
                return VenueNotFound();

            Apply(ev, request);

            // Sections are replaced as a whole, positions follow the order in the request
            var oldSections = await db.Sections.Where(s => s.EventId == id).ToListAsync();
            db.Sections.RemoveRange(oldSections);
            db.Sections.AddRange(CreateSections(ev, request));
            await db.SaveChangesAsync();

            return await Get(id);
        }

        [HttpPost("{id:int}/status")]
        [Authorize(Policy = Policies.Quizmaster)]
        public async Task<ActionResult<EventDto>> ChangeStatus(int id, ChangeStatusRequest request)
        {
            var ev = await db.QuizEvents.FindAsync(id);
            if (ev is null)
                return NotFound();
            if (ev.OrganizerId != User.GetUserId())
                return Forbid();

            var error = await statusService.ChangeStatusAsync(ev, request.Status);
            if (error is not null)
                return Problem(detail: error, statusCode: StatusCodes.Status409Conflict);

            return await Get(id);
        }

        private static void Apply(QuizEvent ev, SaveEventRequest request)
        {
            ev.Title = request.Title.Trim();
            ev.StartsAt = request.StartsAt!.Value.UtcDateTime;
            ev.VenueId = request.VenueId!.Value;
            ev.Capacity = request.Capacity;
            ev.JokerEnabled = request.JokerEnabled;
        }

        private static List<Section> CreateSections(QuizEvent ev, SaveEventRequest request) =>
            request.Sections
                .Select((s, index) => new Section
                {
                    Event = ev,
                    Position = (short)(index + 1),
                    Name = s.Name.Trim(),
                    Topic = s.Topic.Trim(),
                    MaxPoints = s.MaxPoints
                })
                .ToList();

        private ActionResult VenueNotFound()
        {
            ModelState.AddModelError(nameof(SaveEventRequest.VenueId), "Izbrani lokal ne obstaja.");
            return ValidationProblem(ModelState);
        }

        private static EventDto ToDto(QuizEvent ev, List<Section> sections) => new(
            ev.Id,
            ev.Title,
            ev.StartsAt,
            ev.Status,
            ev.Capacity,
            ev.JokerEnabled,
            new VenueDto(ev.Venue.Id, ev.Venue.Name, ev.Venue.Street, ev.Venue.PostalCode, ev.Venue.City, ev.Venue.DefaultCapacity),
            ev.Organizer.DisplayName,
            sections.Select(s => new SectionDto(s.Id, s.Position, s.Name, s.Topic, s.MaxPoints)).ToList());
    }
}