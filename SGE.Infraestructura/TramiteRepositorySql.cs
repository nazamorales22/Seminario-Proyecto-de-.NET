using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites; // Buscá la corrección rápida si el namespace de la interfaz de trámites es un toque distinto
using System.Collections.Generic;
using System.Linq;

namespace SGE.Infraestructura;

public class TramiteRepositorySql : ITramiteRepository
{
    private readonly SGEDbContext _context;

    public TramiteRepositorySql(SGEDbContext context)
    {
        _context = context;
    }

    public void Agregar(Tramite tramite)
    {
        _context.Tramites.Add(tramite);
        _context.SaveChanges();
    }

    public void Eliminar(Guid id)
    {
        throw new NotImplementedException();
    }

    public void EliminarRelacionadosA(Guid expedienteId)
    {
        throw new NotImplementedException();
    }

    public void Modificar(Tramite tramite)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        throw new NotImplementedException();
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        throw new NotImplementedException();
    }

    // Nota: Si la interfaz de las chicas te pide IEnumerable, usá el truco de la lamparita 
    // para que te acomode el tipo de retorno automáticamente como recién.
    public IEnumerable<Tramite> ObtenerTodos()
    {
        return _context.Tramites.ToList();
    }
}