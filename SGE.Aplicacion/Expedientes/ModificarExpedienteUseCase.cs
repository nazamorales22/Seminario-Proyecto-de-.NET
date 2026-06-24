using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion;

namespace SGE.Aplicacion.Expedientes;

public class ModificarExpedienteUseCase(
    IExpedienteRepository repo,
    IAutorizacionService auth,
    IUnidadDeTrabajo unidadDeTrabajo)
{
    public ExpedienteResponse Ejecutar(ModificarExpedienteRequest request)
    {
        if (!auth.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("No tenés permiso para modificar expedientes.");

        var expediente = repo.ObtenerPorId(request.Id)
            ?? throw new EntidadNoEncontradaException("Expediente no encontrado.");

        expediente.ActualizarCaratula(new Caratula(request.NuevaCaratula), request.IdUsuario);
        repo.Modificar(expediente);
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