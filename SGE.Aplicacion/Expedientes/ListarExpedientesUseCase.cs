namespace SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;

public class ListarExpedientesUseCase(IExpedienteRepository repositorio)
{
    public List<ExpedienteDTO> Ejecutar()
    {
        return repositorio.ObtenerTodos()
            .Select(e => new ExpedienteDTO {
                Id = e.Id,
                Caratula = e.Caratula.ToString() ?? "",
                Estado = e.Estado.ToString()
            })
            .ToList();
    }
}