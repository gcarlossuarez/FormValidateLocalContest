# Cambios Implementados - Funcionalidad de Datasets y Pruebas de Consola

## Resumen Ejecutivo

Se ha implementado la funcionalidad de **visualizar y seleccionar archivos `datos*.txt`** de cada problema, permitiendo que el usuario cargue el contenido en un área editable de entrada (stdin). Adicionalmente, se agregó un **botón de prueba rápida** que ejecuta el programa compilado con el contenido del stdin, mostrando el resultado en un área de solo lectura, permitiendo pruebas interactivas antes de la evaluación oficial.

---

## Cambios Realizados

### 1. **FormMain.Designer.cs** - Interfaz Gráfica

Se agregaron cuatro nuevos controles para facilitar las pruebas interactivas:

#### Controles Agregados:
- **`lblDatasets`**: Etiqueta que identifica el ComboBox de archivos de datos
- **`cbDatasets`**: ComboBox que lista todos los archivos `datos*.txt` disponibles en el problema
- **`lblStdin`**: Etiqueta para el área de entrada
- **`txtStdin`**: TextBox editable que muestra el contenido del archivo `datos*.txt` seleccionado
- **`lblStdout`**: Etiqueta para el área de salida
- **`txtStdout`**: TextBox de solo lectura que muestra la salida del programa
- **`btnRunConsole`**: Botón que ejecuta el programa de forma interactiva

#### Posicionamiento:
- El ComboBox de datasets se encuentra a la izquierda, bajo el selector de problemas
- El área de stdin (entrada) está en la esquina inferior izquierda
- El área de stdout (salida) está en la esquina inferior derecha
- El botón "Ejecutar Consola" está posicionado junto al botón "Compilar y Ejecutar"

---

### 2. **FormMain.cs** - Lógica de Negocios

#### a) **Nuevo Campo de Instancia**
```csharp
private string? selectedDataFile;
```
Almacena la ruta completa del archivo `datos*.txt` actualmente seleccionado.

#### b) **Método: `LoadAvailableDatasets(string problemPath)`**
**Responsabilidad**: Explorar el directorio `IN/` del problema y cargar la lista de archivos `datos*.txt` en el ComboBox.

**Proceso**:
1. Limpia el ComboBox y variables asociadas
2. Verifica la existencia del directorio `IN/`
3. Busca todos los archivos que coincidan con el patrón `datos*.txt`
4. Ordena los archivos alfabéticamente
5. Los agrega al ComboBox
6. Si no hay archivos, muestra un mensaje y deshabilita el ComboBox
7. Maneja excepciones y las muestra en el ComboBox

#### c) **Método: `cbDatasets_SelectedIndexChanged(object sender, EventArgs e)`**
**Responsabilidad**: Cuando el usuario selecciona un archivo del ComboBox, carga su contenido en `txtStdin`.

**Proceso**:
1. Valida que haya un problema seleccionado
2. Obtiene el nombre del archivo seleccionado
3. Construye la ruta completa del archivo
4. Lee el contenido como UTF-8 sin BOM
5. Muestra el contenido en `txtStdin` (editable)
6. Actualiza la barra de estado con el nombre del dataset
7. Maneja errores de lectura

#### d) **Método: `btnRunConsole_Click(object sender, EventArgs e)`**
**Responsabilidad**: Ejecutar el programa compilado con el contenido del `txtStdin` como entrada estándar, mostrando el resultado en `txtStdout`.

**Proceso detallado**:
1. Valida que existan un problema y código seleccionado
2. Busca el archivo `.csproj` del problema
3. Crea un directorio de trabajo temporal
4. Copia el `.csproj` al directorio de trabajo
5. Guarda el código del usuario en `Program.cs`
6. Compila el proyecto usando `dotnet build` (timeout 5 minutos)
7. Si hay error de compilación, muestra el error en `txtStdout`
8. Busca el ejecutable compilado
9. Guarda el contenido del `txtStdin` en un archivo temporal
10. Ejecuta el programa con el archivo temporal como entrada estándar
11. Captura la salida estándar y de error
12. Muestra el resultado en `txtStdout` con indicadores visuales:
    - ✅ Ejecución exitosa
    - ❌ Error con código de salida
    - ⏱️ Timeout
13. Limpia archivos temporales
14. Actualiza la barra de estado

**Características especiales**:
- Desactiva botones durante la ejecución
- Usa UTF-8 para entrada/salida
- Timeout de 30 segundos para la ejecución
- Soporte para código de salida no cero
- Limpieza automática de recursos

#### e) **Métodos Existentes Modificados**

**`lstProblems_SelectedIndexChanged()`**:
- Ahora llama a `LoadAvailableDatasets(problemPath)` después de seleccionar un problema
- Automáticamente popula el ComboBox con los archivos disponibles

**`CompileAndRunTests()`**:
- Se removió la lógica experimental de `testDataDir`
- Mantiene la funcionalidad original de evaluar todos los datasets

---

## Flujo de Ejecución

### Secuencia: Seleccionar Problema → Seleccionar Dataset → Ejecutar Prueba

