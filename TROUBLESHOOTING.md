# Solución de Problemas Comunes

## Error: "The input string was not in a correct format"

Este error ocurre cuando el programa intenta parsear un valor que no es válido.

### Causas Comunes:

1. **Falta de `using` statements**
   - Asegúrate de incluir `using System;` y `using System.Linq;` al inicio del archivo
   - Estos son necesarios para usar `int.Parse()`, `Console.ReadLine()`, etc.

2. **Espacios o caracteres inesperados en la entrada**
   - Usa `.Trim()` antes de parsear: `int.Parse(s.Trim())`
   - Usa `StringSplitOptions.RemoveEmptyEntries` al hacer split

3. **Archivos de entrada con formato incorrecto**
   - Verifica que los archivos `datos*.txt` no tengan comillas o caracteres extraños
   - Los archivos deben estar en UTF-8
   - No debe haber espacios adicionales al final de las líneas

### Ejemplo de Código Robusto:

```csharp
using System;
using System.Linq;

try
{
    string? line = Console.ReadLine();
    if (line != null)
    {
        // Split y eliminar entradas vacías
        var numbers = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => int.Parse(s.Trim()))
                         .ToArray();
        
        Console.WriteLine(numbers[0] + numbers[1]);
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}
```

## Error: "No se encontró el ejecutable"

### Solución:
- Asegúrate de que el archivo `.csproj` existe en el directorio del problema
- Verifica que la compilación fue exitosa antes de ejecutar los tests

## Error: "Timeout"

### Solución:
- Incrementa el valor de timeout en el formulario
- Verifica que tu programa no tenga bucles infinitos
- Asegúrate de que tu programa lee todas las líneas necesarias

## El programa compila pero no produce salida

### Verificar:
1. ¿Tu programa lee de `Console.ReadLine()`?
2. ¿Tu programa escribe con `Console.WriteLine()`?
3. ¿El programa termina normalmente sin quedarse esperando más entrada?

## Depuración de Entrada/Salida

Para ver exactamente qué está recibiendo tu programa:

```csharp
using System;

// Imprimir la entrada tal como se recibe
string? line = Console.ReadLine();
Console.Error.WriteLine($"DEBUG: Recibido '{line}' (longitud: {line?.Length})");

// Procesar...
```

Los mensajes escritos con `Console.Error.WriteLine()` aparecerán en el panel de resultados como errores.
