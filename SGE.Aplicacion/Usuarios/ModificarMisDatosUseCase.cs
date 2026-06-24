using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;
using System.Security.Cryptography;
using System.Text;

namespace SGE.Aplicacion.Usuarios;

public record ModificarMisDatosRequest(Guid IdUsuarioActivo, Guid IdUsuarioAModificar, string NuevoNombre, string NuevoCorreo, string? NuevaContrasena);

public class ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow)
{
    public void Ejecutar(ModificarMisDatosRequest request)
    {
        if (request.IdUsuarioActivo != request.IdUsuarioAModificar)
            throw new AutorizacionException("No podés modificar los datos de otro usuario.");

        var usuario = repo.ObtenerPorId(request.IdUsuarioAModificar)
            ?? throw new DominioException("No se encontró el usuario.");

        var correo = new CorreoElectronico(request.NuevoCorreo);
        usuario.ActualizarDatos(request.NuevoNombre, correo);

        if (!string.IsNullOrWhiteSpace(request.NuevaContrasena))
            usuario.CambiarContrasena(Hashear(request.NuevaContrasena));

        repo.Modificar(usuario);
        uow.Guardar();
    }

    private static string Hashear(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}