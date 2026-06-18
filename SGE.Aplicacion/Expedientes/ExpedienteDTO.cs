using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Expedientes;

// lo usa  AltaExpedienteUseCase
public record AltaExpedienteRequest(string Caratula, Guid IdUsuario);

//  lo usa  ModificarExpedienteUseCase (modificar carátula)
public record ModificarExpedienteRequest(Guid Id, string NuevaCaratula, Guid IdUsuario);

// lo usa  CambiarEstadoExpedienteUseCase
public record CambiarEstadoExpedienteRequest(Guid Id, EstadoExpediente NuevoEstado, Guid IdUsuario);

// lo usa  BajaExpedienteUseCase
public record BajaExpedienteRequest(Guid Id, Guid IdUsuario);

// lo usa  ConsultarExpedientesPorEtiquetaUseCase
public record ConsultarExpedientesPorEtiquetaRequest(EtiquetaTramite Etiqueta);

// lo usa  ListarExpedientesPorEstadoUseCase
public record ListarExpedientesPorEstadoRequest(EstadoExpediente Estado);

// lo usan  Alta, Modificar, CambiarEstado, Listar y los informes
public record ExpedienteResponse(
    Guid Id,
    string Caratula,
    EstadoExpediente Estado,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio
);