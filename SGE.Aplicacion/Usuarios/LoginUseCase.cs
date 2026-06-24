using SGE.Dominio.Comun;

namespace SGE.Aplicacion.Usuarios;

public record LoginRequest(string Correo, string Contrasena);
public record LoginResponse(Guid UserId, string Nombre, string Correo, bool EsAdministrador, string Token);

public class LoginUseCase(IUsuarioRepository repo, IHasher hasher, ITokenService tokenService)
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Correo) || string.IsNullOrWhiteSpace(request.Contrasena))
            throw new DominioException("El correo y la contraseña son obligatorios.");

        var usuario = repo.ObtenerPorCorreo(request.Correo)
            ?? throw new DominioException("Credenciales inválidas.");

        var hashIngresado = hasher.Hashear(request.Contrasena);
        if (usuario.ContrasenaHash != hashIngresado)
            throw new DominioException("Credenciales inválidas.");

        var token = tokenService.GenerarToken(usuario);

        return new LoginResponse(
            usuario.Id,
            usuario.Nombre,
            usuario.CorreoElectronico.Valor,
            usuario.EsAdministrador,
            token
        );
    }
}