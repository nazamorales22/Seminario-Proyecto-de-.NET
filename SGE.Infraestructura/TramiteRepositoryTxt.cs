using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Dominio.Comun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SGE.Infraestructura;

public class TramiteRepositoryTxt : ITramiteRepository
{
    private const string Archivo = "tramites.txt";

    public void Agregar(Tramite tramite)
    {
        using var sw = new StreamWriter(Archivo, true);
        sw.WriteLine($"{tramite.Id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido}|{tramite.UsuarioUltimoCambio}|{tramite.FechaCreacion}");
    }

    public void Eliminar(Guid id)
    {
        if (!File.Exists(Archivo)) return;
        var lineas = File.ReadAllLines(Archivo)
            .Where(l => Guid.Parse(l.Split('|')[0]) != id)
            .ToList();
        File.WriteAllLines(Archivo, lineas);
    }

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        if (!File.Exists(Archivo)) return Enumerable.Empty<Tramite>();
        
        return File.ReadAllLines(Archivo)
            .Select(linea => linea.Split('|'))
            .Where(d => d.Length >= 6 && Guid.Parse(d[1]) == expedienteId) 
            .Select(d => {
                return Tramite.Reconstruir(
                    Guid.Parse(d[0]), 
                    Guid.Parse(d[1]), 
                    Enum.Parse<EtiquetaTramite>(d[2]), 
                    new ContenidoTramite(d[3]),
                    Guid.Parse(d[4]), 
                    DateTime.Parse(d[5]) 
                );
            })
            .ToList();
    }

    public void EliminarRelacionadosA(Guid expedienteId)
    {
        if (!File.Exists(Archivo)) return;
        var lineasRestantes = File.ReadAllLines(Archivo)
            .Where(l => {
                var datos = l.Split('|');
                return Guid.Parse(datos[1]) != expedienteId;
            })
            .ToList();
        File.WriteAllLines(Archivo, lineasRestantes);
    }
}