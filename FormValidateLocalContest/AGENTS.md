# Agentes: FormValidateLocalContest

## Descripción General

**FormValidateLocalContest** es una aplicación WinForms para validar y evaluar soluciones de programación competitiva. La aplicación integra herramientas impulsadas por agentes de IA para facilitar el desarrollo orientado a agentes (Agent-Driven Development, ADD).

## Contexto Técnico

- **Lenguaje**: C# (.NET 10.0 para Windows)
- **Framework UI**: Windows Forms (WinForms)
- **Patrón de Arquitectura**: MVC (Model-View-Controller) adaptado a WinForms
- **Sistema de Compilación**: MSBuild (.NET CLI)
- **Control de Versiones**: Git
- **Agentes Soportados**: 
  - VS Code Copilot (GitHub)
  - Claude Code (Anthropic)
  - Gemini Code (Google)
  - Cursor
  - Sourcegraph Cody
  - Cualquier agente que soporte `AGENTS.md`

## Estructura del Proyecto

```
FormValidateLocalContest/
├── AGENTS.md                          # Este archivo (README para agentes)
├── .copilot/                          # Configuración local (NO comprometer)
│   ├── .instructions.md               # Instrucciones para agentes locales
│   ├── config.json                    # Mapeo de skills y directorios
│   └── output/                        # Salidas generadas automáticamente
├── docs/                              # Documentación profesional (comprometer)
│   ├── README.md                      # Índice maestro
│   └── skills/                        # Documentación de skills
│       └── diff-code-review/
│           ├── .instructions.md       # Instrucciones del skill específico
│           ├── MANUAL_DE_USO.md       # Guía de uso
│           ├── EJEMPLOS.md            # Casos de uso
│           └── SALIDAS_ESPERADAS.md   # Formato de salidas
├── FormMain.cs                        # Ventana principal (UI + lógica)
├── FormMain.Designer.cs               # Generado por designer
├── Program.cs                         # Punto de entrada
└── [archivos adicionales de proyecto]
```

## Flujos de Trabajo Soportados

### 1. Desarrollo Orientado a Agentes (Agent-Driven Development)

El proyecto utiliza agentes de IA para:

- **Code Review Automático**: Usar el skill `diff-code-review` para generar análisis detallados de cambios
- **Documentación en Tercera Persona**: Generar documentación neutral y profesional de cambios
- **Análisis de Diffs**: Comparar antes/después de modificaciones

**Invocación típica**:
```
/diff-code-review --files FormMain.cs --format side-by-side --language tercera --group-by-section true
```

### 2. Pruebas Interactivas

La aplicación permite:

- Seleccionar un problema de concurso
- Elegir un dataset (archivo `datos*.txt`) del problema
- Editar la entrada estándar (stdin) antes de ejecutar
- Ejecutar la solución y ver salida en tiempo real sin calificación oficial
- Comparar con la salida esperada manualmente

### 3. Evaluación Oficial

- Compilar y ejecutar contra TODOS los datasets del problema
- Generar calificación automática
- Registrar resultados

## Skills Disponibles

### diff-code-review

**Descripción**: Genera análisis detallados de cambios en código para facilitar revisiones automáticas.

**Ubicación de Documentación**: `docs/skills/diff-code-review/`

**Parámetros Principales**:
- `--files`: Archivos a analizar (admite glob patterns)
- `--format`: `unified`, `side-by-side`, o `compact`
- `--language`: `tercera` (recomendado para profesionalismo)
- `--group-by-section`: Agrupar cambios por sección lógica

**Invocación Rápida**:
```bash
/diff-code-review --files "FormMain*.cs" --format side-by-side
```

**Salidas Generadas**:
- `CODE_REVIEW_SUMMARY.md` - Resumen ejecutivo
- `DIFF_SIDE_BY_SIDE.md` - Análisis línea por línea
- `RECOMENDACIONES_FINALES.md` - Plan de acción
- `INDEX_CODE_REVIEW.md` - Índice de navegación

## Configuración de Agentes

### Archivos de Configuración

1. **`AGENTS.md`** (este archivo)
   - Estándar universal: reconocido por todos los agentes modernos
   - Ubicado en raíz del proyecto
   - Objetivo: proporcionar contexto general del proyecto

2. **`.copilot/.instructions.md`**
   - Instrucciones específicas para máquina local
   - Archivo gitignored (no se comprometera)
   - Objetivo: preferencias locales, paths específicos

3. **`docs/skills/diff-code-review/.instructions.md`**
   - Instrucciones del skill específico
   - Ubicado en directorio del skill
   - Objetivo: comportamiento esperado del skill

### Comportamiento de Descubrimiento

Los agentes buscan instrucciones en este orden de prioridad:

1. **Instrucciones específicas del contexto** (ej: `.copilot/[skill-name].instructions.md`)
2. **Instrucciones del skill** (ej: `docs/skills/[skill-name]/.instructions.md`)
3. **Instrucciones locales** (`.copilot/.instructions.md`)
4. **Instrucciones de proyecto** (este archivo: `AGENTS.md`)
5. **Instrucciones globales del usuario** (`~/.copilot/AGENTS.md` o `~/.claude/CLAUDE.md`)

## Convenciones de Desarrollo

### Estándares de Código

- **Lenguaje**: C# (.NET 10.0)
- **Estilo**: Microsoft C# Coding Conventions
- **Documentación XML**: Requerida en métodos públicos
- **Tercer Persona**: Documentación siempre en tercera persona neutral

### Estándares de Documentación

Toda documentación generada por agentes debe estar en **tercera persona** para:
- Mantener neutralidad profesional
- Facilitar lectura por stakeholders externos
- Asegurar consistencia en code reviews

**Ejemplo correcto**:
> "Se implementó la característica de selección de datasets, permitiendo al usuario cargar archivos `datos*.txt` asociados a cada problema de concurso."

**Ejemplo incorrecto**:
> "Implementé la característica de selección de datasets que te permite cargar archivos..."

## Información de Contacto y Contribución

- **Repositorio**: `gcarlossuarez/FormValidateLocalContest`
- **Documentación Completa**: Ver `docs/README.md`
- **Guía de Contribución**: Ver `docs/CONTRIBUIR.md`

## Referencias

- [agents.md Specification](https://agents.md/)
- [VS Code Custom Instructions](https://code.visualstudio.com/docs/agent-customization/custom-instructions)
- [Claude Code Documentation](https://code.claude.com/docs/en/claude-md)
- [Project Documentation Index](./docs/README.md)
- [Skills Catalog](./docs/skills/README.md)

---

*Última actualización: 2025*

Este archivo es reconocido automáticamente por VS Code Copilot, Claude Code, Gemini Code, Cursor, Sourcegraph Cody y otros agentes modernos de IA.
