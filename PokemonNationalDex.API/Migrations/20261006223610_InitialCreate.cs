using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokemonNationalDex.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DexNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PrimaryType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SecondaryType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Generation = table.Column<int>(type: "INTEGER", nullable: false),
                    IsLegendary = table.Column<bool>(type: "INTEGER", nullable: false),
                    SpriteUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DexEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsShiny = table.Column<bool>(type: "INTEGER", nullable: false),
                    CaughtDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CaughtLocation = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    GameVersion = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DexEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DexEntries_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "Id", "DexNumber", "Generation", "IsLegendary", "Name", "PrimaryType", "SecondaryType", "SpriteUrl" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "Bulbasaur", "Grass", "Poison", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png" },
                    { 2, 4, 1, false, "Charmander", "Fire", null, "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/4.png" },
                    { 3, 7, 1, false, "Squirtle", "Water", null, "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/7.png" },
                    { 4, 25, 1, false, "Pikachu", "Electric", null, "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png" },
                    { 5, 94, 1, false, "Gengar", "Ghost", "Poison", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/94.png" },
                    { 6, 150, 1, true, "Mewtwo", "Psychic", null, "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/150.png" }
                });

            migrationBuilder.InsertData(
                table: "DexEntries",
                columns: new[] { "Id", "CaughtDate", "CaughtLocation", "GameVersion", "IsShiny", "Notes", "PokemonId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "Pallet Town", "FireRed", false, "Starter escolhido no laboratório do Professor Oak.", 1, 2 },
                    { 2, new DateTime(2026, 2, 20, 14, 0, 0, 0, DateTimeKind.Utc), "Viridian Forest", "FireRed", true, "Pikachu shiny capturado após muitas buscas!", 4, 2 },
                    { 3, new DateTime(2026, 3, 5, 18, 45, 0, 0, DateTimeKind.Utc), "Cerulean Cave", "FireRed", false, "Avistado nas profundezas da caverna.", 6, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DexEntries_PokemonId",
                table: "DexEntries",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemons_DexNumber",
                table: "Pokemons",
                column: "DexNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DexEntries");

            migrationBuilder.DropTable(
                name: "Pokemons");
        }
    }
}
