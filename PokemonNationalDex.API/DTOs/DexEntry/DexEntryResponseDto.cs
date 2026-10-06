using PokemonNationalDex.API.DTOs.Pokemon;
using PokemonNationalDex.API.Models;

namespace PokemonNationalDex.API.DTOs.DexEntry;

public class DexEntryResponseDto
{
    public int Id { get; set; }
    public int PokemonId { get; set; }
    public PokemonResponseDto? Pokemon { get; set; }
    public DexStatus Status { get; set; }
    public string StatusDescription => Status.ToString();
    public bool IsShiny { get; set; }
    public DateTime CaughtDate { get; set; }
    public string? CaughtLocation { get; set; }
    public string? GameVersion { get; set; }
    public string? Notes { get; set; }
}
