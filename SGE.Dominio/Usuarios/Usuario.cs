using SGE.Dominio.Comun;

namespace SGE.Dominio.Usuarios;

public class Usuario
{
    private readonly HashSet<Permiso> _permisos = new();

    public Guid Id { get; private init; }
    public string Nombre { get; private set; }
    public CorreoElectronico CorreoElectronico { get; private set; }
    public string ContrasenaHash { get; private set; }
    public bool EsAdministrador { get; private init; }
    public IReadOnlyCollection<Permiso> Permisos => _permisos;

    // Constructor
    private Usuario()
    {
        Nombre = string.Empty;
        CorreoElectronico = null!;
        ContrasenaHash = string.Empty;
    }

    public Usuario(string nombre, CorreoElectronico correoElectronico, string contrasenaHash, bool esAdministrador = false)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new DominioException("El nombre del usuario no puede estar vacío.");
        }

        if (string.IsNullOrWhiteSpace(contrasenaHash))
        {
            throw new DominioException("El usuario debe tener una contraseña.");
        }

        Id = Guid.NewGuid();
        Nombre = nombre;
        CorreoElectronico = correoElectronico ?? throw new DominioException("El correo electrónico es obligatorio.");
        ContrasenaHash = contrasenaHash;
        EsAdministrador = esAdministrador;
    }

    public void AsignarPermiso(Permiso permiso)
    {
        if (!Enum.IsDefined(typeof(Permiso), permiso))
            throw new DominioException("El permiso especificado no es válido.");

        _permisos.Add(permiso);
    }

    public void RemoverPermiso(Permiso permiso)
    {
        _permisos.Remove(permiso);
    }

    public bool TienePermiso(Permiso permiso)
    {
        return _permisos.Contains(permiso);
    }

    public void ActualizarDatos(string nombre, CorreoElectronico correoElectronico)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new DominioException("El nombre del usuario no puede estar vacío.");
        }

        Nombre = nombre;
        CorreoElectronico = correoElectronico ?? throw new DominioException("El correo electrónico es obligatorio.");
    }

    public void CambiarContrasena(string nuevoHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoHash))
        {
            throw new DominioException("La nueva contraseña no puede estar vacía.");
        }

        ContrasenaHash = nuevoHash;
    }


    // Solo para EF Core
    public string PermisosSerializados
    {
        get => string.Join(',', _permisos.Select(p => p.ToString()));
        private set
        {
            _permisos.Clear();
            if (!string.IsNullOrEmpty(value))
                foreach (var p in value.Split(','))
                    _permisos.Add(Enum.Parse<Permiso>(p));
        }
    }
}