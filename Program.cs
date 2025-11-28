using Microsoft.EntityFrameworkCore;
using escuchify_api.Modelos;
using escuchify_api.Repositorios;
using escuchify_api.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// 1. Configurar DB con PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 2. Inyección de Dependencias
builder.Services.AddScoped<IArtistaRepository, ArtistaRepository>();
builder.Services.AddScoped<IDiscoRepository, DiscoRepository>();
builder.Services.AddScoped<ICancionRepository, CancionRepository>();

builder.Services.AddScoped<ArtistaService>();
builder.Services.AddScoped<DiscoService>();
builder.Services.AddScoped<CancionService>();

// 3. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// 4. Controladores
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

// Crear DB si no existe
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Esta línea aplicará la migración y creará la BD si hace falta
    dbContext.Database.Migrate(); 
}

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 5. Activar las rutas de los Controladores
app.MapControllers();

app.Run();