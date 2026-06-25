using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public record ExpedienteConTramitesResponse(
    Guid Id,
    string Caratula,
    EstadoExpediente Estado,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio,
    IEnumerable<TramiteResponse> Tramites
);

public class ObtenerExpedientePorIdUseCase(
    IExpedienteRepository repoExpediente,
    ITramiteRepository repoTramite)
{
    public ExpedienteConTramitesResponse Ejecutar(Guid id)
    {
        var expediente = repoExpediente.ObtenerPorId(id)
            ?? throw new EntidadNoEncontradaException("Expediente no encontrado.");

        var tramites = repoTramite.ObtenerPorExpedienteId(id)
            .Select(t => new TramiteResponse(
                t.Id,
                t.ExpedienteId,
                t.Etiqueta,
                t.Contenido.Valor,
                t.FechaCreacion,
                t.FechaUltimaModificacion,
                t.UsuarioUltimoCambio
            ));

        return new ExpedienteConTramitesResponse(
            expediente.Id,
            expediente.Caratula.Valor,
            expediente.Estado,
            expediente.FechaCreacion,
            expediente.FechaUltimaModificacion,
            expediente.UsuarioUltimoCambio,
            tramites
        );
    }
}