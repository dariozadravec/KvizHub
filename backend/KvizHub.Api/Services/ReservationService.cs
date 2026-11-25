using KvizHub.Api.Data;
using KvizHub.Api.Data.Entities;
using KvizHub.Shared.Enums;
using KvizHub.Shared.Reservations;
using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Services
{
    // Result of a reservation attempt: the reservation, or an HTTP status code with an error message
    public sealed record ReservationOutcome(Reservation? Reservation, int StatusCode, string? Error)
    {
        public static ReservationOutcome Fail(int statusCode, string error) => new(null, statusCode, error);
    }

    public class ReservationService(KvizHubDbContext db)
    {
        public async Task<ReservationOutcome> ReserveAsync(int eventId, int userId, CreateReservationRequest request)
        {
            await using var transaction = await db.Database.BeginTransactionAsync();

            // Locks the event row until the end of the transaction, so concurrent reservations
            // for the same event wait for each other and can never exceed the capacity together
            var ev = await db.QuizEvents
                .FromSql($"SELECT * FROM quiz_event WHERE id = {eventId} FOR UPDATE")
                .SingleOrDefaultAsync();

            if (ev is null)
                return ReservationOutcome.Fail(StatusCodes.Status404NotFound, "Dogodek ne obstaja.");
            if (ev.Status != EventStatus.Published)
                return ReservationOutcome.Fail(StatusCodes.Status409Conflict, "Rezervacije za ta dogodek niso odprte.");

            TeamMember leader;
            Reservation? existing = null;

            if (request.TeamId is not null)
            {
                // The signed-in user must be a member of the selected team
                var member = await db.TeamMembers
                    .Include(m => m.Team)
                    .SingleOrDefaultAsync(m => m.TeamId == request.TeamId && m.UserId == userId);
                if (member is null)
                    return ReservationOutcome.Fail(StatusCodes.Status403Forbidden, "Izbrana ekipa ni vaša.");
                leader = member;

                // A team has at most one reservation per event; a cancelled one is reused
                existing = await db.Reservations.SingleOrDefaultAsync(r => r.EventId == eventId && r.TeamId == member.TeamId);
                if (existing is not null && existing.Status != ReservationStatus.Cancelled)
                    return ReservationOutcome.Fail(StatusCodes.Status409Conflict, "Ekipa že ima rezervacijo za ta dogodek.");
            }
            else
            {
                // A new team is created with the signed-in user as its member
                var user = await db.Users.FindAsync(userId);
                leader = new TeamMember
                {
                    Name = user!.DisplayName,
                    UserId = userId,
                    Team = new Team { Name = request.NewTeamName!.Trim() }
                };
                db.TeamMembers.Add(leader);
            }

            var reservedSeats = await db.Reservations
                .Where(r => r.EventId == eventId && r.Status == ReservationStatus.Confirmed)
                .SumAsync(r => (int)r.TeamSize);
            var freeSeats = ev.Capacity - reservedSeats;

            if (request.TeamSize > freeSeats)
                return ReservationOutcome.Fail(StatusCodes.Status409Conflict,
                    $"Ni dovolj prostih mest. Prostih mest: {Math.Max(freeSeats, 0)}.");

            var reservation = existing ?? new Reservation { EventId = eventId, Team = leader.Team };
            reservation.Leader = leader;
            reservation.TeamSize = (short)request.TeamSize;
            reservation.Status = ReservationStatus.Confirmed;
            reservation.CancelledAt = null;

            if (existing is null)
                db.Reservations.Add(reservation);

            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ReservationOutcome(reservation, StatusCodes.Status200OK, null);
        }
    }
}