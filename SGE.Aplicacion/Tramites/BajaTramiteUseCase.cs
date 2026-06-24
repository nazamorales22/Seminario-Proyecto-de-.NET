using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;
using SGE.Aplicacion;

namespace SGE.Aplicacion.Tramites;

public class BajaTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IAutorizacionService _authService;
    private readonly ActualizacionEstadoExpedienteService _actualizacionEstado;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public BajaTramiteUseCase(
        ITramiteRepository repoTramite,
        IAutorizacionService authService,
        ActualizacionEstadoExpedienteService actualizacionEstado,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repoTramite = repoTramite;
        _authService = authService;
        _actualizacionEstado = actualizacionEstado;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public void Ejecutar(BajaTramiteRequest request)
    {
        if (!_authService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteBaja))
            throw new AutorizacionException("No tiene permiso para dar de baja un trámite.");

        var tramite = _repoTramite.ObtenerPorId(request.Id)
            ?? throw new EntidadNoEncontradaException("No se encontró el trámite.");

        Guid expedienteId = tramite.ExpedienteId;

        _repoTramite.Eliminar(request.Id);
        _actualizacionEstado.Ejecutar(expedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();
    }
}