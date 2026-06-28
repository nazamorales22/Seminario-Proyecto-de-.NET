using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;

namespace SGE.WebApi.Endpoints;


//con esto defino que pido en cada enpoint 
public record AltaExpedienteBody(string Caratula);
public record ModificarExpedienteBody(string NuevaCaratula);
public record CambiarEstadoExpedienteBody(EstadoExpediente NuevoEstado);

public static class ExpedientesEndpoints
{
    public static IEndpointRouteBuilder MapExpedientesEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/expedientes").WithTags("Expedientes");

        // GET /api/expedientes - Listar todos
        grupo.MapGet("/", (ListarExpedientesUseCase useCase) =>
        {
            var resultado = useCase.Ejecutar();
            return Results.Ok(resultado);
        })
        .WithName("ListarExpedientes")
        .RequireAuthorization();

        // GET /api/expedientes/{id} - Obtener por Id con trámites
        grupo.MapGet("/{id:guid}", (Guid id, ObtenerExpedientePorIdUseCase useCase) =>
        {
            var resultado = useCase.Ejecutar(id);
            return Results.Ok(resultado);
        })
        .WithName("ObtenerExpedientePorId")
        .RequireAuthorization();

        // POST /api/expedientes - Alta
        grupo.MapPost("/", (AltaExpedienteBody body, HttpContext context, AltaExpedienteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            var request = new AltaExpedienteRequest(IdUsuario: idUsuario, Caratula: body.Caratula);
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/expedientes/{response.Id}", response);
        })
        .WithName("AltaExpediente")
        .RequireAuthorization();

        // PUT /api/expedientes/{id} - Modificar carátula
        grupo.MapPut("/{id:guid}", (Guid id, ModificarExpedienteBody body, HttpContext context, ModificarExpedienteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            var request = new ModificarExpedienteRequest(Id: id, NuevaCaratula: body.NuevaCaratula, IdUsuario: idUsuario);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        })
        .WithName("ModificarExpediente")
        .RequireAuthorization();

        // PATCH /api/expedientes/{id}/estado - Cambiar estado manual
        grupo.MapPatch("/{id:guid}/estado", (Guid id, CambiarEstadoExpedienteBody body, HttpContext context, CambiarEstadoExpedienteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            var request = new CambiarEstadoExpedienteRequest(Id: id, NuevoEstado: body.NuevoEstado, IdUsuario: idUsuario);
            useCase.Ejecutar(request);
            return Results.NoContent();
        })
        .WithName("CambiarEstadoExpediente")
        .RequireAuthorization();

        // DELETE /api/expedientes/{id} - Baja en cascada
        grupo.MapDelete("/{id:guid}", (Guid id, HttpContext context, BajaExpedienteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            useCase.Ejecutar(new BajaExpedienteRequest(id, idUsuario));
            return Results.NoContent();
        })
        .WithName("BajaExpediente")
        .RequireAuthorization();

        // GET /api/expedientes/por-estado - Listar por estado
        grupo.MapGet("/por-estado", (EstadoExpediente estado, ListarExpedientesPorEstadoUseCase useCase) =>
        {
            var resultado = useCase.Ejecutar(new ListarExpedientesPorEstadoRequest(estado));
            return Results.Ok(resultado);
        })
        .WithName("ListarExpedientesPorEstado")
        .RequireAuthorization();

        return app;
    }

    private static Guid ObtenerIdUsuario(HttpContext context)
        => Guid.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
}