using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repos, IAutorizacionService auth)
{
    public void Ejecutar(string caratulaTexto, Guid usuarioId)
    {
        // 1. Validar Permisos
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteAlta))
        {
            throw new DominioException("No tenés permiso para dar de alta expedientes.");
        }

        // 2. Crear el objeto de dominio (la validación de la carátula ocurre acá adentro)
        var caratula = new Caratula(caratulaTexto);
        var expediente = new Expediente(caratula, usuarioId);

        // 3. Persistir
        repos.Agregar(expediente);
    }
}