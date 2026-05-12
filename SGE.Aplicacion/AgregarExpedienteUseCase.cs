using SGE.Repositorio.Dominio;

namespace SGE.Repositorio.Aplicacion;

public class AgregarExpedienteUseCase(IExpedienteRepository repositorio)
{
    public void Ejecutar(ExpedienteDTO dto)
    {
        // Convertimos el DTO (datos sueltos) en una Entidad (negocio con reglas)
        var expediente = new Expediente(dto.Caratula, dto.IdUsuario);
        
        // Le pedimos al repositorio que lo guarde (no sabemos cómo lo guarda, solo que lo hace)
        repositorio.Agregar(expediente);
    }
}
//Este caso de uso es vital porque aquí es donde "mapeamos" los datos.