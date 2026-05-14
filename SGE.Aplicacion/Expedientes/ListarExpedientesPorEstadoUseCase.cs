using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ListarExpedientesPorEstadoUseCase(IExpedienteRepository repo)
{
    public IEnumerable<Expediente> Ejecutar(EstadoExpediente estado)
    {
        return repo.ObtenerTodos().Where(e => e.Estado == estado);
    }
}