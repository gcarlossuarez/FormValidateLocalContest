using System;
using System.Linq;

// Programa de ejemplo que suma dos números
// Lee dos líneas de la entrada estándar y muestra la suma

try
{
    string? line1 = Console.ReadLine();
    string? line2 = Console.ReadLine();

    if (line1 == null || line2 == null)
    {
        Console.Error.WriteLine("Error: No se pudieron leer las líneas de entrada");
        Environment.Exit(1);
        return;
    }

    // Procesar primera línea
    var parts1 = line1.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
    if (parts1.Length < 2)
    {
        Console.Error.WriteLine($"Error: Línea 1 no tiene suficientes números. Recibido: '{line1}'");
        Environment.Exit(1);
        return;
    }

    // Procesar segunda línea
    var parts2 = line2.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
    if (parts2.Length < 2)
    {
        Console.Error.WriteLine($"Error: Línea 2 no tiene suficientes números. Recibido: '{line2}'");
        Environment.Exit(1);
        return;
    }

    // Parsear números
    int num1, num2, num3, num4;
    
    if (!int.TryParse(parts1[0].Trim(), out num1))
    {
        Console.Error.WriteLine($"Error: No se pudo parsear '{parts1[0]}' como entero");
        Environment.Exit(1);
        return;
    }
    if (!int.TryParse(parts1[1].Trim(), out num2))
    {
        Console.Error.WriteLine($"Error: No se pudo parsear '{parts1[1]}' como entero");
        Environment.Exit(1);
        return;
    }
    if (!int.TryParse(parts2[0].Trim(), out num3))
    {
        Console.Error.WriteLine($"Error: No se pudo parsear '{parts2[0]}' como entero");
        Environment.Exit(1);
        return;
    }
    if (!int.TryParse(parts2[1].Trim(), out num4))
    {
        Console.Error.WriteLine($"Error: No se pudo parsear '{parts2[1]}' como entero");
        Environment.Exit(1);
        return;
    }
    
    // Mostrar resultados
    Console.WriteLine(num1 + num2);
    Console.WriteLine(num3 + num4);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Excepción no manejada: {ex.GetType().Name}");
    Console.Error.WriteLine($"Mensaje: {ex.Message}");
    Environment.Exit(1);
}
