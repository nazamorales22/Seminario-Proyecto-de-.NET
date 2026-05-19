using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;        // <--- AGREGÁ ESTO

namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(
    IExpedienteRepository repoExpediente, 
    ITramiteRepository repoTramite, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, Guid usuarioId)
    {
        // verifica si tien los permisos
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteBaja))
        {
            throw new DominioException("No tenés permiso para eliminar expedientes.");
        }

        //verifica si el expediente existe, sino lanza excepción
        var expediente = repoExpediente.ObtenerPorId(expedienteId)
        ?? throw new DominioException("No se encontró el expediente.");

        
        repoTramite.EliminarRelacionadosA(expedienteId);

        
        repoExpediente.Eliminar(expedienteId);
    }
}
