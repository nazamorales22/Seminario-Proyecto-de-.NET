using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(
    IExpedienteRepository repoExpediente,
    ITramiteRepository repoTramite,
    IAutorizacionService auth)
{
    public void Ejecutar(BajaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
            throw new AutorizacionException("No tenés permiso para eliminar expedientes.");  // ← corregido

        var expediente = repoExpediente.ObtenerPorId(request.Id)
            ?? throw new DominioException("No se encontró el expediente.");

        repoTramite.EliminarRelacionadosA(request.Id);
        repoExpediente.Eliminar(request.Id);
    }
}