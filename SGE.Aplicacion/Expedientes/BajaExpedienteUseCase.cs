using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Aplicacion;


//rebisar que cumpla: 
//El permiso ExpedienteBaja implica implícitamente contar con el permiso TramiteBaja.


namespace SGE.Aplicacion.Expedientes;

public class BajaExpedienteUseCase(
    IExpedienteRepository repoExpediente,
    ITramiteRepository repoTramite,
    IAutorizacionService auth,
    IUnidadDeTrabajo unidadDeTrabajo)
{
    public void Ejecutar(BajaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
            throw new AutorizacionException("No tenés permiso para eliminar expedientes.");

        var expediente = repoExpediente.ObtenerPorId(request.Id)
            ?? throw new EntidadNoEncontradaException("No se encontró el expediente.");

        repoTramite.EliminarRelacionadosA(request.Id);
        repoExpediente.Eliminar(request.Id);
        unidadDeTrabajo.Guardar();
    }
}