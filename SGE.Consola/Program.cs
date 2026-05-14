
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites;
using SGE.Infraestructura;
using SGE.Dominio.Tramites;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;

// 1. CONFIGURACIÓN (Inyección de dependencias manual)
// Usamos los nombres de variables que ya tenés definidos
var repoExpediente = new ExpedienteRepositoryTxt();
var repoTramite = new TramiteRepositoryTxt();

// ERROR CORREGIDO: Solo dejamos un authService. 
// Usamos el FakeAutorizacionService que definiste abajo para que no pida archivos que no existen.
var authService = new FakeAutorizacionService(); 

// Instanciamos los Casos de Uso
var altaExpediente = new AltaExpedienteUseCase(repoExpediente, authService);
var altaTramite = new AltaTramiteUseCase(repoTramite, repoExpediente, authService);
var listarExpedientes = new ListarExpedientesUseCase(repoExpediente);

// ERROR CORREGIDO: Aquí usamos 'repoExpediente' (el nombre que definiste arriba)
var bajaExpedienteUseCase = new BajaExpedienteUseCase(repoExpediente, repoTramite, authService);
// Instancio el caso de uso para modifica
var modificarExpediente = new ModificarExpedienteUseCase(repoExpediente, authService);


var actualizacionEstado = new ActualizacionEstadoExpedienteService(repoExpediente, repoTramite);
var modificarTramite = new ModificarTramiteUseCase(repoTramite, authService, actualizacionEstado);

