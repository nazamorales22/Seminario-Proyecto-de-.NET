
namespace SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

public class ListarTramitesUseCase(ITramiteRepository repositorio)
{
    public List<TramiteDTO> Ejecutar(Guid expedienteId)
    {
        return repositorio.ObtenerPorExpedienteId(expedienteId)
            .Select(t => new TramiteDTO {
                Id = t.Id, 
                ExpedienteId = t.ExpedienteId, 
                // Usamos ToString() para convertir el Value Object a string para el DTO
                //Contenido = t.Contenido.ToString() ?? "", 
                Contenido = t.Contenido.Valor,
                FechaHora = t.FechaCreacion, 
                // Mapeamos UsuarioUltimoCambio (Dominio) a IdUsuario (DTO)
                IdUsuario = t.UsuarioUltimoCambio,
                Etiqueta = t.Etiqueta 

            })
            .ToList();
    }
}