using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;
using SGE.Dominio.Comun;

namespace SGE.Aplicacion.Usuarios;

public record UsuarioResponse(Guid Id, string Nombre, string Correo, bool EsAdministrador, IEnumerable<Permiso> Permisos);

public class ListarUsuariosUseCase(IUsuarioRepository repo)
{
    public IEnumerable<UsuarioResponse> Ejecutar(Guid idUsuarioActivo)
    {
        var usuarioActivo = repo.ObtenerPorId(idUsuarioActivo)
            ?? throw new AutorizacionException("Usuario no encontrado.");

        if (!usuarioActivo.EsAdministrador)
            throw new AutorizacionException("Solo los administradores pueden listar usuarios.");

        return repo.ObtenerTodos().Select(u => new UsuarioResponse(
            u.Id,
            u.Nombre,
            u.CorreoElectronico.Valor,
            u.EsAdministrador,
            u.Permisos
        ));
    }
}