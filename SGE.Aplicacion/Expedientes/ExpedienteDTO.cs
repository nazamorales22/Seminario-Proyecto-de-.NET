namespace SGE.Aplicacion.Expedientes;

public class ExpedienteDTO
{
    public Guid Id { get; set; }
    public string Caratula { get; set; } = "";
    public string Estado { get; set; } = "";
    // Agregamos estos si los usás en el listado, si no, dejalos así
}