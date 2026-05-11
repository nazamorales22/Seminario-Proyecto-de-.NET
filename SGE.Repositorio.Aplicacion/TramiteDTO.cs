namespace SGE.Repositorio.Aplicacion;

public record TramiteDTO(
    int Id,
    int ExpedienteId,
    string Contenido,
    DateTime FechaHora,
    int IdUsuario
);