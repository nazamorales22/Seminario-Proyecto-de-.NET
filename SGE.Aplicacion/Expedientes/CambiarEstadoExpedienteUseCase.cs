using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;
namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase(
    IExpedienteRepository repo,
    IAutorizacionService auth)
{
    public ExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("No tiene permisos para cambiar el estado.");

        var expediente = repo.ObtenerPorId(request.Id)
            ?? throw new DominioException("Expediente no encontrado.");  // 

        expediente.CambiarEstado(request.NuevoEstado, request.IdUsuario);
        repo.Modificar(expediente);

        return new ExpedienteResponse(
            expediente.Id,
            expediente.Caratula.Valor,
            expediente.Estado,
            expediente.FechaCreacion,
            expediente.FechaUltimaModificacion,
            expediente.UsuarioUltimoCambio
        );
    }
}