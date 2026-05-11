namespace SGE.Repositorio.Dominio;

public class Tramite
{
    public int Id { get; set; }
    public int ExpedienteId { get; private set; }
    public string Contenido { get; private set; }
    public DateTime FechaHora { get; private set; }
    public int IdUsuario { get; private set; }

    public Tramite(int expedienteId, string contenido, int idUsuario)
    {
        if (string.IsNullOrWhiteSpace(contenido))
            throw new SGEException("El contenido del trámite no puede estar vacío.");

        ExpedienteId = expedienteId;
        Contenido = contenido;
        IdUsuario = idUsuario;
        FechaHora = DateTime.Now;
    }
}