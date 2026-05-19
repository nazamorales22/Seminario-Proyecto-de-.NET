using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public class TramiteDTO
{
    public Guid Id { get; set; }
    public Guid ExpedienteId { get; set; }
    public string Contenido { get; set; } = "";
    public DateTime FechaHora { get; set; }
    public Guid IdUsuario { get; set; }
    public EtiquetaTramite Etiqueta { get; set; }
}