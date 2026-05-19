using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso) => true;
}