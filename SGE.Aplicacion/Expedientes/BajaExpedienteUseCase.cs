using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;
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
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteBaja))
        {
            throw new DominioException("No tenés permiso para eliminar expedientes.");
        }

        // 2. Validar que el expediente exista
        var expediente = repoExpediente.ObtenerPorId(expedienteId) 
            ?? throw new DominioException("El expediente no existe.");

        // 3. REGLA DE NEGOCIO: No debe tener trámites asociados
        var tramites = repoTramite.ObtenerPorExpedienteId(expedienteId);
        if (tramites.Any())
        {
            throw new DominioException("No se puede eliminar un expediente que posee trámites.");
        }

        // 4. Ejecutar la baja
        repoExpediente.Eliminar(expedienteId);
    }
}

