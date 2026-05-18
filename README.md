# Sistema de Gestión de Expedientes (SGE)
## Documentación de Pruebas — Fase 1

---

## Cómo ejecutar el proyecto

```bash
cd SGE.Consola
dotnet run
```

---

## Camino Feliz

### 1. Alta de expediente
```
Seleccione una opción: 1
Ingrese la carátula del expediente: nuevoExpediente
¡Expediente creado con éxito!
```

### 2. Listar expedientes
```
Seleccione una opción: 3

--- LISTADO DE EXPEDIENTES ---
ID: e65c5c97-0880-4b2d-ae7f-96a2bcc95310 | Carátula: nuevoExpediente | Estado: RecienIniciado
```

### 3. Alta de trámite y cambio de estado automático
```
Seleccione una opción: 2
Ingrese el ID del expediente (GUID): e65c5c97-0880-4b2d-ae7f-96a2bcc95310
Ingrese el contenido del trámite: nuevoTramite
¡Trámite cargado y estado de expediente actualizado!
```

Al listar de nuevo, el estado cambió automáticamente a `ParaResolver`:
```
ID: e65c5c97-0880-4b2d-ae7f-96a2bcc95310 | Carátula: nuevoExpediente | Estado: ParaResolver
```

---

## Caminos de Error

### Carátula vacía → DominioException
```
Seleccione una opción: 1
Ingrese la carátula del expediente: (Enter sin escribir nada)
Error: La carátula no puede estar vacía.
```

### Sin permisos → AutorizacionException
Cambiando `AutorizacionProvisionalService` para que devuelva `false`:
```
Seleccione una opción: 1
Ingrese la carátula del expediente: test
Error: No tiene permiso para dar de alta un expediente.
```

### ID inválido
```
Seleccione una opción: 5
Ingrese el ID del expediente a eliminar: abc123
ID inválido. Debe ser un formato Guid.
```

---

## Reglas de negocio

| Etiqueta del último trámite | Estado resultante |
|-----------------------------|-------------------|
| `PaseAEstudio`              | `ParaResolver`    |
| `Resolucion`                | `ConResolucion`   |
| `PaseAlArchivo`             | `Finalizado`      |
| Sin trámites                | `RecienIniciado`  |
| Cualquier otra              | Sin cambio        |