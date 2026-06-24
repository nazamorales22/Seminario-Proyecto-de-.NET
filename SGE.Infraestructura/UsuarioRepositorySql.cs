using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura;

public class UsuarioRepositorySql : IUsuarioRepository
{
    private readonly SGEDbContext _context;

    public UsuarioRepositorySql(SGEDbContext context)
    {
        _context = context;
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
    }

    public void Modificar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
    }

    public void Eliminar(Guid id)
    {
        var usuario = ObtenerPorId(id);
        if (usuario != null)
            _context.Usuarios.Remove(usuario);
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios.FirstOrDefault(u => u.Id == id);
    }

    public Usuario? ObtenerPorCorreo(string correo)
    {
        return _context.Usuarios.FirstOrDefault(u => u.CorreoElectronico.Valor == correo.ToLowerInvariant());
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _context.Usuarios.ToList();
    }
}