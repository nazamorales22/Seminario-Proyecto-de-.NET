using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Expedientes;

public class ConsultarExpedientesPorEtiquetaUseCase(
    IExpedienteRepository repoExp, 
    ITramiteRepository repoTram)
{
    public IEnumerable<Expediente> Ejecutar(EtiquetaTramite etiqueta)
    {
        // 1. Buscamos todos los trámites que tengan esa etiqueta
        var tramites = repoTram.ObtenerTodos(); // Asegurate que tu repo tenga ObtenerTodos()
        
        // 2. Sacamos los IDs de los expedientes involucrados (sin repetir)
        var idsExpedientes = tramites
            .Where(t => t.Etiqueta == etiqueta)
            .Select(t => t.ExpedienteId)
            .Distinct();

        // 3. Devolvemos los expedientes reales
        return repoExp.ObtenerTodos().Where(e => idsExpedientes.Contains(e.Id));
    }
}