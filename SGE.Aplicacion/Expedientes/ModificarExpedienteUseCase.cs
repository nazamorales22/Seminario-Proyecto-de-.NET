using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
namespace SGE.Aplicacion.Expedientes;

public class ModificarExpedienteUseCase(
    IExpedienteRepository repo,
    IAutorizacionService auth)
{
    public ExpedienteResponse Ejecutar(ModificarExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("No tenés permiso para modificar expedientes.");  // ← corregido

        var expediente = repo.ObtenerPorId(request.Id)
            ?? throw new DominioException("Expediente no encontrado.");

        expediente.ActualizarCaratula(new Caratula(request.NuevaCaratula), request.IdUsuario);
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