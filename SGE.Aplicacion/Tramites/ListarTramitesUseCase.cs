namespace SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

public class ListarTramitesUseCase(ITramiteRepository repositorio)
{
    public List<TramiteResponse> Ejecutar(Guid expedienteId)
    {
        return repositorio.ObtenerPorExpedienteId(expedienteId)
            .Select(t => new TramiteResponse(
                t.Id,
                t.ExpedienteId,
                t.Etiqueta,
                t.Contenido.Valor,
                t.FechaCreacion,
                t.FechaUltimaModificacion,
                t.UsuarioUltimoCambio
            ))
            .ToList();
    }
}