using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites;
namespace SGE.Aplicacion.Expedientes;

public class ConsultarExpedientesPorEtiquetaUseCase(
    IExpedienteRepository repoExp,
    ITramiteRepository repoTram)
{
    public IEnumerable<ExpedienteResponse> Ejecutar(ConsultarExpedientesPorEtiquetaRequest request)
    {
        var tramites = repoTram.ObtenerTodos();

        var idsExpedientes = tramites
            .Where(t => t.Etiqueta == request.Etiqueta)
            .Select(t => t.ExpedienteId)
            .Distinct();

        return repoExp.ObtenerTodos()
            .Where(e => idsExpedientes.Contains(e.Id))
            .Select(e => new ExpedienteResponse(
                e.Id,
                e.Caratula.Valor,
                e.Estado,
                e.FechaCreacion,
                e.FechaUltimaModificacion,
                e.UsuarioUltimoCambio
            ));
    }
}