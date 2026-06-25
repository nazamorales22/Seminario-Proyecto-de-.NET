using SGE.Dominio.Tramites;
using SGE.Aplicacion.Tramites;
using Microsoft.EntityFrameworkCore;
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
    }

    public void Eliminar(Guid id)
    {
        var tramite = ObtenerPorId(id);
        if (tramite != null)
            _context.Tramites.Remove(tramite);
    }

    public void EliminarRelacionadosA(Guid expedienteId)
    {
        var tramites = _context.Tramites
            .Where(t => t.ExpedienteId == expedienteId)
            .ToList();
        _context.Tramites.RemoveRange(tramites);
    }

    public void Modificar(Tramite tramite)
    {
        _context.Tramites.Update(tramite);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return _context.Tramites
            .AsNoTracking() 
            .Where(t => t.ExpedienteId == expedienteId)
            .ToList();
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return _context.Tramites
            .AsNoTracking() //
            .FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerTodos()
    {
        return _context.Tramites.ToList();
    }
}