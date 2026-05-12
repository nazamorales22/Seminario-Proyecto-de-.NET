using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; } = DateTime.Now;
    public DateTime FechaUltimaModificacion { get; private set; } = DateTime.Now;
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoExpediente Estado { get; private set; } = EstadoExpediente.RecienIniciado;

    public Expediente(Caratula caratula, Guid usuarioId)
    {
        Caratula = caratula;
        UsuarioUltimoCambio = usuarioId;
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
}