using SGE.Repositorio.Dominio;
using SGE.Repositorio.Aplicacion;

namespace SGE.Repositorio.Infraestructura;

public class ExpedienteRepositoryTxt : IExpedienteRepository
{
    // Esta ruta apunta a la carpeta que creaste afuera
    private const string NombreArchivo = "../SGE_Datos/expedientes.txt";

    public void Agregar(Expediente expediente)
    {  
        string? directorio = Path.GetDirectoryName(NombreArchivo);
        if (!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
        {
            Directory.CreateDirectory(directorio);
        }
        
        var todos = ObtenerTodos().ToList();
        int proximoId = todos.Count > 0 ? todos.Max(e => e.Id) + 1 : 1;
        expediente.Id = proximoId;

        using StreamWriter sw = File.AppendText(NombreArchivo);
        sw.WriteLine($"{expediente.Id}|{expediente.Caratula}|{expediente.FechaCreacion}|{expediente.Estado}|{expediente.IdUsuarioUltimaModificacion}");
    }

    public IEnumerable<Expediente> ObtenerTodos()
    {
        var lista = new List<Expediente>();
        if (!File.Exists(NombreArchivo)) return lista;

        foreach (var linea in File.ReadAllLines(NombreArchivo))
        {
            var datos = linea.Split('|');
            var e = new Expediente(datos[1], int.Parse(datos[4])) { Id = int.Parse(datos[0]) };
            lista.Add(e);
        }
        return lista;
    }

    public void Eliminar(int id) { /* Luego completamos los demás */ }
    public void Modificar(Expediente expediente) { /* Luego completamos los demás */ }
    public Expediente? ObtenerPorId(int id) => ObtenerTodos().FirstOrDefault(e => e.Id == id);
}