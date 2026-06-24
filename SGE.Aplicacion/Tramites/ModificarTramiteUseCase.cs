using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;
using SGE.Aplicacion;

namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IAutorizacionService _authService;
    private readonly ActualizacionEstadoExpedienteService _actualizacionEstado;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarTramiteUseCase(
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

    public TramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        if (!_authService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permiso para modificar trámites.");

        var tramite = _repoTramite.ObtenerPorId(request.Id)
            ?? throw new EntidadNoEncontradaException("No se encontró el trámite.");

        var contenido = new ContenidoTramite(request.Contenido);
        tramite.Modificar(request.Etiqueta, contenido, request.IdUsuario);
        _repoTramite.Modificar(tramite);
        _actualizacionEstado.Ejecutar(tramite.ExpedienteId, request.IdUsuario);
        _unidadDeTrabajo.Guardar();

        return new TramiteResponse(
            tramite.Id,
            tramite.ExpedienteId,
            tramite.Etiqueta,
            tramite.Contenido.Valor,
            tramite.FechaCreacion,
            tramite.FechaUltimaModificacion,
            tramite.UsuarioUltimoCambio
        );
    }
}