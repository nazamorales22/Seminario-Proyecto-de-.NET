using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public interface IUsuarioRepository
{
    void Agregar(Usuario usuario);
    void Modificar(Usuario usuario);
    void Eliminar(Guid id);
    Usuario? ObtenerPorId(Guid id);
    Usuario? ObtenerPorCorreo(string correo);
    IEnumerable<Usuario> ObtenerTodos();
}