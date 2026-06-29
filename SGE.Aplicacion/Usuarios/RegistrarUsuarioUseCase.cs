using SGE.Dominio.Comun;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public record RegistrarUsuarioRequest(string Nombre, string Correo, string Contrasena);
public record RegistrarUsuarioResponse(Guid Id, string Nombre, string Correo);

public class RegistrarUsuarioUseCase(IUsuarioRepository repo, IUnidadDeTrabajo uow, IHasher hasher)
{
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        var camposFaltantes = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Nombre))
            camposFaltantes.Add("nombre");

        if (string.IsNullOrWhiteSpace(request.Correo))
            camposFaltantes.Add("correo");

        if (string.IsNullOrWhiteSpace(request.Contrasena))
            camposFaltantes.Add("contraseña");

        if (camposFaltantes.Count > 0)
            throw new DominioException($"Los campos son obligatorios: {string.Join(", ", camposFaltantes)}.");

        if (request.Contrasena.Length < 6)
            throw new DominioException("La contraseña debe tener al menos 6 caracteres.");

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