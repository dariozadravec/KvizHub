using KvizHub.Api.Data;
using KvizHub.Api.Data.Entities;
using KvizHub.Shared.Enums;
using KvizHub.Shared.Events;
using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Services
{
    public class EventStatusService(KvizHubDbContext db)
    {
        // Moves the event to the target state; returns an error message, or null on success
        public async Task<string?> ChangeStatusAsync(QuizEvent ev, EventStatus target)
        {
            if (EventStatusFlow.Next(ev.Status) != target)
                return "Dogodek je mogoče premakniti samo v naslednje stanje.";

            if (target == EventStatus.Published)
            {
                if (!await db.Sections.AnyAsync(s => s.EventId == ev.Id))
                    return "Dogodek mora imeti vsaj en sklop.";
                if (ev.StartsAt <= DateTime.UtcNow)
                    return "Začetek dogodka mora biti v prihodnosti.";
            }

            if (target == EventStatus.Finished &&
                await db.Reservations.AnyAsync(r => r.EventId == ev.Id && r.Status == ReservationStatus.Confirmed))
                return "Pred zaključkom označite prisotnost vseh ekip.";

            ev.Status = target;
            await db.SaveChangesAsync();
            return null;
        }
    }
}
