using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Infraestructura;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;

// =========================================================================
// 🗄️ NUEVA PERSISTENCIA: Inicializamos Entity Framework Core
// =========================================================================
using var context = new SGEDbContext();
// Nos aseguramos de que la base de datos esté creada físicamente antes de arrancar
context.Database.EnsureCreated(); 

// Inicializamos tus nuevos repositorios SQL pasándoles el contexto
var repoExpediente = new ExpedienteRepositorySql(context);
var repoTramite = new TramiteRepositorySql(context);

// =========================================================================
// El resto de los servicios y Casos de Uso quedan EXACTAMENTE IGUAL...
// =========================================================================
var authService = new AutorizacionProvisionalService();
var actualizacionEstado = new ActualizacionEstadoExpedienteService(repoExpediente, repoTramite);

// Casos de Uso de Expedientes...
/*


using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Infraestructura;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;

var repoExpediente = new ExpedienteRepositoryTxt();
var repoTramite = new TramiteRepositoryTxt();

var authService = new AutorizacionProvisionalService();

// Servicios primero, porque otros Casos de Uso los necesitan
var actualizacionEstado = new ActualizacionEstadoExpedienteService(repoExpediente, repoTramite);
*/
// Casos de Uso de Expedientes
var altaExpediente = new AltaExpedienteUseCase(repoExpediente, authService);
var listarExpedientes = new ListarExpedientesUseCase(repoExpediente);
var bajaExpedienteUseCase = new BajaExpedienteUseCase(repoExpediente, repoTramite, authService);
var modificarExpediente = new ModificarExpedienteUseCase(repoExpediente, authService);
var cambiarEstadoManual = new CambiarEstadoExpedienteUseCase(repoExpediente, authService);
var listarPorEstado = new ListarExpedientesPorEstadoUseCase(repoExpediente);
var consultarPorEtiqueta = new ConsultarExpedientesPorEtiquetaUseCase(repoExpediente, repoTramite);

// Casos de Uso de Trámites
var altaTramite = new AltaTramiteUseCase(repoTramite, repoExpediente, authService, actualizacionEstado);
var modificarTramite = new ModificarTramiteUseCase(repoTramite, authService, actualizacionEstado);
var bajaTramite = new BajaTramiteUseCase(repoTramite, authService, actualizacionEstado);
var listarTramites = new ListarTramitesUseCase(repoTramite);

bool salir = false;//para el menu

