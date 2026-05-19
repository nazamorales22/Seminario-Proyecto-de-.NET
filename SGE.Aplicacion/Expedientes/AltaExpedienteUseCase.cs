using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;

namespace SGE.Aplicacion.Expedientes;

public class AltaExpedienteUseCase(IExpedienteRepository repos, IAutorizacionService auth)
{
    public void Ejecutar(string caratulaTexto, Guid usuarioId)
    {
        //verifico los permisos
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteAlta))
        {
            throw new DominioException("No tenés permiso para dar de alta expedientes.");
        }

        

        //creo el expediente
        var caratula = new Caratula(caratulaTexto);
        var expediente = new Expediente(caratula, usuarioId);

        
        repos.Agregar(expediente);
    }
}