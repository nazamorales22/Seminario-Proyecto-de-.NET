/*using SGE.Repositorio.Aplicacion;
using SGE.Repositorio.Infraestructura;

// 1. Inicializamos los "fierros" (Infraestructura)
var repoExpediente = new ExpedienteRepositoryTxt();

// 2. Inicializamos los "cerebros" (Casos de Uso)
var agregarExpUseCase = new AgregarExpedienteUseCase(repoExpediente);
var listarExpUseCase = new ListarExpedientesUseCase(repoExpediente);

bool salir = false;
while (!salir)
{
    Console.WriteLine("\n--- SGE: Gestión de Expedientes ---");
    Console.WriteLine("1. Agregar Expediente");
    Console.WriteLine("2. Listar Expedientes");
    Console.WriteLine("3. Salir");
    Console.Write("Seleccione una opción: ");

    string? opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            Console.Write("Ingrese la carátula: ");
            string caratula = Console.ReadLine() ?? "";
            
            // Usamos el DTO para enviar datos a la capa de aplicación
            var nuevoDto = new ExpedienteDTO(0, caratula, DateTime.Now, "", 14748); 
            
            try {
                agregarExpUseCase.Ejecutar(nuevoDto);
                Console.WriteLine("¡Expediente guardado con éxito!");
            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            break;

        case "2":
            var lista = listarExpUseCase.Ejecutar();
            Console.WriteLine("\n--- Listado de Expedientes ---");
            foreach (var e in lista) {
                Console.WriteLine($"ID: {e.Id} | Carátula: {e.Caratula} | Estado: {e.Estado}");
            }
            break;

        case "3":
            salir = true;
            break;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}*/

using SGE.Repositorio.Aplicacion;
using SGE.Repositorio.Infraestructura;

// --- CONFIGURACIÓN DE LA INFRAESTRUCTURA ---
var repoExpediente = new ExpedienteRepositoryTxt();
var repoTramite = new TramiteRepositoryTxt();

// --- CONFIGURACIÓN DE LOS CASOS DE USO ---
var agregarExpUseCase = new AgregarExpedienteUseCase(repoExpediente);
var listarExpUseCase = new ListarExpedientesUseCase(repoExpediente);
var agregarTramiteUseCase = new AgregarTramiteUseCase(repoTramite, repoExpediente);
var listarTramitesUseCase = new ListarTramitesUseCase(repoTramite);

bool salir = false;
while (!salir)
{
    Console.WriteLine("\n====================================");
    Console.WriteLine("   SGE: GESTIÓN DE EXPEDIENTES");
    Console.WriteLine("====================================");
    Console.WriteLine("1. Nuevo Expediente");
    Console.WriteLine("2. Listar todos los Expedientes");
    Console.WriteLine("3. Agregar Trámite a un Expediente");
    Console.WriteLine("4. Ver Trámites de un Expediente");
    Console.WriteLine("5. Salir");
    Console.Write("\nSeleccione una opción: ");

    string? opcion = Console.ReadLine();

    try
    {
        switch (opcion)
        {
            case "1":
                Console.Write("Ingrese la carátula: ");
                string caratula = Console.ReadLine() ?? "";
                // Usamos tu legajo 14748 como ID de usuario
                agregarExpUseCase.Ejecutar(new ExpedienteDTO(0, caratula, DateTime.Now, "", 14748));
                Console.WriteLine(">> Expediente creado correctamente.");
                break;

            case "2":
                var expedientes = listarExpUseCase.Ejecutar();
                Console.WriteLine("\n--- LISTA DE EXPEDIENTES ---");
                foreach (var e in expedientes)
                    Console.WriteLine($"ID: {e.Id} | {e.Caratula} | Estado: {e.Estado}");
                break;

            case "3":
                Console.Write("ID del Expediente: ");
                int idExp = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Contenido del trámite: ");
                string contenido = Console.ReadLine() ?? "";
                
                agregarTramiteUseCase.Ejecutar(new TramiteDTO(0, idExp, contenido, DateTime.Now, 14748));
                Console.WriteLine(">> Trámite agregado.");
                break;

            case "4":
                Console.Write("Ingrese el ID del Expediente para ver sus trámites: ");
                int idBusqueda = int.Parse(Console.ReadLine() ?? "0");
                var tramites = listarTramitesUseCase.Ejecutar(idBusqueda);
                
                Console.WriteLine($"\n--- TRÁMITES DEL EXPEDIENTE {idBusqueda} ---");
                if (!tramites.Any()) Console.WriteLine("No hay trámites registrados.");
                foreach (var t in tramites)
                    Console.WriteLine($"[{t.FechaHora}] - {t.Contenido}");
                break;

            case "5":
                salir = true;
                break;

            default:
                Console.WriteLine("Opción no válida.");
                break;
        }
    }
    catch (Exception ex)
    {
        // Capturamos errores de validación (carátula vacía, expediente inexistente, etc.)
        Console.WriteLine($"\n[ERROR] {ex.Message}");
    }
}