using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    // Quitamos los valores por defecto de acá para que Reconstruir pueda pisarlos
    public Guid Id { get; private set; } 
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; } 
    public DateTime FechaUltimaModificacion { get; private set; } 
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoExpediente Estado { get; private set; }

    // Constructor para NUEVOS (acá sí generamos ID y Fecha actual)
    public Expediente(Caratula caratula, Guid usuarioId)
    {
        Id = Guid.NewGuid();
        Caratula = caratula;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = DateTime.Now;
        Estado = EstadoExpediente.RecienIniciado;
    }

    // El método que soluciona tu problema de la Baja
    public static Expediente Reconstruir(Guid id, Caratula caratula, Guid usuarioId, EstadoExpediente estado, DateTime fecha)
    {
        var exp = new Expediente(caratula, usuarioId);
        exp.Id = id; // Ahora sí pisa el ID generado con el del archivo
        exp.Estado = estado;
        exp.FechaCreacion = fecha;
        return exp;
    }

    public bool ActualizarEstado(EtiquetaTramite? ultimaEtiqueta, Guid idUsuario)
    {
        var estadoAnterior = Estado;
        Estado = ultimaEtiqueta switch
        {
            EtiquetaTramite.Resolucion => EstadoExpediente.ConResolucion,
            EtiquetaTramite.PaseAEstudio => EstadoExpediente.ParaResolver,
            EtiquetaTramite.PaseAlArchivo => EstadoExpediente.Finalizado,
            null => EstadoExpediente.RecienIniciado,
            _ => Estado
        };

        if (estadoAnterior != Estado)
        {
            UsuarioUltimoCambio = idUsuario;
            FechaUltimaModificacion = DateTime.Now;
            return true; 
        }
        return false;
    }
    public void ActualizarCaratula(Caratula nuevaCaratula, Guid usuarioId)
{
    this.Caratula = nuevaCaratula;
    this.UsuarioUltimoCambio = usuarioId;
    this.FechaUltimaModificacion = DateTime.Now;
}
}