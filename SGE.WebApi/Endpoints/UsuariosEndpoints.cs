using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Comun;

namespace SGE.WebApi.Endpoints;

public static class UsuariosEndpoints
{
    public static IEndpointRouteBuilder MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/usuarios").WithTags("Usuarios");

        grupo.MapPost("/login", (LoginRequest request, LoginUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Ok(response);
        })
        .WithName("Login")
        .AllowAnonymous();

        grupo.MapPost("/registro", (RegistrarUsuarioRequest request, RegistrarUsuarioUseCase useCase) =>
        {
            var response = useCase.Ejecutar(request);
            return Results.Created($"/api/usuarios/{response.Id}", response);
        })
        .WithName("RegistrarUsuario")
        .AllowAnonymous();

        grupo.MapPut("/me", (ModificarMisDatosRequest request, HttpContext context, ModificarMisDatosUseCase useCase) =>
        {
            var idDelToken = Guid.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);

            var requestConIdReal = request with { IdUsuarioActivo = idDelToken };

            useCase.Ejecutar(requestConIdReal);
            return Results.NoContent();
        })
        .WithName("ModificarMisDatos")
        .RequireAuthorization();

        grupo.MapGet("/", (HttpContext context, ListarUsuariosUseCase useCase) =>
        {
            var idDelToken = Guid.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
            var response = useCase.Ejecutar(idDelToken);
            return Results.Ok(response);
        })
        .WithName("ListarUsuarios")
        .RequireAuthorization();

        grupo.MapDelete("/{id:guid}", (Guid id, HttpContext context, EliminarUsuarioUseCase useCase) =>
        {
            var idDelToken = Guid.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
            useCase.Ejecutar(new EliminarUsuarioRequest(id, idDelToken));
            return Results.NoContent();
        })
        .WithName("EliminarUsuario")
        .RequireAuthorization();

        grupo.MapPatch("/{id:guid}/permisos", (Guid id, IEnumerable<Permiso> nuevosPermisos, HttpContext context, ModificarPermisosUsuarioUseCase useCase) =>
        {
            var idDelToken = Guid.Parse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
            useCase.Ejecutar(new ModificarPermisosRequest(id, idDelToken, nuevosPermisos));
            return Results.NoContent();
        })
        .WithName("ModificarPermisosUsuario")
        .RequireAuthorization();

        return app;
    }
}