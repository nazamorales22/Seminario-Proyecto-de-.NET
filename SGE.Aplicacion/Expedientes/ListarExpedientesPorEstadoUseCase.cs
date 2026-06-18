using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Expedientes;

public class ListarExpedientesPorEstadoUseCase(IExpedienteRepository repo)
{
    public IEnumerable<ExpedienteResponse> Ejecutar(ListarExpedientesPorEstadoRequest request)
    {
        return repo.ObtenerTodos()
            .Where(e => e.Estado == request.Estado)
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