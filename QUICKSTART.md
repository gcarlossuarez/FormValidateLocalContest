# Guía Rápida de Uso

## Pasos para validar un programa:

1. **Ejecutar la aplicación**:
   ```powershell
   cd "d:\Proyectos Visual Studio\VSCode\FormValidateLocalContest\FormValidateLocalContest"
   dotnet run
   ```

2. **En la aplicación**:
   - Clic en "Seleccionar Directorio" → Selecciona `EjemploProblema`
   - Clic en "Seleccionar Archivo C#" → Selecciona `ProgramaEjemplo.cs`
   - (Opcional) Edita el código en el TextBox
   - Clic en "Compilar y Ejecutar"

3. **Ver resultados**:
   - Aparecerá un mensaje con los resultados de cada test
   - Las salidas generadas estarán en `EjemploProblema/OutputGenerado/`

## Estructura del Ejemplo Incluido

```
EjemploProblema/
├── problema.csproj                    # Proyecto .NET plantilla
├── ProgramaEjemplo.cs                 # Programa C# de ejemplo
├── IN/
│   ├── datos1.txt                    # Entrada: "5 3" y "10 20"
│   └── datos2.txt                    # Entrada: "2 8" y "15 25"
└── OUT/
    ├── Output_datos1.txt             # Salida esperada: "8" y "30"
    └── Output_datos2.txt             # Salida esperada: "10" y "40"
```

## Crear tu propio problema

1. Crea un directorio para tu problema, ejemplo: `MiProblema/`

2. Copia el archivo `problema.csproj` de EjemploProblema

3. Crea las carpetas:
   - `IN/` - Para archivos de entrada
   - `OUT/` - Para salidas esperadas

4. Agrega tus casos de prueba:
   - `IN/datos1.txt`, `IN/datos2.txt`, etc.
   - `OUT/Output_datos1.txt`, `OUT/Output_datos2.txt`, etc.

5. Escribe tu programa C# y valídalo con la aplicación

## Características Importantes

- ✅ Timeout configurable (default: 5 segundos)
- ✅ Manejo de caracteres especiales (UTF-8)
- ✅ Editor de código integrado y editable
- ✅ Compilación y ejecución automática
- ✅ Comparación exacta de salidas
- ✅ Reporte detallado de cada test

## Convenciones de Nombres

⚠️ **Importante**: Los nombres de archivos deben seguir este formato:

**Archivos de entrada**: `datos<número>.txt`
- ✅ Correcto: `datos1.txt`, `datos2.txt`, `datos10.txt`
- ❌ Incorrecto: `input1.txt`, `datos_1.txt`

**Archivos de salida**: `Output_datos<número>.txt`
- ✅ Correcto: `Output_datos1.txt`, `Output_datos2.txt`
- ❌ Incorrecto: `output1.txt`, `Out_datos1.txt`

El número debe coincidir entre entrada y salida esperada.
