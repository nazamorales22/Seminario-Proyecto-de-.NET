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

    public void Ejecutar(Guid idTramite, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid idUsuario)
    {
        if (!_authService.PoseeElPermiso(idUsuario, Permiso.TramiteModificacion))
            throw new AutorizacionException("No tiene permiso para modificar trámites.");

        var tramite = _repoTramite.ObtenerPorId(idTramite) 
            ?? throw new DominioException("No se encontró el trámite.");

        tramite.Modificar(etiqueta, contenido, idUsuario);
        _repoTramite.Modificar(tramite);

        _actualizacionEstado.Ejecutar(tramite.ExpedienteId, idUsuario);
    }
}