using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Usuarios;

public record ModificarMisDatosRequest(Guid IdUsuarioActivo, Guid IdUsuarioAModificar, string NuevoNombre, string NuevoCorreo, string? NuevaContrasena);

public class ModificarMisDatosUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow, IHasher hasher)
{
    public void Ejecutar(ModificarMisDatosRequest request)
    {
        if (request.IdUsuarioActivo != request.IdUsuarioAModificar)
            throw new AutorizacionException("No podés modificar los datos de otro usuario.");

        var usuario = repo.ObtenerPorId(request.IdUsuarioAModificar)
            ?? throw new EntidadNoEncontradaException("No se encontró el usuario.");

        var correo = new CorreoElectronico(request.NuevoCorreo);
        usuario.ActualizarDatos(request.NuevoNombre, correo);

        if (!string.IsNullOrWhiteSpace(request.NuevaContrasena))
            usuario.CambiarContrasena(hasher.Hashear(request.NuevaContrasena));

        repo.Modificar(usuario);
        uow.Guardar();
    }
}