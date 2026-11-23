using System.ComponentModel.DataAnnotations;

namespace KvizHub.Shared.Events;

public class SaveEventRequest
{
    [Required(ErrorMessage = "Naslov je obvezen.")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Datum in ura začetka sta obvezna.")]
    public DateTimeOffset? StartsAt { get; set; }

    [Required(ErrorMessage = "Lokal je obvezen.")]
    public int? VenueId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Kapaciteta mora biti večja od 0.")]
    public int Capacity { get; set; }

    public bool JokerEnabled { get; set; }

    // Sections in the order of the quiz; positions are assigned from this order
    [MinLength(1, ErrorMessage = "Dogodek mora imeti vsaj en sklop.")]
    public List<SectionInput> Sections { get; set; } = [];
}