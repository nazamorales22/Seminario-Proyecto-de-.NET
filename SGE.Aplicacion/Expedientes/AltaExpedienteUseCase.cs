using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(
    IExpedienteRepository repos,
    IAutorizacionService auth,
    IUnidadDeTrabajo unidadDeTrabajo)
{
    public ExpedienteResponse Ejecutar(AltaExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
            throw new AutorizacionException("No tenés permiso para dar de alta expedientes.");

        var caratula = new Caratula(request.Caratula);
        var expediente = new Expediente(caratula, request.IdUsuario);

        repos.Agregar(expediente);
        unidadDeTrabajo.Guardar();

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