var listarTramites = new ListarTramitesUseCase(repoTramite);
var cambiarEstadoManual = new CambiarEstadoExpedienteUseCase(repoExpediente, authService);
// --- NUEVOS CASOS DE USO FASE 2 ---
var listarPorEstado = new ListarExpedientesPorEstadoUseCase(repoExpediente);
// Este lo crearemos a continuación
var consultarPorEtiqueta = new ConsultarExpedientesPorEtiquetaUseCase(repoExpediente, repoTramite);
// 2. MENÚ DE USUARIO
bool salir = false;
while (!salir)
{   
    Console.WriteLine("\n====================================");
    Console.WriteLine("   SGE: GESTIÓN DE EXPEDIENTES");
    Console.WriteLine("====================================");
    Console.WriteLine("1. Dar de alta un expediente");
    Console.WriteLine("2. Dar de alta un trámite");
    Console.WriteLine("3. Listar expedientes");
    Console.WriteLine("4. Dar de baja un Expediente (Cascada)");
    Console.WriteLine("5. Modificar un Expediente");
    Console.WriteLine("6. Modificar un trámite");
    Console.WriteLine("7. Listar trámites de un expediente");
    Console.WriteLine("8. Cambiar estado de expediente (Manual)");
    Console.WriteLine("9. Informe: Expedientes por estado");
    Console.WriteLine("10. Informe: Expedientes con trámites de tipo...");
    Console.WriteLine("0. Salir");
    Console.Write("Seleccione una opción: ");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Write("Ingrese la carátula del expediente: ");
            string? caratula = Console.ReadLine();
            try {
                altaExpediente.Ejecutar(caratula ?? "", Guid.NewGuid());
                Console.WriteLine("¡Expediente creado con éxito!");
            } catch (Exception e) { Console.WriteLine($"Error: {e.Message}"); }
            break;

        case "2":
           Console.Write("Ingrese el ID del expediente (GUID): ");
           if (Guid.TryParse(Console.ReadLine(), out Guid expId)) {
               Console.Write("Ingrese el contenido del trámite: ");
               string contenidoStr = Console.ReadLine() ?? "";
               try {
                  altaTramite.Ejecutar(expId, contenidoStr, EtiquetaTramite.PaseAEstudio, Guid.NewGuid());
                  Console.WriteLine("¡Trámite cargado y estado de expediente actualizado!");
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
             Console.WriteLine("\n--- BAJA DE EXPEDIENTE (CASCADA) ---");
             Console.Write("Ingrese el ID del expediente a eliminar: ");
             string? idInput = Console.ReadLine();
    
             if (Guid.TryParse(idInput, out Guid idParaBorrar))
             {
                try 
                {
                    Guid usuarioIdActivo = Guid.NewGuid(); 
                    bajaExpedienteUseCase.Ejecutar(idParaBorrar, usuarioIdActivo);
                    Console.WriteLine("✅ Expediente y sus trámites asociados eliminados correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}"); // Error corregido: agregada la $
                }
             }
             else
             {
                  Console.WriteLine("⚠️ ID inválido. Debe ser un formato Guid.");
             }
             break;


        case "5":
                   Console.Write("Ingrese el ID del expediente a modificar: ");
                   if (Guid.TryParse(Console.ReadLine(), out Guid idMod)) {
                        Console.Write("Ingrese la nueva carátula: ");
                        string nuevaC = Console.ReadLine() ?? "";
                      try {
                           modificarExpediente.Ejecutar(idMod, nuevaC, Guid.NewGuid());
                           Console.WriteLine("✅ Expediente modificado con éxito.");
                    } catch (Exception e) { Console.WriteLine($"❌ Error: {e.Message}"); }
    }
                    break;

        case "6":
                Console.Write("Ingrese el ID del trámite a modificar: ");
                if (Guid.TryParse(Console.ReadLine(), out Guid idTramite))
                {
                    Console.Write("Ingrese el nuevo contenido: ");
                    string nuevoContenido = Console.ReadLine() ?? "";
                    Console.Write("Ingrese la nueva etiqueta (0=EscritoPresentado, 1=PaseAEstudio, 2=Despacho, 3=Resolucion, 4=Notificacion, 5=PaseAlArchivo): ");
                    if (Enum.TryParse<EtiquetaTramite>(Console.ReadLine(), out EtiquetaTramite etiqueta))
                    {
                        try {
                            modificarTramite.Ejecutar(idTramite, etiqueta, new ContenidoTramite(nuevoContenido), Guid.NewGuid());
                            Console.WriteLine("✅ Trámite modificado con éxito.");
                        } catch (Exception e) { Console.WriteLine($"❌ Error: {e.Message}"); }
                    }
                    else Console.WriteLine("⚠️ Etiqueta no válida.");
                }
                else Console.WriteLine("⚠️ ID inválido.");
        break;

        case "7":
            Console.Write("Ingrese el ID del expediente: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid idExpLista))
            {
                try {
                    var tramites = listarTramites.Ejecutar(idExpLista);
                    foreach (var t in tramites)
                        Console.WriteLine($"ID: {t.Id} | Etiqueta: {t.Etiqueta} | Contenido: {t.Contenido}");
                } catch (Exception e) { Console.WriteLine($"❌ Error: {e.Message}"); }
            }
            else Console.WriteLine("⚠️ ID inválido.");
        break;

        case "8":
            Console.Write("Ingrese el ID del expediente: ");
             if (Guid.TryParse(Console.ReadLine(), out Guid idExp)) 
            {
             // Mostramos los estados disponibles para ayudar al usuario
            Console.WriteLine("Estados: 0= RecienIniciado, 1= ParaResolver, 2= ConResolucion, 3= EnNotificacion, 4= Finalizado");
            Console.Write("Seleccione el número del nuevo estado: ");
        
            if (Enum.TryParse<EstadoExpediente>(Console.ReadLine(), out EstadoExpediente nuevoEstado)) 
            {
                try {
                // Ejecutamos el caso de uso
                    cambiarEstadoManual.Ejecutar(idExp, nuevoEstado, Guid.NewGuid());
                    Console.WriteLine("✅ Estado actualizado correctamente.");
                } 
                catch (Exception ex) { Console.WriteLine($"❌ Error: {ex.Message}"); }
            } 
            else Console.WriteLine("⚠️ Estado no válido.");
            } 
            else Console.WriteLine("⚠️ ID de expediente inválido.");
        break;
        
        case "9":
             Console.WriteLine("\n--- INFORME: EXPEDIENTES POR ESTADO ---");
             Console.WriteLine("0: RecienIniciado, 1: ParaResolver, 2: ConResolucion, 3: EnNotificacion, 4: Finalizado");
             Console.Write("Seleccione el estado: ");
             if (Enum.TryParse<EstadoExpediente>(Console.ReadLine(), out EstadoExpediente estFiltro))
             {
                var filtrados = listarPorEstado.Ejecutar(estFiltro);
                Console.WriteLine($"\nResultados ({estFiltro}):");
                foreach (var e in filtrados) Console.WriteLine($"- {e.Caratula} (ID: {e.Id})");
                if (!filtrados.Any()) Console.WriteLine("No se encontraron resultados.");
    }
    break;

        case "10":
            Console.WriteLine("\n--- INFORME: EXPEDIENTES POR TIPO DE TRÁMITE ---");
            Console.WriteLine("0: Escrito, 1: PaseAEstudio, 2: Despacho, 3: Resolucion, 4: Notificacion, 5: Archivo");
            Console.Write("Seleccione la etiqueta de trámite: ");
            if (Enum.TryParse<EtiquetaTramite>(Console.ReadLine(), out EtiquetaTramite etiFiltro))
            {
                var filtrados = consultarPorEtiqueta.Ejecutar(etiFiltro);
                Console.WriteLine($"\nExpedientes que tienen trámites de tipo [{etiFiltro}]:");
                foreach (var e in filtrados) Console.WriteLine($"- {e.Caratula} (ID: {e.Id})");
                 if (!filtrados.Any()) Console.WriteLine("No se encontraron expedientes con esos trámites.");
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