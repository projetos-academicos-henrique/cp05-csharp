using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonNationalDex.API.Data;
using PokemonNationalDex.API.DTOs.DexEntry;
using PokemonNationalDex.API.DTOs.Pokemon;
using PokemonNationalDex.API.Models;

namespace PokemonNationalDex.API.Controllers;

[ApiController]
[Route("api/v1/dex-entries")]
[Produces("application/json")]
public class DexEntriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public DexEntriesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lista todos os registros de rastreamento com filtros opcionais.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DexEntryResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] DexStatus? status,
        [FromQuery] bool? isShiny,
        [FromQuery] string? gameVersion)
    {
        var query = _context.DexEntries
            .Include(d => d.Pokemon)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(d => d.Status == status.Value);
        }

        if (isShiny.HasValue)
        {
            query = query.Where(d => d.IsShiny == isShiny.Value);
        }

        if (!string.IsNullOrWhiteSpace(gameVersion))
        {
            var trimmedVersion = gameVersion.Trim().ToLower();
            query = query.Where(d => d.GameVersion != null && d.GameVersion.ToLower().Contains(trimmedVersion));
        }

        var entries = await query
            .OrderByDescending(d => d.CaughtDate)
            .Select(d => new DexEntryResponseDto
            {
                Id = d.Id,
                PokemonId = d.PokemonId,
                Status = d.Status,
                IsShiny = d.IsShiny,
                CaughtDate = d.CaughtDate,
                CaughtLocation = d.CaughtLocation,
                GameVersion = d.GameVersion,
                Notes = d.Notes,
                Pokemon = d.Pokemon != null ? new PokemonResponseDto
                {
                    Id = d.Pokemon.Id,
                    DexNumber = d.Pokemon.DexNumber,
                    Name = d.Pokemon.Name,
                    PrimaryType = d.Pokemon.PrimaryType,
                    SecondaryType = d.Pokemon.SecondaryType,
                    Generation = d.Pokemon.Generation,
                    IsLegendary = d.Pokemon.IsLegendary,
                    SpriteUrl = d.Pokemon.SpriteUrl
                } : null
            })
            .ToListAsync();

        return Ok(entries);
    }

    /// <summary>
    /// Obtém um registro de rastreamento específico por ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DexEntryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var entry = await _context.DexEntries
            .Include(d => d.Pokemon)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entry == null)
        {
            return NotFound(new { message = $"Registro de rastreamento #{id} não foi encontrado." });
        }

        var response = new DexEntryResponseDto
        {
            Id = entry.Id,
            PokemonId = entry.PokemonId,
            Status = entry.Status,
            IsShiny = entry.IsShiny,
            CaughtDate = entry.CaughtDate,
            CaughtLocation = entry.CaughtLocation,
            GameVersion = entry.GameVersion,
            Notes = entry.Notes,
            Pokemon = entry.Pokemon != null ? new PokemonResponseDto
            {
                Id = entry.Pokemon.Id,
                DexNumber = entry.Pokemon.DexNumber,
                Name = entry.Pokemon.Name,
                PrimaryType = entry.Pokemon.PrimaryType,
                SecondaryType = entry.Pokemon.SecondaryType,
                Generation = entry.Pokemon.Generation,
                IsLegendary = entry.Pokemon.IsLegendary,
                SpriteUrl = entry.Pokemon.SpriteUrl
            } : null
        };

        return Ok(response);
    }

    /// <summary>
    /// Retorna estatísticas de conclusão e progresso da National Pokédex.
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(DexStatsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats()
    {
        var totalPokemons = await _context.Pokemons.CountAsync();
        var totalTracked = await _context.DexEntries.CountAsync();
        var totalCaught = await _context.DexEntries.CountAsync(d => d.Status == DexStatus.Caught);
        var totalSeen = await _context.DexEntries.CountAsync(d => d.Status == DexStatus.Seen);
        var totalShinies = await _context.DexEntries.CountAsync(d => d.IsShiny);

        double caughtPercentage = totalPokemons > 0
            ? Math.Round(((double)totalCaught / totalPokemons) * 100, 2)
            : 0;

        double seenPercentage = totalPokemons > 0
            ? Math.Round(((double)(totalCaught + totalSeen) / totalPokemons) * 100, 2)
            : 0;

        var stats = new DexStatsResponseDto
        {
            TotalPokemonsInDex = totalPokemons,
            TotalTracked = totalTracked,
            TotalCaught = totalCaught,
            TotalSeen = totalSeen,
            TotalShinies = totalShinies,
            CaughtCompletionPercentage = caughtPercentage,
            SeenCompletionPercentage = seenPercentage
        };

        return Ok(stats);
    }

    /// <summary>
    /// Registra uma nova captura ou avistamento de Pokémon.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DexEntryResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDexEntryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pokemon = await _context.Pokemons.FindAsync(dto.PokemonId);
        if (pokemon == null)
        {
            return BadRequest(new { message = $"O Pokémon com ID {dto.PokemonId} não existe no banco de dados." });
        }

        var entry = new DexEntry
        {
            PokemonId = dto.PokemonId,
            Status = dto.Status,
            IsShiny = dto.IsShiny,
            CaughtDate = dto.CaughtDate ?? DateTime.UtcNow,
            CaughtLocation = dto.CaughtLocation?.Trim(),
            GameVersion = dto.GameVersion?.Trim(),
            Notes = dto.Notes?.Trim()
        };

        _context.DexEntries.Add(entry);
        await _context.SaveChangesAsync();

        var response = new DexEntryResponseDto
        {
            Id = entry.Id,
            PokemonId = entry.PokemonId,
            Status = entry.Status,
            IsShiny = entry.IsShiny,
            CaughtDate = entry.CaughtDate,
            CaughtLocation = entry.CaughtLocation,
            GameVersion = entry.GameVersion,
            Notes = entry.Notes,
            Pokemon = new PokemonResponseDto
            {
                Id = pokemon.Id,
                DexNumber = pokemon.DexNumber,
                Name = pokemon.Name,
                PrimaryType = pokemon.PrimaryType,
                SecondaryType = pokemon.SecondaryType,
                Generation = pokemon.Generation,
                IsLegendary = pokemon.IsLegendary,
                SpriteUrl = pokemon.SpriteUrl
            }
        };

        return CreatedAtAction(nameof(GetById), new { id = entry.Id }, response);
    }

    /// <summary>
    /// Atualiza um registro de rastreamento existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateDexEntryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entry = await _context.DexEntries.FindAsync(id);
        if (entry == null)
        {
            return NotFound(new { message = $"Registro de rastreamento #{id} não foi encontrado." });
        }

        var pokemonExists = await _context.Pokemons.AnyAsync(p => p.Id == dto.PokemonId);
        if (!pokemonExists)
        {
            return BadRequest(new { message = $"O Pokémon com ID {dto.PokemonId} não existe." });
        }

        entry.PokemonId = dto.PokemonId;
        entry.Status = dto.Status;
        entry.IsShiny = dto.IsShiny;
        if (dto.CaughtDate.HasValue)
        {
            entry.CaughtDate = dto.CaughtDate.Value;
        }
        entry.CaughtLocation = dto.CaughtLocation?.Trim();
        entry.GameVersion = dto.GameVersion?.Trim();
        entry.Notes = dto.Notes?.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Exclui um registro de rastreamento.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var entry = await _context.DexEntries.FindAsync(id);
        if (entry == null)
        {
            return NotFound(new { message = $"Registro de rastreamento #{id} não foi encontrado." });
        }

        _context.DexEntries.Remove(entry);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
