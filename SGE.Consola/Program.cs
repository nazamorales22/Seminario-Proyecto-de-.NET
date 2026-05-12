/*

using SGE.Aplicacion;
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
}*/

using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Infraestructura;
using SGE.Dominio.Tramites;

// 1. CONFIGURACIÓN (Inyección de dependencias manual)
// Aquí creamos los "motores" que graban en el disco de tu Linux Mint
var repoExpediente = new ExpedienteRepositoryTxt();
var repoTramite = new TramiteRepositoryTxt();

// Creamos un servicio de autorización "falso" por ahora (que siempre dice que sí)
// Luego lo podés completar con la lógica real
var authService = new FakeAutorizacionService(); 

// Instanciamos los Casos de Uso pasándoles los repositorios
var altaExpediente = new AltaExpedienteUseCase(repoExpediente, authService);
var altaTramite = new AltaTramiteUseCase(repoTramite, repoExpediente, authService);
var listarExpedientes = new ListarExpedientesUseCase(repoExpediente);
// 2. MENÚ DE USUARIO
bool salir = false;
while (!salir)
{   Console.WriteLine("\n====================================");
    Console.WriteLine("   SGE: GESTIÓN DE EXPEDIENTES");
    Console.WriteLine("====================================");

    Console.WriteLine("\n--- SISTEMA DE GESTIÓN DE EXPEDIENTES (SGE) ---");
    Console.WriteLine("1. Dar de alta un expediente");
    Console.WriteLine("2. Dar de alta un trámite");
    Console.WriteLine("3. Listar expedientes");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opción: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Ingrese la carátula del expediente: ");
            string? caratula = Console.ReadLine();
            try {
                // Usamos un Guid fijo o aleatorio para el usuario por ahora
                altaExpediente.Ejecutar(caratula ?? "", Guid.NewGuid());
                Console.WriteLine("¡Expediente creado con éxito!");
            } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
            break;

        case "2":
            Console.Write("Ingrese el ID del expediente (GUID): ");
            if (Guid.TryParse(Console.ReadLine(), out Guid expId)) {
                Console.Write("Ingrese el contenido del trámite: ");
                string contenido = Console.ReadLine() ?? "";
                
                // Mostramos opciones de etiquetas (puedes Hardcodear una por ahora)
                Console.WriteLine("Etiqueta: 1.Resolucion, 2.PaseAEstudio, 3.PaseAlArchivo");
                // Aquí podrías parsear la etiqueta, por ahora mandamos 'PaseAEstudio' de prueba
                try {
                    altaTramite.Ejecutar(expId, EtiquetaTramite.PaseAEstudio, contenido, Guid.NewGuid());
                    Console.WriteLine("¡Trámite cargado y estado de expediente actualizado!");
                } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
            } else {
                Console.WriteLine("ID de expediente no válido.");
            }
            break;

        case "3":
            Console.WriteLine("\n--- LISTADO DE EXPEDIENTES ---");
            var lista = listarExpedientes.Ejecutar();
            if (!lista.Any()) {
                Console.WriteLine("No hay expedientes cargados.");
            } else {
                foreach (var e in lista) {
                    Console.WriteLine($"ID: {e.Id} | Carátula: {e.Caratula} | Estado: {e.Estado}");
                }
            }
            break;

        case "0": salir = true; break;
        default: Console.WriteLine("Opción no válida."); break;
    }
}

// Clase auxiliar temporal para que el código compile
public class FakeAutorizacionService : SGE.Aplicacion.Autorizacion.IAutorizacionService {
    public bool PoseeElPermiso(Guid id, SGE.Aplicacion.Autorizacion.Permiso p) => true;
}