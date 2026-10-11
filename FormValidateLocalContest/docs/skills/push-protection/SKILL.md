# Skill: push-protection

---yaml
name: "push-protection"
description: "Sistema de confirmación obligatoria antes de hacer push a GitHub"
author: "FormValidateLocalContest Team"
version: "1.0.0"
type: "hook"
trigger: "pre-push"
---

## Overview

El skill **push-protection** proporciona un sistema de confirmación interactiva antes de hacer push a GitHub. Analiza commits, verifica seguridad y requiere confirmación explícita del usuario.

## Características

### 1. Análisis de Commits
- Lista todos los commits que serán empujados
- Muestra asunto, autor y fecha de cada commit
- Muestra número total de commits

### 2. Resumen de Cambios
- Estadísticas de líneas modificadas por archivo
- Lista completa de archivos afectados
- Muestra qué fue agregado, modificado o eliminado

### 3. Verificación de Seguridad
Detecta y bloquea automáticamente:
- Archivos con credenciales (`*token*`, `*secret*`, `*password*`)
- Variables de entorno (`.env`, `.env.local`)
- Claves criptográficas (`*key*`, `*private*`)

### 4. Confirmación Interactiva
- Requiere respuesta explícita del usuario (Y/N)
- Cancela push si el usuario rechaza
- Cancela push si se detectan archivos sospechosos
- Preserva commits en local si se cancela

## Invocación

El hook se ejecuta automáticamente con:

```bash
git push origin main
```

Aparecerá un prompt interactivo pidiendo confirmación.

## Flujo de Ejecución

```
git push origin main
         ↓
[PRE-PUSH HOOK TRIGGERED]
         ↓
┌─ Analizar commits
│  ├─ Contar commits pendientes
│  ├─ Mostrar lista de commits
│  ├─ Mostrar resumen de cambios
│  └─ Listar archivos modificados
├─ Verificar seguridad
│  ├─ Buscar credenciales
│  ├─ Buscar variables de entorno
│  └─ Buscar claves privadas
├─ Si encuentra sospechosos
│  └─ ❌ BLOQUEAR PUSH
└─ Si todo OK
   ├─ Mostrar resumen
   ├─ Pedir confirmación del usuario
   ├─ Si usuario confirma (Y)
   │  └─ ✅ PERMITIR PUSH
   └─ Si usuario rechaza (N)
      └─ ❌ CANCELAR PUSH (commits seguros en local)
```

## Pantalla de Confirmación

```
════════════════════════════════════════════════════════════════
🔐 PRE-PUSH CONFIRMATION HOOK
════════════════════════════════════════════════════════════════

📤 Push Detection:
   Remote: origin
   URL: https://github.com/gcarlossuarez/FormValidateLocalContest

📋 Commits a Empujar:
   • d51fb26 - docs: Agregar documentación de skill diff-code-review
   • 642d418 - fix: Reorganizar layout a pantalla completa

📊 Resumen de Cambios:
   FormMain.cs              |  54 +++++-
   FormMain.Designer.cs     | 238 ++++++++++++++++++++
   LAYOUT_REORGANIZADO.md   | 220 +++++++++++++++++++
   3 files changed, 512 insertions(+)

📁 Archivos Modificados:
   • FormMain.cs
   • FormMain.Designer.cs
   • LAYOUT_REORGANIZADO.md

🔒 Verificación de Seguridad:
   ✅ No se detectaron archivos con credenciales

════════════════════════════════════════════════════════════════
❓ ¿Deseas proceder con el push de 2 commit(s)?

   Opciones:
   [Y/y] - Confirmar y hacer push
   [N/n] - Cancelar push

Tu respuesta: █
```

## Casos de Uso

### Caso 1: Push Normal (Confirmado)
```
$ git push origin main

🔐 PRE-PUSH CONFIRMATION HOOK
📋 Commits a Empujar: 1
...
❓ ¿Deseas proceder?
Tu respuesta: Y
✅ Push confirmado. Proceediendo...

Pushing to origin...
[resultado normal de git push]
```

