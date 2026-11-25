using System.ComponentModel.DataAnnotations;

namespace KvizHub.Shared.Reservations;

public class CreateReservationRequest : IValidatableObject
{
    // Existing team of the signed-in user, or null when a new team is created
    public int? TeamId { get; set; }

    public string? NewTeamName { get; set; }

    [Range(1, 6, ErrorMessage = "Število članov mora biti od 1 do 6.")]
    public int TeamSize { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TeamId is null && string.IsNullOrWhiteSpace(NewTeamName))
            yield return new ValidationResult("Izberite ekipo ali vpišite ime nove ekipe.", [nameof(NewTeamName)]);
    }
}