using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    void Eliminar(Guid id);
    void Modificar(Tramite tramite);
    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);
    void EliminarRelacionadosA(Guid expedienteId);
    Tramite? ObtenerPorId(Guid id);
    IEnumerable<Tramite> ObtenerTodos();
}