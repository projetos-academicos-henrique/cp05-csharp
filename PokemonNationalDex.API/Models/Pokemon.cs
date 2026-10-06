using System.ComponentModel.DataAnnotations;

namespace PokemonNationalDex.API.Models;

public class Pokemon
{
    public int Id { get; set; }

    [Required]
    [Range(1, 1500)]
    public int DexNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PrimaryType { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? SecondaryType { get; set; }

    [Range(1, 9)]
    public int Generation { get; set; } = 1;

    public bool IsLegendary { get; set; }

    [MaxLength(500)]
    public string? SpriteUrl { get; set; }

    public ICollection<DexEntry> DexEntries { get; set; } = new List<DexEntry>();
}
