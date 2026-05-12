using SGE.Dominio.Comun;

namespace SGE.Dominio.Comun;

public record class Caratula(string Valor)
{
    public string Valor { get; } = !string.IsNullOrWhiteSpace(Valor) ? Valor : throw new DominioException("La carátula no puede estar vacía.");
}

public record class ContenidoTramite(string Valor)
{
    public string Valor { get; } = !string.IsNullOrWhiteSpace(Valor) ? Valor : throw new DominioException("El contenido del trámite no puede estar vacío.");
}
