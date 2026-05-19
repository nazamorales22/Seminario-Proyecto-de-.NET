using SGE.Aplicacion.Autorizacion;

namespace SGE.Infraestructura;

public class AutorizacionProvisionalService : IAutorizacionService
{
    //siempre devuelve true, todos tienen permiso para hacer las operaciones
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso) => true;
}