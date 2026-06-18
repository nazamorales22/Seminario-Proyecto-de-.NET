using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

// lo usa  AltaTramiteUseCase
public record AltaTramiteRequest(Guid ExpedienteId, string Contenido, EtiquetaTramite Etiqueta, Guid IdUsuario);

// lo usa  ModificarTramiteUseCase
public record ModificarTramiteRequest(Guid Id, string Contenido, EtiquetaTramite Etiqueta, Guid IdUsuario);

// lo usa  BajaTramiteUseCase
public record BajaTramiteRequest(Guid Id, Guid IdUsuario);

// lo usa ListarTramitesUseCase (Response usado por Alta, Modificar y Listar)
public record TramiteResponse(
    Guid Id,
    Guid ExpedienteId,
    EtiquetaTramite Etiqueta,
    string Contenido,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio
);