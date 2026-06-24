using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Comun;

namespace SGE.WebApi.Middlewares;

public class ManejadorGlobalDeExcepciones : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, titulo, detalle) = exception switch
        {
            AutorizacionException => (StatusCodes.Status403Forbidden, "No autorizado", exception.Message),
            EntidadNoEncontradaException => (StatusCodes.Status404NotFound, "Recurso no encontrado", exception.Message),
            DominioException => (StatusCodes.Status400BadRequest, "Error de validación", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor", "Ocurrió un error inesperado. Contacte al administrador.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = titulo,
            Detail = detalle
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}