```
1. Usuario selecciona un problema en lstProblems
   ↓
   lstProblems_SelectedIndexChanged()
   ├─ Actualiza txtProblemaDir.Text
   ├─ Guarda en problemaDirectory
   ├─ LoadAvailableDatasets(problemPath)  ← NUEVO
   │   ├─ Explora IN/
   │   ├─ Busca datos*.txt
   │   └─ Popula cbDatasets
   └─ Muestra descripción (.docx)

2. Usuario selecciona un archivo datos*.txt en cbDatasets
   ↓
   cbDatasets_SelectedIndexChanged()  ← NUEVO
   ├─ Lee contenido del archivo
   ├─ Muestra en txtStdin (editable)
   └─ Actualiza barra de estado

3. Usuario (opcionalmente) edita el contenido en txtStdin

4. Usuario hace clic en "Ejecutar Consola"
   ↓
   btnRunConsole_Click()  ← NUEVO
   ├─ Compila el código del usuario
   ├─ Busca el ejecutable
   ├─ Ejecuta con txtStdin como entrada
   ├─ Captura salida
   └─ Muestra resultado en txtStdout (solo lectura)

5. Usuario revisa resultado y puede:
   - Seleccionar otro dataset
   - Modificar txtStdin y ejecutar nuevamente
   - Ir a "Compilar y Ejecutar" para evaluación oficial
```

---

## Casos de Uso

### Caso 1: Prueba Rápida Interactiva
1. Usuario selecciona un problema (ej: "Suma")
2. ComboBox se llena automáticamente: `datos1.txt`, `datos2.txt`, `datos3.txt`
3. Usuario selecciona `datos1.txt`
4. El contenido (ej: "5\n3") aparece en txtStdin
5. Usuario hace clic en "Ejecutar Consola"
6. El programa se ejecuta y muestra "8" en txtStdout
7. Usuario verifica que es correcto

### Caso 2: Prueba con Datos Personalizados
1. Usuario carga un problema
2. Selecciona un dataset
3. Modifica el contenido en txtStdin (ej: cambia "5\n3" a "10\n20")
4. Hace clic en "Ejecutar Consola"
5. Obtiene resultado con los datos modificados
6. Repite hasta estar satisfecho

### Caso 3: Evaluación Oficial
1. Tras realizar pruebas interactivas satisfactorias
2. Usuario hace clic en "Compilar y Ejecutar"
3. El sistema evalúa el código contra TODOS los datos (datos1.txt, datos2.txt, etc.)
4. Obtiene calificación oficial

---

## Consideraciones Técnicas

### Compilación y Ejecución
- La compilación se realiza en un directorio de trabajo temporal para evitar bloqueos
- El timeout de compilación es de 5 minutos (300 segundos)
- El timeout de ejecución es de 30 segundos
- Soporta UTF-8 sin BOM para entrada/salida
- Limpieza automática de directorios temporales

### Seguridad
- Los directorios de trabajo se crean con nombres únicos usando GUID
- Se intenta limpiar automáticamente
- Los procesos se ejecutan con baja prioridad

### Manejo de Errores
- Errores de compilación se muestran en txtStdout
- Errores de ejecución (timeout, código no cero) se muestran con indicadores visuales
- Mensajes descriptivos guían al usuario

### Compatibilidad
- ✅ Compatible con problemas sin datasets (muestra "No hay archivos datos*.txt")
- ✅ Compatible con múltiples archivos de datos
- ✅ Compatible con validadores personalizados (ValidatorX)
- ✅ Mantiene compatibilidad con "Compilar y Ejecutar" original

---

## Interfaz de Usuario

### Antes (Original)
```
┌─────────────────────┬──────────────────────────┐
│ Código C#           │ Resultados de Pruebas    │
│ (500px)             │ (400px)                  │
│                     │                          │
└─────────────────────┴──────────────────────────┘
┌──────────────────┬──────────────────────────────┐
│ Problemas        │ Botones (Compilar, Cancelar)│
│ (200px)          │                              │
└──────────────────┴──────────────────────────────┘
```

### Después (Con Datasets y Consola)
```
┌─────────────────────┬──────────────────────────┐
│ Código C#           │ Resultados de Pruebas    │
│ (500px)             │ (400px)                  │
│                     │                          │
├─────────────────────┼──────────────────────────┤
│ Entrada (stdin)     │ Salida (stdout)          │
│ (editable, 100px)   │ (solo lectura, 100px)    │
├─────────────────────┼──────────────────────────┤
│ Datasets: [▼]       │ [Ejecutar Consola]       │
│ Problemas: [▼]      │ [Compilar y Ejecutar]    │
└─────────────────────┴──────────────────────────┘
```

---

## Estado de Compilación

✅ **Compilación exitosa**
```
dotnet build
→ 0 Warnings
→ 0 Errors
→ Tiempo: ~4 segundos
```

---

## Archivos Modificados

| Archivo | Cambios |
|---------|---------|
| `FormMain.Designer.cs` | +7 controles, inicialización, event handlers |
| `FormMain.cs` | +3 métodos nuevos, 1 campo nuevo, 1 modificación |

---

## Conclusión

La funcionalidad implementada proporciona una experiencia interactiva y pedagogía para que los usuarios prueben su código antes de la evaluación oficial. El diseño mantiene separación clara entre:
- **Pruebas rápidas** (Ejecutar Consola): Interactivas, iterativas
- **Evaluación oficial** (Compilar y Ejecutar): Automática, calificada

Esto facilita el proceso de debugging y mejora la experiencia del desarrollador.
