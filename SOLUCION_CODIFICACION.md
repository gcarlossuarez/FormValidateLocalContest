# Problema de Codificación Resuelto

## El Problema

Al ejecutar programas C# compilados y pasar entrada estándar, se encontraba el error:
```
The input string '5' was not in a correct format
```

Las comillas simples `'5'` en el error son parte del mensaje de C#, no del contenido real.

## Causa Raíz

El problema estaba en **forzar UTF-8** en `ProcessStartInfo`:

```csharp
StartInfo = new ProcessStartInfo
{
    StandardOutputEncoding = Encoding.UTF8,    // ❌ ESTO CAUSABA EL PROBLEMA
    StandardErrorEncoding = Encoding.UTF8,     // ❌ ESTO CAUSABA EL PROBLEMA
    StandardInputEncoding = Encoding.UTF8      // ❌ ESTO CAUSABA EL PROBLEMA
}
```

## Por Qué Ocurría

1. Windows usa por defecto la codificación del sistema (generalmente Windows-1252 o la página de códigos local)
2. Los ejecutables .NET compilados esperan la codificación predeterminada del sistema
3. Al forzar UTF-8 en la comunicación entre procesos, se creaba una incompatibilidad
4. Los caracteres se interpretaban incorrectamente, causando que números simples no se pudieran parsear

## Solución

**NO especificar la codificación** en `ProcessStartInfo`, dejando que .NET use la predeterminada del sistema:

```csharp
StartInfo = new ProcessStartInfo
{
    FileName = exePath,
    Arguments = "",
    WorkingDirectory = workingDirectory,
    RedirectStandardInput = true,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    UseShellExecute = false,
    CreateNoWindow = true
    // ✅ NO especificar codificación - usar la predeterminada del sistema
}
```

## Cuándo Usar UTF-8

UTF-8 es apropiado para:
- **Leer/escribir archivos**: `File.ReadAllTextAsync(path, Encoding.UTF8)`
- **APIs web y servicios REST**
- **Guardar configuraciones**

Pero NO para:
- **Comunicación entre procesos locales** (stdin/stdout/stderr)
- **Llamadas a ejecutables nativos de Windows**

## Diferencias Observadas

### ❌ Con UTF-8 Forzado:
```
Input recibido: caracteres corruptos
int.Parse() falla con FormatException
```

### ✅ Sin Especificar Codificación:
```
Input recibido: "5 3"
int.Parse() funciona correctamente
```

## Mejoras Implementadas

1. **ListBox más grande**: 560px de ancho para ver mensajes de error completos
2. **HorizontalScrollbar**: Activado en el ListBox para mensajes largos
3. **Fuente más pequeña**: Consolas 8pt para ver más contenido
4. **Formulario más ancho**: 1000px para acomodar todos los controles

## Lección Aprendida

No siempre UTF-8 es la respuesta. En Windows, para comunicación entre procesos locales, es mejor dejar que el sistema use su codificación predeterminada. Esto asegura compatibilidad entre el proceso padre (tu aplicación) y los procesos hijos (los programas compilados que ejecutas).
