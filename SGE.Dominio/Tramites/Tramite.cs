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
    public DateTime FechaUltimaModificacion { get; private set; }
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
        FechaUltimaModificacion = DateTime.Now;
    }

    // Constructor para RECONSTRUIR (usado por el Repositorio)
    private Tramite(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId, DateTime fechaCreacion, DateTime fechaUltimaModificacion)
    {
        Id = id;
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUltimaModificacion;
    }

   // Factory Method
    public static Tramite Reconstruir(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId, DateTime fechaCreacion, DateTime fechaUltimaModificacion)
    {
        return new Tramite(id, expedienteId, etiqueta, contenido, usuarioId, fechaCreacion, fechaUltimaModificacion);
    }

    // Método para modificar
    public void Modificar(EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioId)
    {
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioId;
        FechaUltimaModificacion = DateTime.Now;
    }
}