namespace SGE.Aplicacion;

public record ExpedienteDTO(
    int Id, 
    string Caratula, 
    DateTime FechaCreacion, 
    string Estado, 
    int IdUsuario
);