using SGE.Aplicacion;

namespace SGE.Infraestructura;

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly SGEDbContext _context;

    public UnidadDeTrabajo(SGEDbContext context)
    {
        _context = context;
    }

    public void Guardar()
    {
        _context.SaveChanges();
    }
}