using KvizHub.Api.Data;
using KvizHub.Api.Identity;
using KvizHub.Api.Services;
using KvizHub.Shared.Enums;
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

        [HttpGet("reservations/mine")]
        [Authorize(Policy = Policies.TeamLeader)]
        public async Task<List<ReservationDto>> GetMine()
        {
            var userId = User.GetUserId();

            return await db.Reservations
                .Where(r => r.Leader.UserId == userId)
                .OrderByDescending(r => r.Event.StartsAt)
                .Select(r => new ReservationDto(r.Id, r.EventId, r.Event.Title, r.Event.StartsAt, r.Event.Status, r.Team.Name, r.TeamSize, r.Status))
                .ToListAsync();
        }

        [HttpGet("events/{eventId:int}/reservations")]
        [Authorize(Policy = Policies.Quizmaster)]
        public async Task<ActionResult<List<EventReservationDto>>> GetForEvent(int eventId)
        {
            var ev = await db.QuizEvents.FindAsync(eventId);
            if (ev is null)
                return NotFound();
            if (ev.OrganizerId != User.GetUserId())
                return Forbid();

            // Active reservations first, cancelled ones at the end
            return await db.Reservations
                .Where(r => r.EventId == eventId)
                .OrderBy(r => r.Status == ReservationStatus.Cancelled)
                .ThenBy(r => r.CreatedAt)
                .Select(r => new EventReservationDto(r.Id, r.Team.Name, r.TeamSize, r.Leader.Name, r.Status))
                .ToListAsync();
        }

        [HttpPost("reservations/{id:int}/cancel")]
        [Authorize(Policy = Policies.TeamLeader)]
        public async Task<ActionResult<ReservationDto>> Cancel(int id)
        {
            var outcome = await reservationService.CancelAsync(id, User.GetUserId());
            if (outcome.Reservation is null)
                return Problem(detail: outcome.Error, statusCode: outcome.StatusCode);

            return await ToDtoAsync(id);
        }

        private Task<ReservationDto> ToDtoAsync(int reservationId) =>
            db.Reservations
                .Where(r => r.Id == reservationId)
                .Select(r => new ReservationDto(r.Id, r.EventId, r.Event.Title, r.Event.StartsAt, r.Event.Status, r.Team.Name, r.TeamSize, r.Status))
                .SingleAsync();
    }
}