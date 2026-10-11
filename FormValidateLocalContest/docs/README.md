# Documentación de Desarrollo - FormValidateLocalContest

## Estructura de Directorios

```
FormValidateLocalContest/
├── AGENTS.md                          # Guía universal para agentes (estándar multiplataforma)
│
├── .copilot/                          # Configuración local de agentes (NO comprometer)
│   ├── .instructions.md               # Instrucciones locales para agentes
│   ├── config.json                    # Configuración central
│   ├── README.md                      # Guía de configuración
│   └── output/                        # Salidas generadas por skills
│
├── docs/                              # Documentación profesional (SÍ comprometer)
│   ├── README.md                      # Índice maestro
│   ├── CARACTERÍSTICAS.md             # Descripción de características
│   ├── ARQUITECTURA.md                # Arquitectura de la aplicación
│   ├── GUIA_USUARIO.md               # Manual para usuarios finales
│   ├── GUIA_DESARROLLO.md            # Manual para desarrolladores
│   ├── CONTRIBUIR.md                 # Guía de contribución
│   ├── CAMBIOS_DATASETS.md           # Documentación de nueva característica datasets
│   │
│   └── skills/                        # Documentación de skills por tema
│       ├── README.md                  # Índice de skills disponibles
│       │
│       └── diff-code-review/          # Skill 1: Revisión de código automática
│           ├── .instructions.md       # Instrucciones del skill (estándar VS Code)
│           ├── MANUAL_DE_USO.md       # Guía de uso del skill
│           ├── SKILL_REFERENCE.md     # Referencia técnica
│           ├── EJEMPLOS.md            # Casos de uso y ejemplos
│           └── SALIDAS_ESPERADAS.md   # Formato de outputs
│
├── FormMain.cs                        # Interfaz principal y lógica
├── FormMain.Designer.cs               # Generado por VS Designer
├── Program.cs                         # Punto de entrada
├── FormValidateLocalContest.csproj    # Configuración del proyecto
├── FormValidateLocalContest.sln       # Solución
│
├── Problems.zip                       # Problemas de concurso (datasets incluidos)
├── .gitignore                         # Exclusiones de git (incluye .copilot/)
└── README.md                          # README principal del proyecto

```

## Navegación por Rol

### Para Usuarios Finales
1. Comienza en: [`docs/GUIA_USUARIO.md`](./GUIA_USUARIO.md)
2. Luego: [`docs/CARACTERÍSTICAS.md`](./CARACTERÍSTICAS.md)
3. Para características específicas: [`docs/CAMBIOS_DATASETS.md`](./CAMBIOS_DATASETS.md)

### Para Desarrolladores
1. Comienza en: [`docs/GUIA_DESARROLLO.md`](./GUIA_DESARROLLO.md)
2. Comprende la arquitectura: [`docs/ARQUITECTURA.md`](./ARQUITECTURA.md)
3. Lee skills relevantes: [`docs/skills/diff-code-review/MANUAL_DE_USO.md`](./skills/diff-code-review/MANUAL_DE_USO.md)
4. Antes de contribuir: [`docs/CONTRIBUIR.md`](./CONTRIBUIR.md)

### Para Revisores de Código
1. Usa el skill: `/diff-code-review --files <archivo> --format side-by-side --language tercera --group-by-section true`
2. Lee outputs en: `.copilot/output/diff-code-review/`
3. Para preguntas: [`docs/skills/diff-code-review/MANUAL_DE_USO.md`](./skills/diff-code-review/MANUAL_DE_USO.md)

### Para Mantenedores
1. Panorama completo: [`docs/ARQUITECTURA.md`](./ARQUITECTURA.md)
2. Hoja de ruta: [`docs/CONTRIBUIR.md`](./CONTRIBUIR.md) (sección "Roadmap Futuro")
3. Monitoreo de skills: [`.copilot/README.md`](../.copilot/README.md)

## Filosofía de Organización

Este proyecto implementa **Desarrollo Orientado a Agentes (Agent-Driven Development, ADD)**, donde:

- **Herramientas de IA** (Copilot, skills) actúan como agentes colaborativos
- **Documentación automática** se genera en tercera persona (profesional, sin sesgos)
- **Skills reutilizables** encapsulan análisis complejos (code review, security audit, etc.)
- **Directorios claros** separan configuración temporal de documentación permanente

### Principios

1. ✅ **Documentación profesional en tercera persona**: Neutral, formal, enfocada en hechos
2. ✅ **Skills reutilizables entre proyectos**: Inversión en infraestructura de agentes
3. ✅ **Separación clara**: `.copilot/` (temporal, no comprometer) vs `docs/` (permanente, comprometer)
4. ✅ **Outputs organizados**: `.copilot/output/` evita saturación visual del workspace
5. ✅ **Escalabilidad**: Estructura lista para crecer con más skills (security-review, performance-audit, etc.)

## Mantenimiento de Directorios

### .copilot/ (Local, NO comprometer a Git)

Contiene:
- Configuración de skills (`config.json`)
- Definiciones técnicas de skills (`SKILL_*.md`)
- Salidas automáticas de skills (`output/`)

**Uso**: Configuración local del developer y máquina de agentes

**En .gitignore**:
```
.copilot/
```

### docs/ (Permanente, SÍ comprometer a Git)

Contiene:
- Guías de usuario, desarrollo, contribución
- Documentación de características
- Manuales de skills (para referencia pública)
- Arquitectura y decisiones de diseño

**Uso**: Fuente de verdad para usuarios y desarrolladores

**En Git**: Todo el directorio `docs/`

## Scripts Útiles

### Ejecutar Skill diff-code-review
```powershell
# En VS Code o terminal
/diff-code-review --files FormMain.cs --format side-by-side --language tercera --group-by-section true

# Outputs generados en: .copilot/output/diff-code-review/
```

### Limpiar Outputs Antiguos
```bash
# Mantener últimos 5 análisis, eliminar anteriores
ls .copilot/output/diff-code-review/ | sort -r | tail -n +6 | xargs rm -rf
```

### Preparar para Portabilidad
```bash
# Copiar skill a ~/.copilot/skills/ para usar en otros proyectos
mkdir -p ~/.copilot/skills/diff-code-review
cp .copilot/SKILL_DIFF_REVIEW.md ~/.copilot/skills/diff-code-review/
cp -r docs/skills/diff-code-review/ ~/.copilot/skills/
```

## Roadmap Futuro

Próximos skills planificados:

| Skill | Descripción | Token Savings | Prioridad |
|-------|-------------|---------------|-----------|
| `security-review` | Análisis de vulnerabilidades | 60-70% | Alta |
| `performance-audit` | Detección de anti-patrones | 55-65% | Alta |
| `test-generation` | Generación de casos de prueba | 45-55% | Media |
| `documentation-generator` | Documentación automática | 70-80% | Media |

## Contacto y Contribución

Para:
- **Bugs reportados**: Ver [`docs/CONTRIBUIR.md`](./CONTRIBUIR.md)
- **Nuevos skills**: Consultar [`docs/CONTRIBUIR.md#nuevos-skills`](./CONTRIBUIR.md)
- **Preguntas sobre skills**: Ver documentación específica en `docs/skills/<skillname>/`

---

**Última actualización**: 2025-01-20  
**Versión**: 1.0.0  
**Mantenedor**: Equipo de Desarrollo  
**Estatus**: ✅ Producción (Aprobado)
