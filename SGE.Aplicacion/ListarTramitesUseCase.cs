namespace SGE.Repositorio.Aplicacion;

public class ListarTramitesUseCase(ITramiteRepository repositorio)
{
    public List<TramiteDTO> Ejecutar(int expedienteId)
    {
        return repositorio.ObtenerPorExpediente(expedienteId)
            .Select(t => new TramiteDTO(t.Id, t.ExpedienteId, t.Contenido, t.FechaHora, t.IdUsuario))
            .ToList();
    }
}