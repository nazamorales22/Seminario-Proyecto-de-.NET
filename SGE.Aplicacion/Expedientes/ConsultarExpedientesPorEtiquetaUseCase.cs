using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Expedientes;

public class ConsultarExpedientesPorEtiquetaUseCase(
    IExpedienteRepository repoExp, 
    ITramiteRepository repoTram)
{
    public IEnumerable<Expediente> Ejecutar(EtiquetaTramite etiqueta)
    {
        //buscamos tramites con esa etiqueta
        var tramites = repoTram.ObtenerTodos();
        
        //obtenemos los id de los expedientes con esos tramites
        var idsExpedientes = tramites
            .Where(t => t.Etiqueta == etiqueta)
            .Select(t => t.ExpedienteId)
            .Distinct();

        //devolvemos los expedientes que tengan esos id
        return repoExp.ObtenerTodos().Where(e => idsExpedientes.Contains(e.Id));
    }
}