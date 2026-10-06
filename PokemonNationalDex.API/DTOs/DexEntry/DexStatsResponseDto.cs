namespace PokemonNationalDex.API.DTOs.DexEntry;

public class DexStatsResponseDto
{
    public int TotalPokemonsInDex { get; set; }
    public int TotalTracked { get; set; }
    public int TotalCaught { get; set; }
    public int TotalSeen { get; set; }
    public int TotalShinies { get; set; }
    public double CaughtCompletionPercentage { get; set; }
    public double SeenCompletionPercentage { get; set; }
}
