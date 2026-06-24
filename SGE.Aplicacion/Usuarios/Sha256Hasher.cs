using System.Security.Cryptography;
using System.Text;

namespace SGE.Aplicacion.Usuarios;

public class Sha256Hasher : IHasher
{
    public string Hashear(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}