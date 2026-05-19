
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;
namespace SGE.Aplicacion.Tramites;

public class AltaTramiteUseCase(
    ITramiteRepository repoTramite, 
    IExpedienteRepository repoExpediente, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, string contenidoTexto, EtiquetaTramite etiqueta, Guid usuarioId)
    {
        //verifico los permisos
        if (!auth.PoseeElPermiso(usuarioId, Permiso.TramiteAlta))
        {
            throw new DominioException("No tenés permiso para crear trámites.");
        }

        // Verifico que el expediente exista
        var expediente = repoExpediente.ObtenerPorId(expedienteId) 
            ?? throw new DominioException("El expediente no existe.");

        //creo el trámite
        var contenido = new ContenidoTramite(contenidoTexto);
        var tramite = new Tramite(expedienteId, etiqueta, contenido, usuarioId);

       // el expediente
        expediente.ActualizarEstado(etiqueta, usuarioId);

       //guardo los cambios
        repoTramite.Agregar(tramite);
        repoExpediente.Modificar(expediente);
    }
}