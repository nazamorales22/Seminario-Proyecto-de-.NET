using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Tramites;

namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(
    IExpedienteRepository repoExpediente, 
    ITramiteRepository repoTramite, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, Guid usuarioId)
    {
        // 1. Validar Permisos
        // Cambié AutorizacionException por DominioException que es la que ya tenés creada
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteBaja))
        {
            throw new DominioException("No tenés permiso para eliminar expedientes.");
        }

        // 2. Coordinar la eliminación en cascada (Página 7 del TP)
        repoTramite.EliminarRelacionadosA(expedienteId);

        // 3. Finalmente eliminamos el expediente
        repoExpediente.Eliminar(expedienteId);
    }
}
