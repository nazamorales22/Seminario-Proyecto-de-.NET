namespace SGE.Repositorio.Dominio;

public class Expediente
{
    public int Id { get; set; }
    public string Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public EstadoExpediente Estado { get; private set; }
    public int IdUsuarioUltimaModificacion { get; private set; }

    public Expediente(string caratula, int idUsuario)
    {
        if (string.IsNullOrWhiteSpace(caratula))
            throw new SGEException("La carátula no puede estar vacía.");

        Caratula = caratula;
        IdUsuarioUltimaModificacion = idUsuario;
        FechaCreacion = DateTime.Now;
        Estado = EstadoExpediente.RecienCreado;
    }

    public void ActualizarEstado(EstadoExpediente nuevoEstado, int idUsuario)
    {
        if (Estado == EstadoExpediente.Finalizado)
            throw new SGEException("No se puede modificar un expediente finalizado.");

        Estado = nuevoEstado;
        IdUsuarioUltimaModificacion = idUsuario;
    }
}

//Este es el "Dominio Rico" porque tiene la lógica de validación adentro.