while (!salir)
{   
    Console.WriteLine("\n====================================");
    Console.WriteLine("   SGE: GESTIÓN DE EXPEDIENTES");
    Console.WriteLine("====================================");
    Console.WriteLine("1. Dar de alta un expediente");
    Console.WriteLine("2. Dar de alta un trámite");
    Console.WriteLine("3. Listar expedientes");
    Console.WriteLine("4. Listar trámites de un expediente");
    Console.WriteLine("5. Dar de baja un Expediente (Cascada)");
    Console.WriteLine("6. Dar de baja un trámite");
    Console.WriteLine("7. Modificar un Expediente");
    Console.WriteLine("8. Modificar un trámite");    
    Console.WriteLine("9. Cambiar estado de expediente (Manual)");
    Console.WriteLine("10. Informe: Expedientes por estado");
    Console.WriteLine("11. Informe: Expedientes con trámites de tipo...");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opción: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Ingrese la carátula del expediente: ");
            string? caratula = Console.ReadLine();
            try {
                var request = new AltaExpedienteRequest(caratula ?? "", Guid.NewGuid());
                var response = altaExpediente.Ejecutar(request);
                Console.WriteLine($"Expediente creado exitosamente con ID: {response.Id}");
            } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
            break;

        case "2":
           Console.Write("Ingrese el ID del expediente (GUID): ");
           if (Guid.TryParse(Console.ReadLine(), out Guid expId)) {

                // Verificamos que el expediente exista antes de seguir
                var expExiste = repoExpediente.ObtenerPorId(expId);
                if (expExiste == null)                {
                    Console.WriteLine("Error: No se encontró el expediente.");
                    break;
                }

                Console.Write("Ingrese el contenido del trámite: ");
                string contenidoStr = Console.ReadLine() ?? "";
                try {
                    var request = new AltaTramiteRequest(expId, contenidoStr, EtiquetaTramite.PaseAEstudio, Guid.NewGuid());
                    var response = altaTramite.Ejecutar(request);
                    Console.WriteLine($"Trámite cargado exitosamente con ID: {response.Id}");
             } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
           } else {
                Console.WriteLine("ID de expediente no válido.");
           }
           break;

        case "3":
            Console.WriteLine("\n--- LISTADO DE EXPEDIENTES ---");
            try {
                var lista = listarExpedientes.Ejecutar();
                foreach (var e in lista) {
                    Console.WriteLine($"ID: {e.Id} | Carátula: {e.Caratula} | Estado: {e.Estado}");
                }
                
            } catch (Exception e) { Console.WriteLine($"Error al listar: {e.Message}"); }
            break;
        
        case "4":
            Console.WriteLine("\n--- LISTAR TRÁMITES DE UN EXPEDIENTE ---");
            Console.Write("Ingrese el ID del expediente: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid idExpLista))
            {

                // Verificamos que el expediente exista antes de seguir
                var expExiste = repoExpediente.ObtenerPorId(idExpLista);
                if (expExiste == null)              {
                    Console.WriteLine("Error: No se encontró el expediente.");  
                    break;
                }

                try {
                    var tramites = listarTramites.Ejecutar(idExpLista);
                    foreach (var t in tramites)
                        Console.WriteLine($"ID: {t.Id} | Etiqueta: {t.Etiqueta} | Contenido: {t.Contenido}");
                } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
            }
            else Console.WriteLine("ID inválido.");
        break;


        case "5":
             Console.WriteLine("\n--- BAJA DE EXPEDIENTE (CASCADA) ---");
             Console.Write("Ingrese el ID del expediente a eliminar: ");
             string? idInput = Console.ReadLine();
    
             if (Guid.TryParse(idInput, out Guid idParaBorrar))
             {
                try 
                {
                    var request = new BajaExpedienteRequest(idParaBorrar, Guid.NewGuid());
                    bajaExpedienteUseCase.Ejecutar(request);
                    Console.WriteLine("Expediente y sus trámites asociados eliminados correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}"); // Error corregido: agregada la $
                }
             }
             else
             {
                  Console.WriteLine("ID inválido. Debe ser un formato Guid.");
             }
             break;

        case "6":
            Console.WriteLine("\n--- BAJA DE TRÁMITE ---");        
            Console.Write("Ingrese el ID del trámite a eliminar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid idBajaTramite))
            {

                try {
                    var request = new BajaTramiteRequest(idBajaTramite, Guid.NewGuid());
                    bajaTramite.Ejecutar(request);
                    Console.WriteLine("Trámite eliminado y estado del expediente actualizado.");
                } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
            }
            else Console.WriteLine("ID inválido.");
        break;


        case "7":
            Console.WriteLine("\n--- MODIFICAR EXPEDIENTE ---");
           Console.Write("Ingrese el ID del expediente a modificar: ");
           if (Guid.TryParse(Console.ReadLine(),  out Guid idMod)) // con el true hacemos que no distinga mayúsculas de minúsculas
           {

            // Verificamos que el expediente exista antes de seguir
            var expExiste = repoExpediente.ObtenerPorId(idMod);
            if (expExiste == null)            
            {
                Console.WriteLine("Error: No se encontró el expediente.");
                break;
            }
                
              try {

                    Console.Write("Ingrese la nueva carátula: ");
                    string nuevaC = Console.ReadLine() ?? "";

                    var request = new ModificarExpedienteRequest(idMod, nuevaC, Guid.NewGuid());
                    var response = modificarExpediente.Ejecutar(request);
                    Console.WriteLine($"Expediente modificado con éxito. ID: {response.Id}");
            } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
           } else {
                Console.WriteLine("ID de expediente no válido.");
           }    
         break;

        case "8":
            Console.WriteLine("\n--- MODIFICAR TRÁMITE ---");
            Console.Write("Ingrese el ID del trámite a modificar: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid idTramite))
            {

                // Verificamos que el trámite exista antes de seguir
                var tramiteExiste = repoTramite.ObtenerPorId(idTramite);
                if (tramiteExiste == null)                
                {
                    Console.WriteLine("Error: No se encontró el trámite.");
                    break;
                }

                Console.Write("Ingrese el nuevo contenido: ");
                string nuevoContenido = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(nuevoContenido)) 
                {
                    Console.WriteLine("Error: El contenido no puede estar vacío.");
                    break;
                }

                Console.Write("Ingrese la nueva etiqueta (0=EscritoPresentado, 1=PaseAEstudio, 2=Despacho, 3=Resolucion, 4=Notificacion, 5=PaseAlArchivo): ");
                if (Enum.TryParse<EtiquetaTramite>(Console.ReadLine(), true, out EtiquetaTramite etiqueta)
                    && Enum.IsDefined(typeof(EtiquetaTramite), etiqueta))
                {
                    try {
                        var request = new ModificarTramiteRequest(idTramite, nuevoContenido, etiqueta, Guid.NewGuid());
                        var response = modificarTramite.Ejecutar(request);
                        Console.WriteLine($"Trámite modificado exitosamente con ID: {response.Id}");
                    } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
                }
                else Console.WriteLine("Etiqueta no válida.");
            }
            else Console.WriteLine("ID inválido.");
        break;



        case "9":
            Console.WriteLine("\n--- CAMBIAR ESTADO DE EXPEDIENTE (MANUAL) ---");
            Console.Write("Ingrese el ID del expediente: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid idExp)) 
            {
                // Verificamos que el expediente exista antes de seguir
                var expExiste = repoExpediente.ObtenerPorId(idExp);
                if (expExiste == null)
                {
                    Console.WriteLine("Error: No se encontró el expediente.");
                    break;
                }

                Console.WriteLine("Estados: 0= RecienIniciado, 1= ParaResolver, 2= ConResolucion, 3= EnNotificacion, 4= Finalizado");
                Console.Write("Seleccione el número del nuevo estado: ");
                if (Enum.TryParse<EstadoExpediente>(Console.ReadLine(), true, out EstadoExpediente nuevoEstado))
                {
                    try {
                        var request = new CambiarEstadoExpedienteRequest(idExp, nuevoEstado, Guid.NewGuid());
                        var response = cambiarEstadoManual.Ejecutar(request);
                        Console.WriteLine("Estado actualizado correctamente.");
                    } 
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                } 
                else Console.WriteLine("Estado no válido.");
            } 
            else Console.WriteLine("ID de expediente inválido.");
        break;
        
        case "10":
             Console.WriteLine("\n--- INFORME: EXPEDIENTES POR ESTADO ---");
             Console.WriteLine("0: RecienIniciado, 1: ParaResolver, 2: ConResolucion, 3: EnNotificacion, 4: Finalizado");
             Console.Write("Seleccione el estado: ");

             string? leerEstado = Console.ReadLine();// Leemos la entrada del usuario para el estado del expediente
             if (Enum.TryParse<EstadoExpediente>(leerEstado, true, out EstadoExpediente estFiltro )// con el true hacemos que no distinga mayúsculas de minúsculas
                && Enum.IsDefined(typeof(EstadoExpediente), estFiltro))
             {
                var request = new ListarExpedientesPorEstadoRequest(estFiltro);
                var filtrados = listarPorEstado.Ejecutar(request);
                Console.WriteLine($"\nResultados ({estFiltro}):");
                foreach (var e in filtrados) Console.WriteLine($"- {e.Caratula} (ID: {e.Id})");
                if (!filtrados.Any()) Console.WriteLine("No se encontraron resultados.");
            }
            else Console.WriteLine("Estado no válido.");
    break;

        case "11":
            Console.WriteLine("\n--- INFORME: EXPEDIENTES POR TIPO DE TRÁMITE ---");
            Console.WriteLine("0: Escrito, 1: PaseAEstudio, 2: Despacho, 3: Resolucion, 4: Notificacion, 5: Archivo");
            Console.Write("Seleccione la etiqueta de trámite: ");

            string? leerEtiqueta = Console.ReadLine();// Leemos la entrada del usuario para la etiqueta de trámite
            if (Enum.TryParse<EtiquetaTramite>(leerEtiqueta, true ,out EtiquetaTramite etiFiltro)// con el true hacemos que no distinga mayúsculas de minúsculas
                && Enum.IsDefined(typeof(EtiquetaTramite), etiFiltro))
            {
                var request = new ConsultarExpedientesPorEtiquetaRequest(etiFiltro);
                var filtrados = consultarPorEtiqueta.Ejecutar(request);
                Console.WriteLine($"\nExpedientes que tienen trámites de tipo {etiFiltro}:");
                foreach (var e in filtrados) Console.WriteLine($"- {e.Caratula} (ID: {e.Id})");
                if (!filtrados.Any()) Console.WriteLine("No se encontraron expedientes con esos trámites.");
            }
            else Console.WriteLine("Etiqueta no válida.");
    break;

        case "0": salir = true; break;
        default: Console.WriteLine("Opción no válida."); break;
        
    }
}
