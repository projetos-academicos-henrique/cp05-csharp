using System.ComponentModel.DataAnnotations;
using PokemonNationalDex.API.Models;

namespace PokemonNationalDex.API.DTOs.DexEntry;

public class UpdateDexEntryDto
{
    [Required(ErrorMessage = "O identificador do Pokémon é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O PokemonId deve ser válido.")]
    public int PokemonId { get; set; }

    [Required(ErrorMessage = "O status de rastreamento é obrigatório.")]
    [EnumDataType(typeof(DexStatus), ErrorMessage = "Status inválido. Use 1 para 'Seen' ou 2 para 'Caught'.")]
    public DexStatus Status { get; set; }

    public bool IsShiny { get; set; }

    public DateTime? CaughtDate { get; set; }

    [StringLength(150, ErrorMessage = "O local de captura deve ter no máximo 150 caracteres.")]
    public string? CaughtLocation { get; set; }

    [StringLength(80, ErrorMessage = "A versão do jogo deve ter no máximo 80 caracteres.")]
    public string? GameVersion { get; set; }

    [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres.")]
    public string? Notes { get; set; }
}
