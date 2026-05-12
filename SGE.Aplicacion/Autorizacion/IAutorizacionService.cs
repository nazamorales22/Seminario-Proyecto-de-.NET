namespace SGE.Aplicacion.Autorizacion;

public enum Permiso 
{ 
    ExpedienteAlta, ExpedienteBaja, ExpedienteModificacion, 
    TramiteAlta, TramiteBaja, TramiteModificacion 
}

public interface IAutorizacionService
{
    bool PoseeElPermiso(Guid idUsuario, Permiso permiso);
}