namespace PokemonNationalDex.API.DTOs.Pokemon;

public class PokemonResponseDto
{
    public int Id { get; set; }
    public int DexNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrimaryType { get; set; } = string.Empty;
    public string? SecondaryType { get; set; }
    public int Generation { get; set; }
    public bool IsLegendary { get; set; }
    public string? SpriteUrl { get; set; }
}
