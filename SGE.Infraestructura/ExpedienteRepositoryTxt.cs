using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;

namespace SGE.Infraestructura;

public class ExpedienteRepositoryTxt : IExpedienteRepository
{
    private const string Archivo = "expedientes.txt";

    public void Agregar(Expediente expediente)
    {
        using var sw = new StreamWriter(Archivo, true);
        // Usamos UsuarioUltimoCambio que es el nombre real en tu Dominio
        sw.WriteLine($"{expediente.Id}|{expediente.Caratula}|{expediente.UsuarioUltimoCambio}|{expediente.Estado}|{expediente.FechaCreacion}");
    }

    public void Modificar(Expediente expediente)
    {
        if (!File.Exists(Archivo)) return;
        var lineas = File.ReadAllLines(Archivo).ToList();
        var nuevaLista = lineas.Select(linea => {
            var datos = linea.Split('|');
            if (Guid.Parse(datos[0]) == expediente.Id)
            {
                return $"{expediente.Id}|{expediente.Caratula}|{expediente.UsuarioUltimoCambio}|{expediente.Estado}|{expediente.FechaCreacion}";
            }
            return linea;
        });
        File.WriteAllLines(Archivo, nuevaLista);
    }

public void Eliminar(Guid id)
{
    if (!File.Exists("expedientes.txt")) return;

    // 1. Leemos todas las líneas
    var lineas = File.ReadAllLines("expedientes.txt");

    // 2. Filtramos: nos quedamos con las que NO coincidan con el ID
    var lineasRestantes = lineas
        .Where(linea => {
            var datos = linea.Split('|');
            // IMPORTANTE: Asegurate de que el ID sea el primer dato (pos 0)
            return Guid.Parse(datos[0]) != id;
        })
        .ToList();

    // 3. SOBRESCRIBIMOS el archivo con las líneas que quedaron
    File.WriteAllLines("expedientes.txt", lineasRestantes);
}

    public Expediente? ObtenerPorId(Guid id) => ObtenerTodos().FirstOrDefault(e => e.Id == id);

public IEnumerable<Expediente> ObtenerTodos()
{
    if (!File.Exists(Archivo)) return Enumerable.Empty<Expediente>();
    
    return File.ReadAllLines(Archivo).Select(linea => {
        var datos = linea.Split('|');
        // Usamos los datos del archivo, especialmente el ID (datos[0])
        return Expediente.Reconstruir(
            Guid.Parse(datos[0]), 
            new Caratula(datos[1]), 
            Guid.Parse(datos[2]),
            Enum.Parse<EstadoExpediente>(datos[3]),
            DateTime.Parse(datos[4])
        );
    }).ToList();
}
}