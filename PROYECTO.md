# Validador de Programas C# - Aplicación Windows Forms

## ✅ Funcionalidades Implementadas

### Interfaz de Usuario
- [x] Botón para seleccionar archivo C# (.cs)
- [x] TextBox grande y editable con fuente monoespaciada para el código
- [x] Botón para seleccionar directorio del problema
- [x] TextBox para configurar timeout (default: 5 segundos)
- [x] Botón "Compilar y Ejecutar"
- [x] StatusStrip con barra de estado para mostrar progreso y resultados

### Funcionalidad Core
- [x] Cargar archivo C# con codificación UTF-8
- [x] Editar código directamente en el TextBox
- [x] Seleccionar directorio del problema con estructura IN/OUT/.csproj
- [x] Compilar código usando `dotnet build`
- [x] Ejecutar tests con entrada estándar: `type datos<n>.txt | dotnet run`
- [x] Crear/limpiar directorio OutputGenerado automáticamente
- [x] Guardar salidas generadas
- [x] Comparar salidas con archivos esperados (UTF-8)
- [x] Normalizar salidas para comparación (espacios y líneas vacías)
- [x] Timeout configurable por test
- [x] Manejo de errores de compilación
- [x] Manejo de errores de ejecución
- [x] Reporte detallado con iconos:
  - ✅ CORRECTO - Salida coincide
  - ❌ ERRÓNEO - Salida diferente
  - ⏱️ TIMEOUT - Excedió tiempo límite
  - 💥 EXCEPCIÓN - Error durante ejecución
  - ⚠️ Advertencia - Archivo esperado faltante

### Validación
- [x] Verificar existencia de directorio IN
- [x] Verificar existencia de directorio OUT
- [x] Verificar existencia de archivo .csproj
- [x] Verificar archivos datos*.txt en IN
- [x] Verificar archivos Output_datos*.txt correspondientes en OUT
- [x] Validar timeout como número entero positivo
- [x] Validar que hay código para compilar

## 📦 Estructura del Proyecto

```
FormValidateLocalContest/
├── FormValidateLocalContest/           # Proyecto principal
│   ├── Form1.cs                        # Lógica del formulario
│   ├── Form1.Designer.cs               # Diseño del formulario
│   ├── Program.cs                      # Punto de entrada
│   └── FormValidateLocalContest.csproj # Archivo de proyecto
│
├── EjemploProblema/                    # Problema de ejemplo
│   ├── problema.csproj                 # Plantilla de proyecto
│   ├── ProgramaEjemplo.cs              # Solución de ejemplo
│   ├── IN/                             # Casos de prueba entrada
│   │   ├── datos1.txt
│   │   └── datos2.txt
│   └── OUT/                            # Salidas esperadas
│       ├── Output_datos1.txt
│       └── Output_datos2.txt
│
├── README.md                           # Documentación completa
└── QUICKSTART.md                       # Guía rápida
```

## 🚀 Cómo Ejecutar

```powershell
# Opción 1: Desde el directorio del proyecto
cd "d:\Proyectos Visual Studio\VSCode\FormValidateLocalContest\FormValidateLocalContest"
dotnet run

# Opción 2: Compilar y ejecutar el ejecutable
dotnet build
.\bin\Debug\net10.0-windows\FormValidateLocalContest.exe
```

## 💡 Mejoras Futuras Sugeridas

### Funcionalidades Adicionales
- [ ] Soporte para múltiples problemas en una misma sesión
- [ ] Historial de ejecuciones
- [ ] Guardar configuraciones (último directorio usado, timeout preferido)
- [ ] Resaltado de sintaxis en el TextBox del código
- [ ] Diff visual entre salida esperada y generada
- [ ] Exportar resultados a archivo (CSV, JSON, HTML)
- [ ] Modo batch para validar múltiples soluciones
- [ ] Estadísticas: tiempo de ejecución por test, memoria usada
- [ ] Soporte para otros lenguajes (Python, Java, etc.)

### Mejoras de UI/UX
- [ ] Panel dividido para ver entrada/salida esperada/salida generada
- [ ] Colorear resultados (verde=correcto, rojo=error, amarillo=timeout)
- [ ] Progress bar durante la ejecución
- [ ] Lista detallada de tests con checkboxes para ejecutar selectivamente
- [ ] Atajos de teclado (Ctrl+O=abrir, F5=compilar, etc.)
- [ ] Tema oscuro/claro
- [ ] Autoguardado del código editado

### Optimizaciones
- [ ] Cache de compilación (no recompilar si el código no cambió)
- [ ] Ejecución paralela de tests independientes
- [ ] Modo "fast fail" (detener al primer error)
- [ ] Compilación incremental

### Robustez
- [ ] Validación más estricta de archivos de entrada/salida
- [ ] Manejo de archivos de gran tamaño
- [ ] Límite de memoria por proceso
- [ ] Sanitización de rutas de archivo
- [ ] Log de errores detallado

## 🧪 Testing

Para probar la aplicación con el ejemplo incluido:

1. Ejecuta la aplicación
2. Selecciona directorio: `EjemploProblema`
3. Selecciona archivo: `EjemploProblema\ProgramaEjemplo.cs`
4. Click en "Compilar y Ejecutar"
5. Deberías ver: "Completado: 2/2 correctos, 0 fallidos"

### Pruebas Adicionales

**Test de error de compilación:**
- Modifica el código para introducir un error de sintaxis
- Debería mostrar los errores de compilación

**Test de timeout:**
- Modifica el código para incluir `Thread.Sleep(10000);`
- Reduce el timeout a 1 segundo
- Debería mostrar TIMEOUT

**Test de salida incorrecta:**
- Modifica el código para generar una salida diferente
- Debería mostrar ERRÓNEO

## 📝 Notas Técnicas

- **Framework**: .NET 10.0 Windows Forms
- **Codificación**: UTF-8 en todos los archivos
- **Ejecución**: PowerShell para pipes (`type | dotnet run`)
- **Comparación**: Normaliza espacios finales y líneas vacías
- **Timeout**: Aplicado individualmente por test
- **Compilación**: Usa el .csproj del directorio del problema como plantilla

## 📄 Licencia

Este proyecto es de código abierto y puede ser usado libremente para propósitos educativos.

## 🤝 Contribuciones

Sugerencias de mejora y reportes de bugs son bienvenidos.
