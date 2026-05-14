using SGE.Dominio.Tramites;

namespace SGE.Dominio.Tramites;

public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    void Eliminar(Guid id);
    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);
    void EliminarRelacionadosA(Guid expedienteId);
    // Si agregaron modificar tramite, va acá:
    void Modificar(Tramite tramite); 
}