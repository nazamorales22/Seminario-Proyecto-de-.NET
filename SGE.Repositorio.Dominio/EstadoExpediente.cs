namespace SGE.Repositorio.Dominio;

public enum EstadoExpediente
{
    RecienCreado,
    ParaResolver,
    ConResolucion,
    EnNotificacion,
    Finalizado
}

//Este define los estados legales que pide el TP.