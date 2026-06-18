namespace SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;

public class ListarExpedientesUseCase(IExpedienteRepository repositorio)
{
    public List<ExpedienteResponse> Ejecutar()
    {
        return repositorio.ObtenerTodos()
            .Select(e => new ExpedienteResponse(
                e.Id,
                e.Caratula.Valor,
                e.Estado,
                e.FechaCreacion,
                e.FechaUltimaModificacion,
                e.UsuarioUltimoCambio
            ))
            .ToList();
    }
}