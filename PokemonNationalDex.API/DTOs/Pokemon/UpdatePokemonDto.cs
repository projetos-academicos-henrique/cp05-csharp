using System.ComponentModel.DataAnnotations;

namespace PokemonNationalDex.API.DTOs.Pokemon;

public class UpdatePokemonDto
{
    [Required(ErrorMessage = "O número da National Dex é obrigatório.")]
    [Range(1, 1500, ErrorMessage = "O número da Dex deve estar entre 1 e 1500.")]
    public int DexNumber { get; set; }

    [Required(ErrorMessage = "O nome do Pokémon é obrigatório.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo primário é obrigatório.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O tipo primário deve ter entre 2 e 50 caracteres.")]
    public string PrimaryType { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "O tipo secundário deve ter no máximo 50 caracteres.")]
    public string? SecondaryType { get; set; }

    [Range(1, 9, ErrorMessage = "A geração deve estar entre 1 e 9.")]
    public int Generation { get; set; } = 1;

    public bool IsLegendary { get; set; }

    [Url(ErrorMessage = "A URL da imagem/sprite deve ser válida.")]
    [StringLength(500, ErrorMessage = "A URL deve ter no máximo 500 caracteres.")]
    public string? SpriteUrl { get; set; }
}
