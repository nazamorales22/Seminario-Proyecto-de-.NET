using SGE.Repositorio.Dominio;

namespace SGE.Repositorio.Aplicacion;

public interface ITramiteRepository
{
    void Agregar(Tramite tramite);
    IEnumerable<Tramite> ObtenerPorExpediente(int expedienteId);
    void Eliminar(int id);
}