### Caso 2: Push Rechazado por Usuario
```
$ git push origin main

🔐 PRE-PUSH CONFIRMATION HOOK
📋 Commits a Empujar: 1
...
❓ ¿Deseas proceder?
Tu respuesta: N
❌ Push cancelado por el usuario
💡 Tus commits están seguros en local
[git push abortado]
```

### Caso 3: Push Bloqueado por Seguridad
```
$ git push origin main

🔐 PRE-PUSH CONFIRMATION HOOK
📋 Commits a Empujar: 1
...
🔒 Verificación de Seguridad:
⚠️  ADVERTENCIA: Archivos sospechosos detectados:
   ❌ config.env
   ❌ secrets.json
❌ No se puede continuar con el push
[git push abortado - seguridad]
```

## Archivos del Hook

### Windows (Recomendado)
- `.git/hooks/pre-push.cmd` - Implementación en Batch/PowerShell
- Compatible con Git Bash y PowerShell
- Ejecutable desde cualquier cliente Git en Windows

### Linux/Mac
- `.git/hooks/pre-push` - Implementación en Bash
- Requiere permisos ejecutables: `chmod +x .git/hooks/pre-push`

## Configuración

El hook se ejecuta automáticamente. No requiere configuración adicional.

### Deshabilitar Temporalmente (si es necesario)

```bash
# Renombrar el hook para deshabilitarlo
mv .git/hooks/pre-push .git/hooks/pre-push.disabled

# Después de hacer push sin confirmación, restaurar:
mv .git/hooks/pre-push.disabled .git/hooks/pre-push
```

### Forzar Push sin Hook (NO recomendado)

```bash
# Solo si realmente sabes qué haces:
git push --no-verify origin main
```

## Seguridad

### Patrones Detectados

El hook busca automáticamente:

| Patrón | Razón | Ejemplo |
|--------|-------|---------|
| `*token*` | Tokens de acceso | `GITHUB_TOKEN`, `api_token` |
| `*secret*` | Secretos de aplicación | `.env.secrets`, `api_secret` |
| `*password*` | Contraseñas | `db_password`, `admin_password` |
| `*credential*` | Credenciales | `.credentials`, `aws_credentials` |
| `.env*` | Variables de entorno | `.env`, `.env.local` |
| `*key*` | Claves privadas | `private.key`, `ssh_key` |

### Falsos Positivos

Si necesitas subir un archivo con estos nombres (pero sin datos sensibles):

1. **Opción A**: Renombra temporalmente
2. **Opción B**: Usa `--no-verify` (después de revisar manualmente)
3. **Opción C**: Agrega a `.gitignore` si no debe versionarse

## Instalación

El hook ya está instalado en `.git/hooks/`. Se ejecuta automáticamente con cada `git push`.

Para verificar que funciona:

```bash
git push origin main
```

Deberías ver el prompt de confirmación.

## Monitoreo

El hook verifica antes de cada push:
- ✅ Commits pendientes
- ✅ Archivos modificados
- ✅ Credenciales/secrets
- ✅ Confirmación del usuario

## Futuras Mejoras

- [ ] Registro de auditoría automático
- [ ] Integración con GitHub Actions
- [ ] Validación de mensajes de commit
- [ ] Verificación de firmas GPG
- [ ] Notificaciones por email de pushes

## Referencias

- [Git Hooks](https://git-scm.com/book/en/v2/Customizing-Git-Git-Hooks)
- [Pre-push Hook Docs](https://git-scm.com/docs/githooks#_pre_push)
- [GitHub Security Best Practices](https://docs.github.com/en/code-security)

---

**Última actualización**: 2026-10-11  
**Versión**: 1.0.0  
**Estado**: Activo en repositorio
