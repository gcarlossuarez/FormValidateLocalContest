# Validador de Programas C# - FormValidateLocalContest

Aplicación Windows Forms para validar programas C# contra casos de prueba de forma automática.

## Características

- **Selección de archivo C#**: Carga cualquier archivo .cs en el editor integrado
- **Editor de código integrado**: TextBox editable con fuente monoespaciada para modificar el código
- **Compilación en directorio aislado**: Evita conflictos con múltiples archivos Main
- **Ejecución directa del .exe**: Compila una vez y ejecuta el binario (más rápido)
- **Visualización en tiempo real**: Panel de resultados que se actualiza durante la ejecución
- **Botón de cancelación**: Detiene la ejecución en cualquier momento
- **Validación automática**: Compara las salidas generadas con las esperadas
- **Timeout configurable**: Límite de tiempo por defecto de 5 segundos (modificable)
- **Codificación UTF-8**: Manejo correcto de caracteres especiales

## Estructura de Directorios de Problema

Cada problema debe tener la siguiente estructura:

```
Problema/
├── problema.csproj          (Archivo de proyecto .NET)
├── IN/                      (Carpeta con archivos de entrada)
│   ├── datos1.txt
│   ├── datos2.txt
│   └── datos3.txt
├── OUT/                     (Carpeta con salidas esperadas)
│   ├── Output_datos1.txt
│   ├── Output_datos2.txt
│   └── Output_datos3.txt
└── OutputGenerado/          (Se crea/limpia automáticamente)
    └── [archivos generados durante la ejecución]
```

### Archivos importantes:

1. **problema.csproj**: Archivo de proyecto de consola .NET que se usa como plantilla
2. **IN/datos<n>.txt**: Archivos de entrada numerados secuencialmente
3. **OUT/Output_datos<n>.txt**: Salidas esperadas correspondientes a cada entrada
4. **Program.cs**: Se genera automáticamente con el código del TextBox al compilar

## Uso

1. **Seleccionar Directorio del Problema**:
   - Click en "Seleccionar Directorio"
   - Navega hasta el directorio que contiene la estructura del problema
   - El directorio debe tener las carpetas IN, OUT y un archivo .csproj

2. **Cargar el programa C#**:
   - Click en "Seleccionar Archivo C#"
   - Selecciona el archivo .cs que quieres validar
   - El código se cargará en el TextBox y podrás editarlo

3. **Configurar Timeout** (opcional):
   - El valor por defecto es 5 segundos
   - Modifica el valor en el campo "Timeout (seg)" si es necesario

4. **Compilar y Ejecutar**:
   - Click en "Compilar y Ejecutar"
   - La aplicación:
     - Guarda el código del TextBox como Program.cs en el directorio del problema
     - Compila el proyecto con `dotnet build`
     - Si la compilación falla, muestra los errores
     - Si compila correctamente, ejecuta cada caso de prueba
     - Compara las salidas con las esperadas
     - Muestra el resultado de cada test en un cuadro de diálogo

5. **Interpretar Resultados**:
   - ✅ **CORRECTO**: La salida coincide exactamente con la esperada
   - ❌ **ERRÓNEO**: La salida no coincide (revisa OutputGenerado para ver la diferencia)
   - ⏱️ **TIMEOUT**: El programa excedió el tiempo límite
   - 💥 **EXCEPCIÓN**: El programa lanzó una excepción durante la ejecución
   - ⚠️ **Advertencia**: Falta el archivo de salida esperado

## Ejemplo de Problema

Se incluye un problema de ejemplo en la carpeta `EjemploProblema`:

**Descripción**: Lee dos líneas con dos números cada una y muestra la suma de cada par.

**Entrada de ejemplo** (datos1.txt):
```
5 3
10 20
```

**Salida esperada** (Output_datos1.txt):
```
8
30
```

**Programa de ejemplo** (ProgramaEjemplo.cs):
```csharp
string? line1 = Console.ReadLine();
string? line2 = Console.ReadLine();

if (line1 != null && line2 != null)
{
    var nums1 = line1.Split(' ').Select(int.Parse).ToArray();
    var nums2 = line2.Split(' ').Select(int.Parse).ToArray();
    
    Console.WriteLine(nums1[0] + nums1[1]);
    Console.WriteLine(nums2[0] + nums2[1]);
}
```

## Requisitos

- .NET SDK instalado (versión 6.0 o superior)
- Windows con PowerShell
- Visual Studio 2022 o superior (opcional, para desarrollo)

## Ejecución

### Desde Visual Studio:
1. Abre la solución en Visual Studio
2. Presiona F5 para compilar y ejecutar

### Desde línea de comandos:
```powershell
cd "d:\Proyectos Visual Studio\VSCode\FormValidateLocalContest\FormValidateLocalContest"
dotnet run
```

## Notas Técnicas

- La aplicación utiliza codificación UTF-8 para todos los archivos
- Las comparaciones de salida normalizan espacios al final de línea y líneas vacías finales
- El timeout se aplica individualmente a cada test
- La carpeta OutputGenerado se limpia automáticamente en cada ejecución
- Los mensajes de error de compilación y ejecución se muestran completos

## Solución de Problemas

**Error: "No se encontró el directorio IN"**
- Verifica que el directorio del problema contenga una carpeta llamada exactamente "IN"

**Error: "No se encontró un archivo .csproj"**
- Asegúrate de que existe un archivo .csproj en la raíz del directorio del problema

**Timeout constante**
- Aumenta el valor de timeout en el campo correspondiente
- Revisa que el programa no tenga bucles infinitos

**Comparación siempre falla**
- Revisa el directorio OutputGenerado para ver la salida real
- Verifica que los archivos esperados estén en UTF-8
- Comprueba que no haya espacios extras al final de las líneas
