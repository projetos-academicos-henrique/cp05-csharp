using System.ComponentModel.DataAnnotations;

namespace PokemonNationalDex.API.Models;

public class DexEntry
{
    public int Id { get; set; }

    [Required]
    public int PokemonId { get; set; }

    public Pokemon? Pokemon { get; set; }

    [Required]
    public DexStatus Status { get; set; } = DexStatus.Seen;

    public bool IsShiny { get; set; }

    public DateTime CaughtDate { get; set; } = DateTime.UtcNow;

    [MaxLength(150)]
    public string? CaughtLocation { get; set; }

    [MaxLength(80)]
    public string? GameVersion { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
