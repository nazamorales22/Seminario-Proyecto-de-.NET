using SGE.Repositorio.Dominio;

namespace SGE.Repositorio.Aplicacion;

public class AgregarTramiteUseCase(ITramiteRepository repoTramite, IExpedienteRepository repoExp)
{
    public void Ejecutar(TramiteDTO dto)
    {
        // Validamos que el expediente exista antes de pegarle un trámite
        var exp = repoExp.ObtenerPorId(dto.ExpedienteId) 
                  //?? throw new SGEException("El expediente no existe.");
                    ?? throw new Exception("El expediente no existe.");
        var tramite = new Tramite(dto.ExpedienteId, dto.Contenido, dto.IdUsuario);
        repoTramite.Agregar(tramite);
    }
}
