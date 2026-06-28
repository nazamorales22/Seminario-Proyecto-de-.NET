using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

namespace SGE.WebApi.Endpoints;

public record AltaTramiteBody(Guid ExpedienteId, EtiquetaTramite Etiqueta, string Contenido);
public record ModificarTramiteBody(EtiquetaTramite Etiqueta, string Contenido);

public static class TramitesEndpoints
{
    public static IEndpointRouteBuilder MapTramitesEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/tramites").WithTags("Trámites");

        // GET /api/tramites?expedienteId={id} - Listar trámites de un expediente
        grupo.MapGet("/", (Guid expedienteId, ListarTramitesUseCase useCase) =>
        {
            var resultado = useCase.Ejecutar(expedienteId);
            return Results.Ok(resultado);
        })
        .WithName("ListarTramitesPorExpediente")
        .RequireAuthorization();

        // POST /api/tramites - Alta de trámite
        grupo.MapPost("/", (AltaTramiteBody body, HttpContext context, AltaTramiteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            var request = new AltaTramiteRequest(
                ExpedienteId: body.ExpedienteId,
                Etiqueta: body.Etiqueta,
                Contenido: body.Contenido,
                IdUsuario: idUsuario);
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/tramites/{response.Id}", response);
        })
        .WithName("AltaTramite")
        .RequireAuthorization();

        // PUT /api/tramites/{id} - Modificar trámite
        grupo.MapPut("/{id:guid}", (Guid id, ModificarTramiteBody body, HttpContext context, ModificarTramiteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            var request = new ModificarTramiteRequest(
                Id: id,
                Etiqueta: body.Etiqueta,
                Contenido: body.Contenido,
                IdUsuario: idUsuario);
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        })
        .WithName("ModificarTramite")
        .RequireAuthorization();

        // DELETE /api/tramites/{id} - Baja de trámite
        grupo.MapDelete("/{id:guid}", (Guid id, HttpContext context, BajaTramiteUseCase useCase) =>
        {
            var idUsuario = ObtenerIdUsuario(context);
            useCase.Ejecutar(new BajaTramiteRequest(id, idUsuario));
            return Results.NoContent();
        })
        .WithName("BajaTramite")
        .RequireAuthorization();

        return app;
    }

    private static Guid ObtenerIdUsuario(HttpContext context)
        => Guid.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
}