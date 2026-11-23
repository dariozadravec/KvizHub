using System.ComponentModel.DataAnnotations;

namespace KvizHub.Shared.Events;

public class SectionInput
{
    [Required(ErrorMessage = "Ime sklopa je obvezno.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Tema sklopa je obvezna.")]
    public string Topic { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Največje število točk mora biti večje od 0.")]
    public int MaxPoints { get; set; }
}