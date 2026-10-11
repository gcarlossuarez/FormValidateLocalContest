# Skills Disponibles - FormValidateLocalContest

Catálogo de herramientas automatizadas de análisis y revisión generadas por agentes de Copilot.

## Overview

| Skill | Descripción | Uso | Estado |
|-------|-------------|-----|--------|
| **diff-code-review** | Análisis automático de cambios en código | `/diff-code-review --files <file>` | ✅ Activo |
| **security-review** | Análisis de vulnerabilidades | Planificado | 📋 Roadmap |
| **performance-audit** | Detección de anti-patrones | Planificado | 📋 Roadmap |
| **test-generation** | Generación de casos de prueba | Planificado | 📋 Roadmap |

## 1. diff-code-review ✅

**Propósito**: Analizar cambios de código y generar documentación profesional de revisión.

**Beneficios**:
- ✅ Análisis automático sin sesgos en tercera persona
- ✅ Diff lado a lado con explicaciones detalladas
- ✅ Recomendaciones de mejora estructuradas
- ✅ Ahorro de tokens: 50-65% vs revisión manual

**Documentación**:
- [Manual de Uso](./diff-code-review/MANUAL_DE_USO.md)
- [Referencia Técnica](./diff-code-review/SKILL_REFERENCE.md)
- [Salidas Esperadas](./diff-code-review/SALIDAS_ESPERADAS.md)
- [Ejemplos](./diff-code-review/EJEMPLOS.md)

**Entrada Típica**:
```bash
/diff-code-review --files FormMain.Designer.cs --format side-by-side --language tercera --group-by-section true
```

**Salida Típica** (4 documentos):
```
.copilot/output/diff-code-review/2025-01-20_FormMain.Designer.cs/
├── CODE_REVIEW_SUMMARY.md          # Resumen ejecutivo (14.6 KB)
├── DIFF_SIDE_BY_SIDE.md            # Análisis línea por línea (15.1 KB)
├── RECOMENDACIONES_FINALES.md      # Plan de acción (9.1 KB)
└── INDEX_CODE_REVIEW.md            # Guía de navegación (9.4 KB)
```

**Lenguajes Soportados**:
- `tercera` (Recomendado) - Análisis profesional, formal, imparcial
- `técnica` - Enfoque en implementación y patrones
- `usuario` - Lenguaje accesible para no-técnicos

**Filtros de Archivos**:
- `.cs` - C# (primario)
- `.ts`, `.tsx` - TypeScript/React
- `.py` - Python
- `.js`, `.jsx` - JavaScript
- `.go` - Go
- `.java` - Java
- `.sql` - SQL
- Cualquier archivo de código

---

## 2. security-review 📋

**Estado**: Planificado (Roadmap Q1 2025)

**Propósito**: Identificar vulnerabilidades y patrones de seguridad problemáticos.

**Funcionalidades Planeadas**:
- Análisis de inyección SQL, XSS, CSRF
- Validación de entrada/output
- Gestión de secretos y credenciales
- Patrones criptográficos
- Autenticación y autorización

**Documentación**: Disponible cuando se implemente

---

## 3. performance-audit 📋

**Estado**: Planificado (Roadmap Q1 2025)

**Propósito**: Detectar anti-patrones de rendimiento y optimización.

**Funcionalidades Planeadas**:
- Análisis de memoria y asignaciones
- Detección de loops ineficientes
- Problemas de async/await
- Uso ineficiente de colecciones
- Queries N+1

**Documentación**: Disponible cuando se implemente

---

## 4. test-generation 📋

**Estado**: Planificado (Roadmap Q2 2025)

**Propósito**: Generar casos de prueba automáticamente.

**Funcionalidades Planeadas**:
- Generación de casos de uso
- Edge cases automáticos
- Cobertura de código estimada
- Mock objects
- Test fixtures

**Documentación**: Disponible cuando se implemente

---

## Cómo Usar Skills

### Patrón General

```bash
/[nombre-skill] [opciones] --language tercera --group-by-section true
```

### Opción 1: Invocar desde VS Code Chat

```
Usuario: /diff-code-review --files FormMain.cs --format side-by-side --language tercera --group-by-section true

VS Code Copilot:
1. Lee .copilot/SKILL_DIFF_REVIEW.md
2. Detecta archivos modificados
3. Genera 4 documentos
4. Guarda en .copilot/output/diff-code-review/
```

