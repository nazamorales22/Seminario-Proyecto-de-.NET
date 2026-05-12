using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura;

public class TramiteRepositoryTxt : ITramiteRepository
{
    private const string Archivo = "tramites.txt";

    public void Agregar(Tramite tramite)
    {
        using var sw = new StreamWriter(Archivo, true);
        sw.WriteLine($"{tramite.Id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido.Texto}|{tramite.UsuarioId}|{tramite.FechaCreacion}");
    }

    public void Eliminar(Guid id)
    {
        var lineas = File.ReadAllLines(Archivo).Where(l => Guid.Parse(l.Split('|')[0]) != id);
        File.WriteAllLines(Archivo, lineas);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        if (!File.Exists(Archivo)) return Enumerable.Empty<Tramite>();
        return File.ReadAllLines(Archivo)
            .Select(linea => {
                var d = linea.Split('|');
                return new Tramite(Guid.Parse(d[1]), (EtiquetaTramite)Enum.Parse(typeof(EtiquetaTramite), d[2]), new SGE.Dominio.Comun.ContenidoTramite(d[3]), Guid.Parse(d[4])) { Id = Guid.Parse(d[0]) };
            })
            .Where(t => t.ExpedienteId == expedienteId);
    }
}