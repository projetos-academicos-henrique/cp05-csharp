using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonNationalDex.API.Data;
using PokemonNationalDex.API.DTOs.Pokemon;
using PokemonNationalDex.API.Models;

namespace PokemonNationalDex.API.Controllers;

[ApiController]
[Route("api/v1/pokemons")]
[Produces("application/json")]
public class PokemonsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PokemonsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista os Pokémon cadastrados na Pokédex com filtros opcionais e paginação.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PokemonResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] int? generation,
        [FromQuery] string? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 50) pageSize = 10;

        var query = _context.Pokemons.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var trimmedName = name.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(trimmedName));
        }

        if (generation.HasValue)
        {
            query = query.Where(p => p.Generation == generation.Value);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            var trimmedType = type.Trim().ToLower();
            query = query.Where(p => p.PrimaryType.ToLower() == trimmedType ||
                                     (p.SecondaryType != null && p.SecondaryType.ToLower() == trimmedType));
        }

        var pokemons = await query
            .OrderBy(p => p.DexNumber)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PokemonResponseDto
            {
                Id = p.Id,
                DexNumber = p.DexNumber,
                Name = p.Name,
                PrimaryType = p.PrimaryType,
                SecondaryType = p.SecondaryType,
                Generation = p.Generation,
                IsLegendary = p.IsLegendary,
                SpriteUrl = p.SpriteUrl
            })
            .ToListAsync();

        return Ok(pokemons);
    }

    /// <summary>
    /// Obtém detalhes de um Pokémon pelo seu ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PokemonResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var pokemon = await _context.Pokemons.FindAsync(id);

        if (pokemon == null)
        {
            return NotFound(new { message = $"Pokémon com ID {id} não foi encontrado na base de dados." });
        }

        var response = new PokemonResponseDto
        {
            Id = pokemon.Id,
            DexNumber = pokemon.DexNumber,
            Name = pokemon.Name,
            PrimaryType = pokemon.PrimaryType,
            SecondaryType = pokemon.SecondaryType,
            Generation = pokemon.Generation,
            IsLegendary = pokemon.IsLegendary,
            SpriteUrl = pokemon.SpriteUrl
        };

        return Ok(response);
    }

    /// <summary>
    /// Cadastra um novo Pokémon na Pokédex Nacional.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PokemonResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePokemonDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var dexExists = await _context.Pokemons.AnyAsync(p => p.DexNumber == dto.DexNumber);
        if (dexExists)
        {
            return BadRequest(new { message = $"Já existe um Pokémon cadastrado com o número #{dto.DexNumber} na National Dex." });
        }

        var pokemon = new Pokemon
        {
            DexNumber = dto.DexNumber,
            Name = dto.Name.Trim(),
            PrimaryType = dto.PrimaryType.Trim(),
            SecondaryType = string.IsNullOrWhiteSpace(dto.SecondaryType) ? null : dto.SecondaryType.Trim(),
            Generation = dto.Generation,
            IsLegendary = dto.IsLegendary,
            SpriteUrl = dto.SpriteUrl?.Trim()
        };

        _context.Pokemons.Add(pokemon);
        await _context.SaveChangesAsync();

        var response = new PokemonResponseDto
        {
            Id = pokemon.Id,
            DexNumber = pokemon.DexNumber,
            Name = pokemon.Name,
            PrimaryType = pokemon.PrimaryType,
            SecondaryType = pokemon.SecondaryType,
            Generation = pokemon.Generation,
            IsLegendary = pokemon.IsLegendary,
            SpriteUrl = pokemon.SpriteUrl
        };

        return CreatedAtAction(nameof(GetById), new { id = pokemon.Id }, response);
    }

    /// <summary>
    /// Atualiza as informações de um Pokémon existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePokemonDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pokemon = await _context.Pokemons.FindAsync(id);
        if (pokemon == null)
        {
            return NotFound(new { message = $"Pokémon com ID {id} não foi encontrado." });
        }

        var dexConflict = await _context.Pokemons.AnyAsync(p => p.DexNumber == dto.DexNumber && p.Id != id);
        if (dexConflict)
        {
            return BadRequest(new { message = $"O número #{dto.DexNumber} já está em uso por outro Pokémon." });
        }

        pokemon.DexNumber = dto.DexNumber;
        pokemon.Name = dto.Name.Trim();
        pokemon.PrimaryType = dto.PrimaryType.Trim();
        pokemon.SecondaryType = string.IsNullOrWhiteSpace(dto.SecondaryType) ? null : dto.SecondaryType.Trim();
        pokemon.Generation = dto.Generation;
        pokemon.IsLegendary = dto.IsLegendary;
        pokemon.SpriteUrl = dto.SpriteUrl?.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Remove um Pokémon da Pokédex Nacional.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var pokemon = await _context.Pokemons.FindAsync(id);
        if (pokemon == null)
        {
            return NotFound(new { message = $"Pokémon com ID {id} não foi encontrado." });
        }

        _context.Pokemons.Remove(pokemon);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
