using SGE.Repositorio.Dominio;

namespace SGE.Repositorio.Aplicacion;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);
    void Eliminar(int id);
    void Modificar(Expediente expediente);
    IEnumerable<Expediente> ObtenerTodos();
    Expediente? ObtenerPorId(int id);
}