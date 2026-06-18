using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;
namespace SGE.Aplicacion.Tramites;

public class ModificarTramiteUseCase
{
    private readonly ITramiteRepository _repoTramite;
    private readonly IAutorizacionService _authService;
    private readonly ActualizacionEstadoExpedienteService _actualizacionEstado;

    public ModificarTramiteUseCase(ITramiteRepository repoTramite, IAutorizacionService authService, ActualizacionEstadoExpedienteService actualizacionEstado)
    {
        _repoTramite = repoTramite;
        _authService = authService;
        _actualizacionEstado = actualizacionEstado;
    }

    public TramiteResponse Ejecutar(ModificarTramiteRequest request)
    {
        if (!_authService.PoseeElPermiso(request.IdUsuario, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permiso para modificar trámites.");

        var tramite = _repoTramite.ObtenerPorId(request.Id)
            ?? throw new DominioException("No se encontró el trámite.");

        var contenido = new ContenidoTramite(request.Contenido);
        tramite.Modificar(request.Etiqueta, contenido, request.IdUsuario);
        _repoTramite.Modificar(tramite);

        _actualizacionEstado.Ejecutar(tramite.ExpedienteId, request.IdUsuario);

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