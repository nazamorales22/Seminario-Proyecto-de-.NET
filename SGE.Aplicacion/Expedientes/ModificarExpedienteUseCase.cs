using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ModificarExpedienteUseCase(
    IExpedienteRepository repo, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, string nuevaCaratula, Guid usuarioId)
    {
        //verificar permisos
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteModificacion))
        {
            throw new DominioException("No tenés permiso para modificar expedientes.");
        }

        //
        var expediente = repo.ObtenerPorId(expedienteId);
        if (expediente == null) throw new DominioException("Expediente no encontrado.");

        
        expediente.ActualizarCaratula(new Caratula(nuevaCaratula), usuarioId);

        //guardamos los cambios
        repo.Modificar(expediente);
    }
}