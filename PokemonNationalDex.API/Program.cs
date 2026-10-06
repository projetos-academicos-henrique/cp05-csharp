using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PokemonNationalDex.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados SQLite com Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=pokemondex.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Configuração dos Controllers com serialização de enums em texto
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pokemon National Dex Tracker API",
        Version = "v1",
        Description = "API RESTful com Entity Framework Core (.NET 10) para rastreamento de progresso e capturas da Pokédex Nacional.",
        Contact = new OpenApiContact
        {
            Name = "CP5 C# Software Development Team",
        }
    });
});

var app = builder.Build();

// Aplicar Migrations e Seeds automaticamente na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configuração do pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon National Dex Tracker API v1");
    c.RoutePrefix = "swagger";
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full);
});

// Redireciona a raiz para a documentação do Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseAuthorization();
app.MapControllers();

app.Run();
