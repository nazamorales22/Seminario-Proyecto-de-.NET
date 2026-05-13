using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ModificarExpedienteUseCase(
    IExpedienteRepository repo, 
    IAutorizacionService auth)
{
    public void Ejecutar(Guid expedienteId, string nuevaCaratula, Guid usuarioId)
    {
        // 1. Validar Permisos (Pág. 7 del TP)
        if (!auth.PoseeElPermiso(usuarioId, Permiso.ExpedienteModificacion))
        {
            throw new DominioException("No tenés permiso para modificar expedientes.");
        }

        // 2. Buscar el expediente actual
        var expediente = repo.ObtenerPorId(expedienteId);
        if (expediente == null) throw new DominioException("Expediente no encontrado.");

        // 3. Modificar (Usamos el comportamiento del Dominio)
        // Nota: Deberías tener un método en la clase Expediente para cambiar la carátula
        expediente.ActualizarCaratula(new Caratula(nuevaCaratula), usuarioId);

        // 4. Persistir
        repo.Modificar(expediente);
    }
}