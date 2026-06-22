using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;
using System; // NUEVO: Para usar EventHandler si no estaba

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; } 
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; } 
    public DateTime FechaUltimaModificacion { get; private set; } 
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoExpediente Estado { get; private set; }

    // =========================================================================
    // 🔔 NUEVO: EVENTO DE DOMINIO (Para el Patrón Observer - Clase 12)
    // =========================================================================
    // Este evento se disparará cada vez que el estado cambie de verdad.
    public event EventHandler<EstadoExpedienteCambiadoEventArgs>? EstadoCambiado;

    // =========================================================================
    // 🗄️ NUEVO: CONSTRUCTOR PRIVADO VACÍO PARA ENTITY FRAMEWORK CORE
    // =========================================================================
    // EF Core lo necesita sí o sí para materializar la entidad desde SQLite.
#pragma warning disable CS8618 // Deshabilita el aviso de que Caratula arranca en null
    private Expediente() { } 
#pragma warning restore CS8618

    // Constructor para NUEVOS
    public Expediente(Caratula caratula, Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new DominioException("El usuario no puede ser vacío.");

        Id = Guid.NewGuid();
        Caratula = caratula;
        UsuarioUltimoCambio = usuarioId;
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = DateTime.Now;
        Estado = EstadoExpediente.RecienIniciado;
    }

    // Constructor privado para RECONSTRUIR
    private Expediente(Guid id, Caratula caratula, Guid usuarioId, EstadoExpediente estado, DateTime fechaCreacion, DateTime fechaUltimaModificacion)
    {
        Id = id;
        Caratula = caratula;
        UsuarioUltimoCambio = usuarioId;
        Estado = estado;
        FechaCreacion = fechaCreacion;
        FechaUltimaModificacion = fechaUltimaModificacion;
    }
    
    // Factory Method (Excelente aclaración sobre los permisos)
    public static Expediente Reconstruir(Guid id, Caratula caratula, Guid usuarioId, EstadoExpediente estado, DateTime fechaCreacion, DateTime fechaUltimaModificacion)
    {
        return new Expediente(id, caratula, usuarioId, estado, fechaCreacion, fechaUltimaModificacion);
    }

    public bool ActualizarEstado(EtiquetaTramite? ultimaEtiqueta, Guid idUsuario)
    {
        if(Estado == EstadoExpediente.Finalizado)
        {
            throw new DominioException("No se pueden agregar tramites ni modificar un expediente que se encuentra finalizado.  ");
        }
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

            // 🔔 NUEVO: Disparamos el evento del Observer
            NotificarCambioEstado(estadoAnterior, Estado);

            return true; 
        }
        return false;
    }

    public void ActualizarCaratula(Caratula nuevaCaratula, Guid usuarioId)
    {
        Caratula = nuevaCaratula;
        UsuarioUltimoCambio = usuarioId;
        FechaUltimaModificacion = DateTime.Now;
    }

    public void CambiarEstado(EstadoExpediente nuevoEstado, Guid idUsuario)
    {
        // 🛡️ NUEVO: CONTROL DEL CICLO DE VIDA (Requerimiento funcional nuevo)
        if (Estado == EstadoExpediente.Finalizado)
            throw new DominioException("No se puede cambiar el estado de un expediente que ya está Finalizado.");

        var estadoAnterior = Estado;
        Estado = nuevoEstado;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;

        // 🔔 NUEVO: Disparamos el evento del Observer
        NotificarCambioEstado(estadoAnterior, Estado);
    }

    // Método auxiliar para lanzar el evento de forma segura
    private void NotificarCambioEstado(EstadoExpediente anterior, EstadoExpediente nuevo)
    {
        EstadoCambiado?.Invoke(this, new EstadoExpedienteCambiadoEventArgs(Id, anterior, nuevo));
    }
}

// =========================================================================
// 🔔 NUEVO: OBJETO DE DATOS PARA EL EVENTO (Observer)
// =========================================================================
// Esta clase auxiliar viaja con el evento llevando la info de qué pasó.
public class EstadoExpedienteCambiadoEventArgs : EventArgs
{
    public Guid ExpedienteId { get; }
    public EstadoExpediente EstadoAnterior { get; }
    public EstadoExpediente EstadoNuevo { get; }

    public EstadoExpedienteCambiadoEventArgs(Guid expedienteId, EstadoExpediente anterior, EstadoExpediente nuevo)
    {
        ExpedienteId = expedienteId;
        EstadoAnterior = anterior;
        EstadoNuevo = nuevo;
    }
}