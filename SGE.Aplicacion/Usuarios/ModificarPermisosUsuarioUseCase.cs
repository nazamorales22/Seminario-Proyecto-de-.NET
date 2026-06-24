using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;

namespace SGE.Aplicacion.Usuarios;

public record ModificarPermisosRequest(Guid IdUsuarioAModificar, Guid IdUsuarioActivo, IEnumerable<Permiso> NuevosPermisos);

public class ModificarPermisosUsuarioUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow)
{
    public void Ejecutar(ModificarPermisosRequest request)
    {
        var usuarioActivo = repo.ObtenerPorId(request.IdUsuarioActivo)
            ?? throw new AutorizacionException("Usuario no encontrado.");

        if (!usuarioActivo.EsAdministrador)
            throw new AutorizacionException("Solo los administradores pueden modificar permisos.");

        var usuario = repo.ObtenerPorId(request.IdUsuarioAModificar)
            ?? throw new DominioException("No se encontró el usuario.");

        foreach (var permiso in Enum.GetValues<Permiso>())
            usuario.RemoverPermiso(permiso);

        foreach (var permiso in request.NuevosPermisos)
            usuario.AsignarPermiso(permiso);

        repo.Modificar(usuario);
        uow.Guardar();
    }
}