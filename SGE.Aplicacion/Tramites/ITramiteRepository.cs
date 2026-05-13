using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    void Eliminar(Guid id);
    void EliminarRelacionadosA(Guid expedienteId);
    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);
}