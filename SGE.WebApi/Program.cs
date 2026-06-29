using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SGE.Infraestructura;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Aplicacion.Usuarios;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion;
using Scalar.AspNetCore;
using SGE.WebApi.Middlewares;
using SGE.WebApi.Servicios;
using SGE.WebApi.Endpoints;
using SGE.WebApi.Configuracion;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<SGEDbContext>(options =>
    options.UseSqlite("Data Source=SGE.sqlite"));

// Unit of Work y Repositorios
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<IExpedienteRepository, ExpedienteRepositorySql>();
builder.Services.AddScoped<ITramiteRepository, TramiteRepositorySql>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositorySql>();

// Servicio de autorización
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();

// Servicio de actualización de estado
builder.Services.AddScoped<ActualizacionEstadoExpedienteService>();

// Casos de uso de Expedientes
builder.Services.AddScoped<AltaExpedienteUseCase>();
builder.Services.AddScoped<BajaExpedienteUseCase>();
builder.Services.AddScoped<ModificarExpedienteUseCase>();
builder.Services.AddScoped<CambiarEstadoExpedienteUseCase>();
builder.Services.AddScoped<ListarExpedientesUseCase>();
builder.Services.AddScoped<ListarExpedientesPorEstadoUseCase>();
builder.Services.AddScoped<ConsultarExpedientesPorEtiquetaUseCase>();
builder.Services.AddScoped<ObtenerExpedientePorIdUseCase>();

// Casos de uso de Trámites
builder.Services.AddScoped<AltaTramiteUseCase>();
builder.Services.AddScoped<BajaTramiteUseCase>();
builder.Services.AddScoped<ModificarTramiteUseCase>();
builder.Services.AddScoped<ListarTramitesUseCase>();

// Casos de uso de Usuarios
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegistrarUsuarioUseCase>();
builder.Services.AddScoped<ListarUsuariosUseCase>();
builder.Services.AddScoped<EliminarUsuarioUseCase>();
builder.Services.AddScoped<ModificarPermisosUsuarioUseCase>();
builder.Services.AddScoped<ModificarMisDatosUseCase>();

builder.Services.AddExceptionHandler<ManejadorGlobalDeExcepciones>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IHasher, Sha256Hasher>();

builder.Services.AddScoped<ITokenService, TokenService>();


// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});


//para que permita texto en vez de los nros en los permisos 
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});


var app = builder.Build();

// Inicializar base de datos y sembrar datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SGEDbContext>();
    context.Database.EnsureCreated();
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
    app.MapScalarApiReference(options =>
    {
        options.WithPreferredScheme("Bearer")
               .WithHttpBearerAuthentication(bearer =>
               {
                   bearer.Token = "";
               });
    });//para el token
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapUsuariosEndpoints();
app.MapExpedientesEndpoints();
app.MapTramitesEndpoints();

app.MapGet("/", () => "¡La API está funcionando!");

app.Run();