using SGE.Dominio.Comun;

namespace SGE.Dominio.Comun;

public record class Caratula
{
    public string Valor { get; init; }

    public Caratula(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DominioException("La carátula no puede estar vacía.");
        }
        Valor = valor;
    }

    public override string ToString() => Valor;
}

public record class ContenidoTramite
{
    public string Valor { get; init; }

    public ContenidoTramite(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DominioException("El contenido del trámite no puede estar vacío.");
        }
        Valor = valor;
    }

    public override string ToString() => Valor;
}