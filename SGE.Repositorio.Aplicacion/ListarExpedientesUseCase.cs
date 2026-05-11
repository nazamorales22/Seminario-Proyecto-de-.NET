using SGE.Repositorio.Dominio;

namespace SGE.Repositorio.Aplicacion;

public class ListarExpedientesUseCase(IExpedienteRepository repositorio)
{
    public List<ExpedienteDTO> Ejecutar()
    {
        var expedientes = repositorio.ObtenerTodos();
        
        // Convertimos la lista de Entidades a una lista de DTOs
        return expedientes.Select(e => new ExpedienteDTO(
            e.Id, 
            e.Caratula, 
            e.FechaCreacion, 
            e.Estado.ToString(), // Convertimos el Enum a string para la Consola
            e.IdUsuarioUltimaModificacion
        )).ToList();
    }
}
//Aquí aplicamos lo de devolver DTOs para que la Consola no toque las entidades.