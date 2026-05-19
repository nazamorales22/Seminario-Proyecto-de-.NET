using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(
    IExpedienteRepository repo, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, EstadoExpediente nuevoEstado, Guid usuarioId)
    {

        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("No tiene permisos para cambiar el estado.");
        }

        //verificar que el expediente exista
        var expediente = repo.ObtenerPorId(expedienteId);
        if (expediente == null) throw new Exception("Expediente no encontrado.");

        expediente.CambiarEstado(nuevoEstado, usuarioId);

        repo.Modificar(expediente);
    }
}