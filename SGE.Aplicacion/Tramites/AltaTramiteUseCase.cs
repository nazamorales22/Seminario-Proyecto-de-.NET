using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion;

namespace SGE.Aplicacion.Tramites;

public class AltaTramiteUseCase(
    ITramiteRepository repoTramite,
    IExpedienteRepository repoExpediente,
    IAutorizacionService auth,
    ActualizacionEstadoExpedienteService actualizacionEstado,
    IUnidadDeTrabajo unidadDeTrabajo)
{
    public TramiteResponse Ejecutar(AltaTramiteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.TramiteAlta))
            throw new AutorizacionException("No tenés permiso para crear trámites.");

        var camposFaltantes = new List<string>();

        if (request.ExpedienteId == Guid.Empty)
            camposFaltantes.Add("expedienteId");

        if (string.IsNullOrWhiteSpace(request.Contenido))
            camposFaltantes.Add("contenido");

        if (camposFaltantes.Count > 0)
            throw new DominioException($"Los campos son obligatorios: {string.Join(", ", camposFaltantes)}.");

        var expediente = repoExpediente.ObtenerPorId(request.ExpedienteId)
            ?? throw new EntidadNoEncontradaException("El expediente no existe.");

        var contenido = new ContenidoTramite(request.Contenido);
        var tramite = new Tramite(request.ExpedienteId, request.Etiqueta, contenido, request.IdUsuario);

        repoTramite.Agregar(tramite);
        unidadDeTrabajo.Guardar();  // guarda el trámite

        actualizacionEstado.Ejecutar(request.ExpedienteId, request.IdUsuario);
        unidadDeTrabajo.Guardar();  // guarda el cambio de estado

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