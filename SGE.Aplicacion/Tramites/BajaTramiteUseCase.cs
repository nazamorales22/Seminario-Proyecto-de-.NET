using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites;

public class BajaTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IAutorizacionService _authService;
    private readonly ActualizacionEstadoExpedienteService _actualizacionEstado;

    public BajaTramiteUseCase(ITramiteRepository repoTramite, IAutorizacionService authService, ActualizacionEstadoExpedienteService actualizacionEstado)
    {
        _repoTramite = repoTramite;
        _authService = authService;
        _actualizacionEstado = actualizacionEstado;
    }

    public void Ejecutar(BajaTramiteRequest request)
    {
        if (!_authService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja))
            throw new AutorizacionException("No tiene permiso para dar de baja un trámite.");

        var tramite = _repoTramite.ObtenerPorId(request.Id)
            ?? throw new DominioException("No se encontró el trámite.");

        Guid expedienteId = tramite.ExpedienteId;

        _repoTramite.Eliminar(request.Id);

        _actualizacionEstado.Ejecutar(expedienteId, request.IdUsuario);
    }
}