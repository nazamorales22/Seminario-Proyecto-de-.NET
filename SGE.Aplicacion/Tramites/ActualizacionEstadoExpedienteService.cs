// la cmbiamos using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Tramites;

public class ActualizacionEstadoExpedienteService
{
    private readonly IExpedienteRepository _repoExpediente;
    private readonly ITramiteRepository _repoTramite;

    public ActualizacionEstadoExpedienteService(IExpedienteRepository repoExpediente, ITramiteRepository repoTramite)
    {
        _repoExpediente = repoExpediente;
        _repoTramite = repoTramite;
    }

    public void Ejecutar(Guid expedienteId, Guid idUsuario)
    {
        var expediente = _repoExpediente.ObtenerPorId(expedienteId);
        if (expediente == null) return;

        var tramites = _repoTramite.ObtenerPorExpedienteId(expedienteId);
        var ultimaEtiqueta = tramites
            .OrderByDescending(t => t.FechaCreacion)
            .FirstOrDefault()?.Etiqueta;

        bool cambio = expediente.ActualizarEstado(ultimaEtiqueta, idUsuario);
        if (cambio)
            _repoExpediente.Modificar(expediente);
    }
}