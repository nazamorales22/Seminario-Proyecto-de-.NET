using SGE.Dominio.Comun;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public record RegistrarUsuarioRequest(string Nombre, string Correo, string Contrasena);
public record RegistrarUsuarioResponse(Guid Id, string Nombre, string Correo);

public class RegistrarUsuarioUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow, IHasher hasher)
{
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        // Verificar que el correo no esté ya registrado
        var existente = repo.ObtenerPorCorreo(request.Correo);
        if (existente != null)
            throw new DominioException("El correo electrónico ya está registrado.");

        var correo = new CorreoElectronico(request.Correo);
        var hash = hasher.Hashear(request.Contrasena);
        var usuario = new Usuario(request.Nombre, correo, hash);

        repo.Agregar(usuario);
        uow.Guardar();

        return new RegistrarUsuarioResponse(usuario.Id, usuario.Nombre, usuario.CorreoElectronico.Valor);
    }
}