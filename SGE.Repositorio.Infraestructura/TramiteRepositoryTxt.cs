using SGE.Repositorio.Dominio;
using SGE.Repositorio.Aplicacion;

namespace SGE.Repositorio.Infraestructura;

public class TramiteRepositoryTxt : ITramiteRepository
{
    private const string NombreArchivo = "../SGE_Datos/tramites.txt";

    public void Agregar(Tramite tramite)
    {
        // Aseguramos que la carpeta exista
        string? directorio = Path.GetDirectoryName(NombreArchivo);
        if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }

        // Buscamos el último ID para los trámites
        var todos = ObtenerTodosLosTramites();
        int proximoId = todos.Count > 0 ? todos.Max(t => t.Id) + 1 : 1;
        
        // Usamos reflexión o un campo de ayuda para asignar el ID si es necesario, 
        // pero para este nivel, podemos asumir que el objeto ya viene con su lógica.
        // Formato: Id|ExpedienteId|Contenido|Fecha|Usuario
        using StreamWriter sw = File.AppendText(NombreArchivo);
        sw.WriteLine($"{proximoId}|{tramite.ExpedienteId}|{tramite.Contenido}|{tramite.FechaHora}|{tramite.IdUsuario}");
    }

    public IEnumerable<Tramite> ObtenerPorExpediente(int expedienteId)
    {
        return ObtenerTodosLosTramites().Where(t => t.ExpedienteId == expedienteId);
    }

    private List<Tramite> ObtenerTodosLosTramites()
    {
        var lista = new List<Tramite>();
        if (!File.Exists(NombreArchivo)) return lista;

        foreach (var linea in File.ReadAllLines(NombreArchivo))
        {
            var datos = linea.Split('|');
            if (datos.Length >= 5)
            {
                var t = new Tramite(int.Parse(datos[1]), datos[2], int.Parse(datos[4]))
                {
                    // Asumiendo que agregamos una propiedad Id en la entidad para identificarlo
                };
                lista.Add(t);
            }
        }
        return lista;
    }

    public void Eliminar(int id) { /* Opcional para esta fase */ }
}