# Skill: diff-code-review

---yaml
name: "diff-code-review"
description: "Generates detailed code review diffs comparing before/after changes"
author: "FormValidateLocalContest Team"
version: "1.0.0"
language: "tercera"  # Spanish, third person
format: "compact"
---

## Overview

El skill `diff-code-review` proporciona análisis detallado de cambios en código, generando comparativas antes/después con recomendaciones de mejora.

## Invocación

```bash
/diff-code-review --files FILE1.cs FILE2.cs --format compact --language tercera --group-by-section true
```

### Parámetros

| Parámetro | Descripción | Valores |
|-----------|-------------|---------|
| `--files` | Archivos a revisar (separados por espacio) | Rutas relativas o absolutas |
| `--format` | Formato de salida | `compact`, `side-by-side`, `detailed` |
| `--language` | Idioma de la salida | `español`, `tercera` (tercera persona) |
| `--group-by-section` | Agrupar por secciones de código | `true`, `false` |

## Formatos de Salida

### 1. Compact
Resumen conciso de cambios principales con líneas aproximadas.

```
📋 DIFF COMPACTO
━━━━━━━━━━━━━━━━━━━━
[cambios principales]
[líneas aproximadas]
✅ RESUMEN
⚠️ CONSIDERACIONES
```

### 2. Side-by-Side
Comparativa visual lado a lado del código antes y después.

```
ANTES                          │ DESPUÉS
───────────────────────────────┼─────────────────────────────
[código anterior]              │ [código nuevo]
```

### 3. Detailed
Análisis exhaustivo con explicaciones línea por línea.

```
📊 ANÁLISIS DETALLADO
━━━━━━━━━━━━━━━━━━━━━━
[sección 1]
  - Cambio A (línea X)
  - Cambio B (línea Y)
[sección 2]
  ...
```

## Casos de Uso

### Caso 1: Revisar Cambios de Layout
```bash
/diff-code-review --files FormMain.Designer.cs --format compact
```

Genera análisis de cambios en posiciones X,Y y tamaños de controles.

### Caso 2: Revisar Lógica y Métodos
```bash
/diff-code-review --files FormMain.cs --format side-by-side --group-by-section true
```

Compara métodos nuevos, modificados y eliminados lado a lado.

### Caso 3: Análisis Completo de Cambios
```bash
/diff-code-review --files FormMain.Designer.cs FormMain.cs --format detailed
```

Genera revisión exhaustiva de todos los cambios en ambos archivos.

## Salidas Esperadas

### Compact
```
📋 DIFF COMPACTO: FormMain.Designer.cs
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

CAMBIOS PRINCIPALES:
• Reorganización de posiciones X,Y de controles
• Aumentado ClientSize de (1000, 670) a (1000, 820)
• Agregado WindowState.Maximized
• Reducción de tamaños duplicados

LÍNEAS AFECTADAS: ~100-200 líneas

📋 DIFF COMPACTO: FormMain.cs
━━━━━━━━━━━━━━━━━━━━━━━━━━━━

CAMBIOS PRINCIPALES:
• Método NormalizeCodeText() agregado (~15 líneas)
• Evento TxtCode_TextChanged() implementado (~20 líneas)
• Aplicación de normalización en btnSelectFile_Click()

LÍNEAS AFECTADAS: ~50 líneas

✅ RESUMEN DE MEJORAS
• Layout más eficiente (50% más espacio vertical)
• Funcionalidad de preservación de saltos de línea
• Sin regresiones funcionales detectadas

⚠️ CONSIDERACIONES
• Flag isNormalizing previene recursión infinita
• Posiciones hardcodeadas podrían ser dinámicas en futuro
• Verificar visualmente en distintas resoluciones
```

### Side-by-Side
```
FormMain.Designer.cs

ANTES (Línea 31)                   │ DESPUÉS (Línea 31)
lstProblems.Size = new Size(400, 94)  │ lstProblems.Size = new Size(480, 200)
ClientSize = new Size(1000, 670)      │ ClientSize = new Size(1000, 820)
                                       │ WindowState = FormWindowState.Maximized
```

## Recomendaciones

1. **Ejecutar después de cambios importantes** de UI o lógica
2. **Revisar salidas detalladas** para cambios en métodos críticos
3. **Usar formato compacto** para resúmenes rápidos en commits
4. **Agregar a documentación** los cambios importantes detectados

## Referencias

- [MANUAL_DE_USO.md](./MANUAL_DE_USO.md) - Guía completa de uso
- [EJEMPLOS.md](./EJEMPLOS.md) - Casos de uso detallados
- [.instructions.md](./.instructions.md) - Instrucciones técnicas para agentes
- [SKILL_REFERENCE.md](./SKILL_REFERENCE.md) - Referencia técnica

---

**Última actualización**: 2026-10-11
**Versión**: 1.0.0
**Estado**: Activo y disponible para invocación