### Opción 2: Usar desde Terminal PowerShell

```powershell
# Terminal abierto en directorio del proyecto
code --profile default --command "copilot.chat.openSymbolFromReferences"

# O simplemente abrir VS Code y usar Chat con /diff-code-review
```

### Opción 3: Integrar en Scripts de CI/CD

```yaml
# .github/workflows/code-review.yml
name: Automatic Code Review
on: [pull_request]
jobs:
  review:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Install Copilot CLI (futuro)
        run: npm install -g @github/copilot-cli
      - name: Run Code Review
        run: copilot diff-code-review --files "src/**/*.cs"
```

---

## Descoberta de Skills

¿Cómo sabe Copilot dónde encontrar los skills?

### Orden de Búsqueda
1. **Workspace local**: `.copilot/SKILL_*.md` (este proyecto)
2. **Proyecto**: `.copilot/skills/` (si existe)
3. **Usuario global**: `~/.copilot/skills/` (en tu máquina)
4. **Repositorio Git**: `.git/.copilot/` (en el repo)

### Archivo de Configuración
`.copilot/config.json` define:
```json
{
  "skills": [
    {
      "name": "diff-code-review",
      "enabled": true,
      "documentation_path": "../docs/skills/diff-code-review"
    }
  ]
}
```

---

## Reutilizar Skills en Otros Proyectos

### Opción 1: Copiar Localmente (Simple, No Portátil)
```bash
# En <nuevo-proyecto>/
cp -r <FormValidateLocalContest>/.copilot .
cp -r <FormValidateLocalContest>/docs/skills ./docs/
```
❌ Problema: Duplicación, difícil mantener sincronización

### Opción 2: Central Repository (Recomendado, Portátil) ✅
```bash
# Una sola vez, en tu máquina
mkdir -p ~/.copilot/skills/
cp <FormValidateLocalContest>/.copilot/SKILL_DIFF_REVIEW.md ~/.copilot/skills/diff-code-review/
cp -r <FormValidateLocalContest>/docs/skills/diff-code-review ~/.copilot/skills/

# En cualquier proyecto: create .copilot/config.json
{
  "skills_path": "~/.copilot/skills",
  "skills": ["diff-code-review"]
}

# Usar: /diff-code-review --files <file>
```
✅ Ventaja: 1 skill definition, infinitos proyectos

### Opción 3: Git Submodule (Avanzado, Centralizado)
```bash
# En el proyecto nuevo
git submodule add https://github.com/tu-org/copilot-skills .copilot/skills

# En .copilot/config.json
{
  "skills_path": "./.copilot/skills",
  "skills": ["diff-code-review"]
}
```

---

## Integración con Desarrollo Orientado a Agentes (ADD)

Este proyecto implementa **Agent-Driven Development**, donde skills forman parte integral del workflow:

```
Desarrollo Manual
       ↓
    Escribir Código
       ↓
    [AGENT] /diff-code-review    ← Revisión automática
       ↓
    Analizar Outputs (.copilot/output/)
       ↓
    Implementar Recomendaciones
       ↓
    Commit → Push → CI/CD Pipeline
       ↓
    [Futuro] /security-review    ← Análisis de seguridad
       ↓
    [Futuro] /performance-audit  ← Optimización
```

## Roadmap Futuro

```
Q1 2025
├── security-review        (Prioridad: ALTA)
└── performance-audit      (Prioridad: ALTA)

Q2 2025
├── test-generation        (Prioridad: MEDIA)
└── documentation-generator (Prioridad: MEDIA)

Q3 2025+
├── architecture-review
├── type-safety-audit
└── [Comunidad puede proponer]
```

## Contribuir Nuevo Skill

Ver [`docs/CONTRIBUIR.md#crear-nuevo-skill`](../CONTRIBUIR.md)

Pasos resumidos:
1. Define propósito en `docs/skills/<nuevo-skill>/SKILL_REFERENCE.md`
2. Crea `.copilot/SKILL_<nombre>.md` con instrucciones
3. Prueba en `.copilot/output/`
4. Documenta salidas esperadas
5. Abre PR con revisión de skill

---

**Última actualización**: 2025-01-20  
**Versión**: 1.0.0  
**Status**: ✅ Producción
