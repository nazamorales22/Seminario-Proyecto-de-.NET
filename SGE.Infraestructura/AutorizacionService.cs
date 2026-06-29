using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Comun;

namespace SGE.Infraestructura;

public class AutorizacionService(IUsuarioRepository usuarioRepository) : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        var usuario = usuarioRepository.ObtenerPorId(idUsuario);

        if (usuario == null)
            return false;

        if (usuario.EsAdministrador)
            return true;

        if (usuario.TienePermiso(permiso))
            return true;

        // Regla de implicancia: ExpedienteBaja implica TramiteBaja
        if (permiso == Permiso.TramiteBaja && usuario.TienePermiso(Permiso.ExpedienteBaja))
            return true;

        return false;
    }
}