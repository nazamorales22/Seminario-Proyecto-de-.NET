using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; } = DateTime.Now;
    public Guid UsuarioUltimoCambio { get; private set; }

    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId)
    {
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
    }
}
