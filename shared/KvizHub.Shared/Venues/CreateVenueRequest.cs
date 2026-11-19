using System.ComponentModel.DataAnnotations;

namespace KvizHub.Shared.Venues;

public class CreateVenueRequest
{
    [Required(ErrorMessage = "Ime lokala je obvezno.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Ulica je obvezna.")]
    public string Street { get; set; } = "";

    [Required(ErrorMessage = "Poštna številka je obvezna.")]
    public string PostalCode { get; set; } = "";

    [Required(ErrorMessage = "Kraj je obvezen.")]
    public string City { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Kapaciteta mora biti večja od 0.")]
    public int DefaultCapacity { get; set; }

    // Set to true after the user confirms the warning about another venue at the same address
    public bool ConfirmSameAddress { get; set; }
}