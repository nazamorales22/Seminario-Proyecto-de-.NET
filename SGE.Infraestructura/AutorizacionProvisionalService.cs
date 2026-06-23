using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;

namespace SGE.Infraestructura;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso) => true;
}
