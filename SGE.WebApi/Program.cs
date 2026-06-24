using Microsoft.EntityFrameworkCore;
using SGE.Infraestructura;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar SGEDbContext
builder.Services.AddDbContext<SGEDbContext>(options =>
    options.UseSqlite("Data Source=SGE.sqlite"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Crear la base de datos y sembrar datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGEDbContext>();
    context.Database.EnsureCreated();
    
    // Configurar journal_mode=DELETE como pide el TP
    var connection = context.Database.GetDbConnection();
    connection.Open();
    using (var command = connection.CreateCommand())
    {
        command.CommandText = "PRAGMA journal_mode=DELETE;";
        command.ExecuteNonQuery();
    }
    
    context.SembrarDatos();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.Run();