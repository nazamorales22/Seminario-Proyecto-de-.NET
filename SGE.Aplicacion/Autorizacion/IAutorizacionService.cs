using SGE.Dominio.Comun;

namespace SGE.Aplicacion.Autorizacion;

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
}
