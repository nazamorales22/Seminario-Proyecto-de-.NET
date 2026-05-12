

using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;

namespace SGE.Aplicacion.Tramites;

public class AltaTramiteUseCase(
    ITramiteRepository repoTramite, 
    IExpedienteRepository repoExpediente, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, string contenidoTexto, EtiquetaTramite etiqueta, Guid usuarioId)
    {
        // 1. Validar Permisos
        if (!auth.PoseeElPermiso(usuarioId, Permiso.TramiteAlta))
        {
            throw new DominioException("No tenés permiso para crear trámites.");
        }

        // 2. Buscar si el expediente existe
        var expediente = repoExpediente.ObtenerPorId(expedienteId) 
            ?? throw new DominioException("El expediente no existe.");

        // 3. Crear el trámite (la validación del contenido ocurre en el Value Object)
        var contenido = new ContenidoTramite(contenidoTexto);
        var tramite = new Tramite(expedienteId, etiqueta, contenido, usuarioId);

        // 4. EL MOMENTO CLAVE: El expediente actualiza su propio estado
        // según la etiqueta del nuevo trámite.
        expediente.ActualizarEstado(etiqueta, usuarioId);

        // 5. Persistir ambos cambios
        repoTramite.Agregar(tramite);
        repoExpediente.Modificar(expediente);
    }
}