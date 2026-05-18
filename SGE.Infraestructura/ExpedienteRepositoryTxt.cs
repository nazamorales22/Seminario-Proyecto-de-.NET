using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Comun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SGE.Infraestructura;

public class ExpedienteRepositoryTxt : IExpedienteRepository
{
    private const string Archivo = "expedientes.txt";

    public void Agregar(Expediente expediente)
    {
        using var sw = new StreamWriter(Archivo, true);
        sw.WriteLine($"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.UsuarioUltimoCambio}|{expediente.Estado}|{expediente.FechaCreacion}|{expediente.FechaUltimaModificacion}");
    }

    public void Modificar(Expediente expediente)
    {
        if (!File.Exists(Archivo)) return;
        var lineas = File.ReadAllLines(Archivo).ToList();
        var nuevaLista = lineas.Select(linea => {
            var datos = linea.Split('|');
            if (datos.Length > 0 && Guid.Parse(datos[0]) == expediente.Id)
                return $"{expediente.Id}|{expediente.Caratula.Valor}|{expediente.UsuarioUltimoCambio}|{expediente.Estado}|{expediente.FechaCreacion}|{expediente.FechaUltimaModificacion}";
            return linea;
        }).ToList();
        File.WriteAllLines(Archivo, nuevaLista);
    }

    public void Eliminar(Guid id)
    {
        if (!File.Exists(Archivo)) return;
        var lineasRestantes = File.ReadAllLines(Archivo)
            .Where(l => {
                var datos = l.Split('|');
                return datos.Length > 0 && Guid.Parse(datos[0]) != id;
            })
            .ToList();
        File.WriteAllLines(Archivo, lineasRestantes);
    }

    public Expediente? ObtenerPorId(Guid id) => ObtenerTodos().FirstOrDefault(e => e.Id == id);

    public IEnumerable<Expediente> ObtenerTodos()
    {
        if (!File.Exists(Archivo)) return Enumerable.Empty<Expediente>();
        
        return File.ReadAllLines(Archivo).Select(linea => {
            var datos = linea.Split('|');
            return Expediente.Reconstruir(
                Guid.Parse(datos[0]), 
                new Caratula(datos[1]), 
                Guid.Parse(datos[2]),
                Enum.Parse<EstadoExpediente>(datos[3]),
                DateTime.Parse(datos[4]),
                DateTime.Parse(datos[5]) 
            );
        }).ToList();
    }
}