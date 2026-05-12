using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura;

public class TramiteRepositoryTxt : ITramiteRepository
{
    private const string Archivo = "tramites.txt";

    public void Agregar(Tramite tramite)
    {
        using var sw = new StreamWriter(Archivo, true);
        // Usamos UsuarioUltimoCambio y nos aseguramos de que el contenido se guarde como string
        sw.WriteLine($"{tramite.Id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido}|{tramite.UsuarioUltimoCambio}|{tramite.FechaCreacion}");
    }

    public void Eliminar(Guid id)
    {
        if (!File.Exists(Archivo)) return;
        var lineas = File.ReadAllLines(Archivo).Where(l => Guid.Parse(l.Split('|')[0]) != id).ToList();
        File.WriteAllLines(Archivo, lineas);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        if (!File.Exists(Archivo)) return Enumerable.Empty<Tramite>();
        
        return File.ReadAllLines(Archivo)
            .Select(linea => {
                var d = linea.Split('|');
                // IMPORTANTE: El orden de los datos debe coincidir con el constructor de Tramite
                return new Tramite(
                    Guid.Parse(d[1]), // expedienteId
                    (EtiquetaTramite)Enum.Parse(typeof(EtiquetaTramite), d[2]), // etiqueta
                    new SGE.Dominio.Comun.ContenidoTramite(d[3]), // contenido
                    Guid.Parse(d[4]) // usuarioId (que se asignará a UsuarioUltimoCambio en el constructor)
                );
            })
            .Where(t => t.ExpedienteId == expedienteId);
    }
}