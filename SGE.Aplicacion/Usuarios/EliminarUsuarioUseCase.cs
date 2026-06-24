using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Usuarios;

public record EliminarUsuarioRequest(Guid IdUsuarioAEliminar, Guid IdUsuarioActivo);

public class EliminarUsuarioUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow)
{
    public void Ejecutar(EliminarUsuarioRequest request)
    {
        var usuarioActivo = repo.ObtenerPorId(request.IdUsuarioActivo)
            ?? throw new AutorizacionException("Usuario no encontrado.");

        if (!usuarioActivo.EsAdministrador)
            throw new AutorizacionException("Solo los administradores pueden eliminar usuarios.");

        var usuarioAEliminar = repo.ObtenerPorId(request.IdUsuarioAEliminar)
            ?? throw new EntidadNoEncontradaException("No se encontró el usuario a eliminar.");

        repo.Eliminar(request.IdUsuarioAEliminar);
        uow.Guardar();
    }
}