
//Sirve para capturar errores especificos en el sistema

namespace SGE.Dominio.Comun;

public class DominioException(string mensaje) : Exception(mensaje);
