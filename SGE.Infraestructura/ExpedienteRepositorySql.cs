using SGE.Dominio.Expedientes; // Ajustá según tus namespaces reales
using System.Collections.Generic;
using System.Linq;
using SGE.Aplicacion.Expedientes;
using Microsoft.EntityFrameworkCore;
namespace SGE.Infraestructura;

public class ExpedienteRepositorySql : IExpedienteRepository
{
    // Esta es nuestra conexión directa a la base de datos
    private readonly SGEDbContext _context;

    // Constructor: Le pedimos a .NET que nos pase el contexto
    public ExpedienteRepositorySql(SGEDbContext context)
    {
        _context = context;
    }

    // 1. GUARDAR UN EXPEDIENTE NUEVO
    public void Agregar(Expediente expediente)
    {
        _context.Expedientes.Add(expediente); // EF sabe que va a la tabla de Expedientes
        
    }

    // 2. LEER TODOS LOS EXPEDIENTES
    public IEnumerable<Expediente> ObtenerTodos()
    {
        return _context.Expedientes.ToList();
    }

    // 3. BUSCAR UN EXPEDIENTE POR ID
    public Expediente? ObtenerPorId(Guid id)
    {
        return _context.Expedientes.FirstOrDefault(e => e.Id == id);
    }

    // 4. MODIFICAR UN EXPEDIENTE EXISTENTE
    public void Modificar(Expediente expediente)
    {
        _context.Expedientes.Update(expediente); // EF busca la fila por ID y actualiza sus campos
        
    }

    // 5. ELIMINAR UN EXPEDIENTE
    public void Eliminar(Guid id)
    {
        var expediente = ObtenerPorId(id);
        if (expediente != null)
        {
            _context.Expedientes.Remove(expediente); // Remueve la fila
            
            // ¡Acá actúa el borrado en cascada automático que configuramos hoy! 
            // SQLite va a borrar solo todos los trámites que le pertenecían a este expediente.
        }
    }

}