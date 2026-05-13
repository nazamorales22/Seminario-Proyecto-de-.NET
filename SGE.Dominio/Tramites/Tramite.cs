using System;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    // Constructor para trámites NUEVOS
    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = DateTime.Now; 
    }

    // Constructor para RECONSTRUIR (usado por el Repositorio)
    private Tramite(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId, DateTime fecha)
    {
        Id = id;
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = fecha;
    }

    public static Tramite Reconstruir(Guid id, Guid expId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId, DateTime fecha)
    {
        return new Tramite(id, expId, etiqueta, contenido, usuarioId, fecha);
    }
}