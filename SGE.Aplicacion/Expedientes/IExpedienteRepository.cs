using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);
    void Modificar(Expediente expediente);
    void Eliminar(Guid id);
    Expediente? ObtenerPorId(Guid id); // Retorna null si no existe [cite: 121]
    IEnumerable<Expediente> ObtenerTodos(); // Usa IEnumerable según Apéndice A [cite: 127]
}