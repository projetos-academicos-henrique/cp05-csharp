using Microsoft.EntityFrameworkCore;
using PokemonNationalDex.API.Models;

namespace PokemonNationalDex.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pokemon> Pokemons => Set<Pokemon>();
    public DbSet<DexEntry> DexEntries => Set<DexEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento e índices
        modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.DexNumber).IsUnique();
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.PrimaryType).IsRequired().HasMaxLength(50);
            entity.Property(p => p.SecondaryType).HasMaxLength(50);
            entity.Property(p => p.SpriteUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<DexEntry>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.CaughtLocation).HasMaxLength(150);
            entity.Property(d => d.GameVersion).HasMaxLength(80);
            entity.Property(d => d.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Pokemon)
                  .WithMany(p => p.DexEntries)
                  .HasForeignKey(d => d.PokemonId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed inicial de Pokémons icônicos para o rastreador
        modelBuilder.Entity<Pokemon>().HasData(
            new Pokemon
            {
                Id = 1,
                DexNumber = 1,
                Name = "Bulbasaur",
                PrimaryType = "Grass",
                SecondaryType = "Poison",
                Generation = 1,
                IsLegendary = false,
                SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png"
            },
            new Pokemon
            {
                Id = 2,
                DexNumber = 4,
                Name = "Charmander",
                PrimaryType = "Fire",
                SecondaryType = null,
                Generation = 1,
                IsLegendary = false,
                SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/4.png"
            },
            new Pokemon
            {
                Id = 3,
                DexNumber = 7,
                Name = "Squirtle",
                PrimaryType = "Water",
                SecondaryType = null,
                Generation = 1,
                IsLegendary = false,
                SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/7.png"
            },
            new Pokemon
            {
                Id = 4,
                DexNumber = 25,
                Name = "Pikachu",
                PrimaryType = "Electric",
                SecondaryType = null,
                Generation = 1,
                IsLegendary = false,
                SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png"
            },
            new Pokemon
            {
                Id = 5,
                DexNumber = 94,
                Name = "Gengar",
                PrimaryType = "Ghost",
                SecondaryType = "Poison",
                Generation = 1,
                IsLegendary = false,
                SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/94.png"
            },
            new Pokemon
            {
                Id = 6,
                DexNumber = 150,
                Name = "Mewtwo",
                PrimaryType = "Psychic",
                SecondaryType = null,
                Generation = 1,
                IsLegendary = true,
                SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/150.png"
            }
        );

        // Seed inicial de entradas de rastreamento
        modelBuilder.Entity<DexEntry>().HasData(
            new DexEntry
            {
                Id = 1,
                PokemonId = 1,
                Status = DexStatus.Caught,
                IsShiny = false,
                CaughtDate = new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                CaughtLocation = "Pallet Town",
                GameVersion = "FireRed",
                Notes = "Starter escolhido no laboratório do Professor Oak."
            },
            new DexEntry
            {
                Id = 2,
                PokemonId = 4,
                Status = DexStatus.Caught,
                IsShiny = true,
                CaughtDate = new DateTime(2026, 2, 20, 14, 0, 0, DateTimeKind.Utc),
                CaughtLocation = "Viridian Forest",
                GameVersion = "FireRed",
                Notes = "Pikachu shiny capturado após muitas buscas!"
            },
            new DexEntry
            {
                Id = 3,
                PokemonId = 6,
                Status = DexStatus.Seen,
                IsShiny = false,
                CaughtDate = new DateTime(2026, 3, 5, 18, 45, 0, DateTimeKind.Utc),
                CaughtLocation = "Cerulean Cave",
                GameVersion = "FireRed",
                Notes = "Avistado nas profundezas da caverna."
            }
        );
    }
}
