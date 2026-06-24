using SGE.Dominio.Comun;
using System.Security.Cryptography;
using System.Text;

namespace SGE.Aplicacion.Usuarios;

public record LoginRequest(string Correo, string Contrasena);
public record LoginResponse(Guid UserId, string Nombre, string Correo, bool EsAdministrador);

public class LoginUseCase(IUsuarioRepository repo)
{
    public LoginResponse Ejecutar(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Correo) || string.IsNullOrWhiteSpace(request.Contrasena))
            throw new DominioException("El correo y la contraseña son obligatorios.");

        var usuario = repo.ObtenerPorCorreo(request.Correo)
            ?? throw new DominioException("Credenciales inválidas.");

        var hashIngresado = Hashear(request.Contrasena);
        if (usuario.ContrasenaHash != hashIngresado)
            throw new DominioException("Credenciales inválidas.");

        return new LoginResponse(
            usuario.Id,
            usuario.Nombre,
            usuario.CorreoElectronico.Valor,
            usuario.EsAdministrador
        );
    }

    private static string Hashear(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}