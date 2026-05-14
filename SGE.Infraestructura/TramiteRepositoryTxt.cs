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
        sw.WriteLine($"{tramite.Id}|{tramite.ExpedienteId}|{tramite.Etiqueta}|{tramite.Contenido}|{tramite.UsuarioUltimoCambio}|{tramite.FechaCreacion}|{tramite.FechaUltimaModificacion}");
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
            .Where(d => d.Length >= 7 && Guid.Parse(d[1]) == expedienteId) // 7 en vez de 6
            .Select(d => {
                return Tramite.Reconstruir(
                    Guid.Parse(d[0]),
                    Guid.Parse(d[1]),
                    Enum.Parse<EtiquetaTramite>(d[2]),
                    new ContenidoTramite(d[3]),
                    Guid.Parse(d[4]),
                    DateTime.Parse(d[5]),
                    DateTime.Parse(d[6])  // ← FechaUltimaModificacion
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

    public Tramite? ObtenerPorId(Guid id)
    {
        if (!File.Exists(Archivo)) return null;
        return File.ReadAllLines(Archivo)
            .Select(l => l.Split('|'))
            .Where(d => d.Length >= 7 && Guid.Parse(d[0]) == id)
            .Select(d => Tramite.Reconstruir(
                Guid.Parse(d[0]),
                Guid.Parse(d[1]),
                Enum.Parse<EtiquetaTramite>(d[2]),
                new ContenidoTramite(d[3]),
                Guid.Parse(d[4]),
                DateTime.Parse(d[5]),
                DateTime.Parse(d[6])
            ))
            .FirstOrDefault();
    }

    public IEnumerable<Tramite> ObtenerTodos()
    {
        if (!File.Exists(Archivo)) return Enumerable.Empty<Tramite>();
        return File.ReadAllLines(Archivo)
            .Select(l => l.Split('|'))
            .Where(d => d.Length >= 7)
            .Select(d => Tramite.Reconstruir(
                Guid.Parse(d[0]),
                Guid.Parse(d[1]),
                Enum.Parse<EtiquetaTramite>(d[2]),
                new ContenidoTramite(d[3]),
                Guid.Parse(d[4]),
                DateTime.Parse(d[5]),
                DateTime.Parse(d[6])
            ));
    }


    public void Modificar(Tramite tramite)
    {
        var tramites = ObtenerTodos().ToList();
        var index = tramites.FindIndex(t => t.Id == tramite.Id);

        if (index == -1)
            throw new RepositorioException("No se encontró el trámite a modificar.");

        tramites[index] = tramite;
        File.WriteAllLines(Archivo, tramites.Select(t => 
            $"{t.Id}|{t.ExpedienteId}|{t.Etiqueta}|{t.Contenido}|{t.UsuarioUltimoCambio}|{t.FechaCreacion}|{t.FechaUltimaModificacion}"));
    }

}