# Skill: Diff Code Review - Análisis de Cambios

## Descripción

Se ha creado un skill personalizado para facilitar la revisión de código mediante análisis detallado de cambios (diffs) antes y después de las modificaciones. Este skill está diseñado para:

1. **Generar diffs claros** mostrando exactamente qué se cambió
2. **Mostrar contexto** con líneas antes y después
3. **Facilitar code review** sin necesidad de revisar archivos completos
4. **Reutilizable** para futuros proyectos y cambios

## Cómo Usar Este Skill

### Invocación en Chat

Cuando necesites revisar cambios realizados en un proyecto, simplemente solicita:

```
Usa el skill diff-code-review para mostrar los cambios realizados en FormMain.cs y FormMain.Designer.cs
```

O de forma más específica:

```
Genera un diff detallado de los cambios en FormMain.cs desde la línea 80 hasta la línea 200, 
mostrando antes/después con explicaciones en tercera persona
```

### Parámetros del Skill

El skill acepta los siguientes parámetros:

| Parámetro | Tipo | Descripción | Ejemplo |
|-----------|------|-------------|---------|
| `files` | array | Lista de archivos a revisar | `["FormMain.cs", "FormMain.Designer.cs"]` |
| `format` | string | Formato del diff: `unified`, `side-by-side`, `compact` | `"unified"` |
| `context_lines` | number | Líneas de contexto antes/después del cambio | `3` |
| `language` person` | string | Persona gramatical: `tercera`, `primera` | `"tercera"` |
| `group_by_section` | bool | Agrupar cambios por sección lógica | `true` |

### Ejemplos de Uso

#### Ejemplo 1: Diff Completo en Formato Unified
```
/diff-code-review --files FormMain.cs --format unified --context-lines 3 --language tercera
```

**Salida esperada:**
```
=== Archivo: FormMain.cs ===

--- Antes (línea XXX)
+++ Después (línea YYY)

@@ -435,5 +435,20 @@
  
  private void lstProblems_SelectedIndexChanged(object sender, EventArgs e)
  {
-     // Código anterior
+     // Cargar datasets disponibles
+     LoadAvailableDatasets(problemPath);
  }

Se ha agregado una llamada a LoadAvailableDatasets() que...
```

#### Ejemplo 2: Diff Side-by-Side Compacto
```
/diff-code-review --files FormMain.Designer.cs --format side-by-side --language tercera --group-by-section true
```

#### Ejemplo 3: Múltiples Archivos
```
/diff-code-review --files FormMain.cs FormMain.Designer.cs FormValidateLocalContest.csproj --format compact
```

---

## Formato de Salida

### Formato Unified (Predeterminado)
Muestra líneas añadidas con `+` y líneas eliminadas con `-`:

```diff
 Línea sin cambio
-Línea eliminada
+Línea añadida
 Otra línea sin cambio
```

### Formato Side-by-Side
Muestra cambios en dos columnas:

```
ANTES                              │ DESPUÉS
─────────────────────────────────────────────────
string testDataDir = ...;          │ string testDataDir = selectedDatasetPath ?? 
                                   │        problemaDirectory ?? "";
```

### Formato Compact
Resumen breve de cambios:

```
[ADDED] 10 líneas en btnRunConsole_Click()
[MODIFIED] 5 líneas en CompileAndRunTests()
[REMOVED] 3 líneas (referencias a testDataDir)
```

---

## Ventajas del Skill

1. **Ahorra tokens**: Evita pasar archivos completos
2. **Claridad**: Enfoque en cambios relevantes
3. **Documentación**: Explicaciones en tercera persona
4. **Reusable**: Mismo skill para cualquier proyecto
5. **Flexible**: Múltiples formatos y opciones

---

## Casos de Uso

### Caso 1: Code Review Post-Cambios
Después de implementar cambios, solicita:
```
Usa el skill diff-code-review en FormMain.cs para que pueda revisar qué se modificó
```

### Caso 2: Documentación de Cambios
Para documentar cambios en un PR:
```
Genera un diff en formato side-by-side de FormMain.Designer.cs con explicaciones en tercera persona
```

### Caso 3: Verificación de Completitud
Para verificar que todos los cambios se realizaron:
```
Muestra los diffs completos de FormMain.cs y FormMain.Designer.cs agrupados por sección
```

---

## Integración con Workflow

```
Flujo típico de desarrollo:
1. Implementar cambios
2. Compilar y verificar (→ sin errores)
3. **Usar skill diff-code-review** ← AQUÍ
4. Revisar cambios
5. Si hay problemas → volver a paso 1
6. Si OK → commit y push
```

---

## Ejemplos Reales del Proyecto

### Cambios en FormMain.cs - Añadido btnRunConsole_Click()

```
Ubicación: Líneas 603-715
Tipo: Adición completa de método
Explicación en tercera persona: Se ha añadido un nuevo método asíncrono que 
ejecuta el programa compilado con el contenido del txtStdin como entrada estándar...
```

### Cambios en FormMain.Designer.cs - Nuevos Controles

```
Ubicación: Múltiples secciones
Tipo: Adiciones en InitializeComponent()
Explicación: Se han agregado cinco nuevos controles (lblStdin, txtStdin, lblStdout, 
txtStdout, btnRunConsole) inicializados en el método InitializeComponent()...
```

---

## Notas Técnicas

- El skill utiliza git diff internamente para generar diffs precisos
- Soporta archivos de cualquier extensión (.cs, .csproj, .md, etc.)
- Línea de contexto se ajusta automáticamente al archivo
- Las explicaciones se generan en tercera persona de forma automática
- Compatible con repositorios locales y remotos

---

## Mantenimiento

Este skill se mantiene actualizado junto con los cambios del proyecto y puede ser reutilizado en:
- ✅ Futuros cambios en FormValidateLocalContest
- ✅ Otros proyectos C# de WinForms
- ✅ Proyectos en cualquier lenguaje
- ✅ Code reviews en general

Para solicitar mejoras o ajustes, incluir en la solicitud: "Mejorar skill diff-code-review"
