using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}