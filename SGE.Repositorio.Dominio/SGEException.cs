namespace SGE.Repositorio.Dominio;

public class SGEException : Exception
{
    public SGEException(string message) : base(message) { }
}
//Sirve para capturar errores especificos en el sistema