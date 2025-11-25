using System;
using System.Linq;
using System.Text;

// Programa para depurar exactamente qué está recibiendo
Console.Error.WriteLine("=== DEBUG: INICIO ===");
Console.Error.WriteLine($"Encoding Console.InputEncoding: {Console.InputEncoding.EncodingName}");
Console.Error.WriteLine($"Encoding Console.OutputEncoding: {Console.OutputEncoding.EncodingName}");

string? line1 = Console.ReadLine();

if (line1 != null)
{
    Console.Error.WriteLine($"Línea recibida: '{line1}'");
    Console.Error.WriteLine($"Longitud: {line1.Length}");
    Console.Error.WriteLine($"Bytes: {string.Join(" ", line1.Select(c => ((int)c).ToString("X2")))}");
    Console.Error.WriteLine($"Chars: {string.Join(" ", line1.Select(c => $"'{c}'"))}");
    
    var parts = line1.Split(' ');
    Console.Error.WriteLine($"Después de Split: {parts.Length} partes");
    for (int i = 0; i < parts.Length; i++)
    {
        Console.Error.WriteLine($"  Parte[{i}]: '{parts[i]}' (bytes: {string.Join(" ", parts[i].Select(c => ((int)c).ToString("X2")))})");
    }
    
    // Intentar parsear
    if (parts.Length >= 3)
    {
        Console.Error.WriteLine($"Intentando parsear: '{parts[0]}', '{parts[1]}', '{parts[2]}'");
        
        bool ok1 = int.TryParse(parts[0], out int n1);
        bool ok2 = int.TryParse(parts[1], out int n2);
        bool ok3 = int.TryParse(parts[2], out int n3);
        
        Console.Error.WriteLine($"TryParse resultados: {ok1} ({n1}), {ok2} ({n2}), {ok3} ({n3})");
    }
}
else
{
    Console.Error.WriteLine("No se recibió ninguna línea");
}

Console.Error.WriteLine("=== DEBUG: FIN ===");
Console.WriteLine("OK");
