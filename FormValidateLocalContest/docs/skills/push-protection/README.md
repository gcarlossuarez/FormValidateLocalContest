# Push Protection - Hook de Confirmación

## 🔐 ¿Qué es?

**push-protection** es un Git hook que requiere confirmación explícita del usuario antes de hacer `git push`. Evita empujes accidentales a GitHub y detecta automáticamente archivos con credenciales.

## ⚡ Uso Rápido

Solo escribe tu comando normal:

```bash
git push origin main
```

**Aparecerá un prompt pidiendo confirmación:**

```
════════════════════════════════════════════════════════════════
🔐 PRE-PUSH CONFIRMATION HOOK
════════════════════════════════════════════════════════════════

📤 Push Detection:
   Remote: origin
   URL: https://github.com/gcarlossuarez/FormValidateLocalContest

📋 Commits a Empujar:
   • abc1234 - fix: Resolver bug

📊 Resumen de Cambios:
   FormMain.cs | 54 ++++
   1 files changed, 54 insertions(+)

📁 Archivos Modificados:
   • FormMain.cs

🔒 Verificación de Seguridad:
   ✅ No se detectaron archivos con credenciales

════════════════════════════════════════════════════════════════
❓ ¿Deseas proceder con el push de 1 commit(s)?

   [Y/y] - Confirmar y hacer push
   [N/n] - Cancelar push

Tu respuesta: _
```

**Escribe:**
- `Y` para confirmar y hacer push
- `N` para cancelar (commits permanecen en local)

## 🛡️ Seguridad

El hook detecta y bloquea automáticamente archivos que contengan:

- Tokens (GitHub, API, etc.)
- Secretos de aplicación
- Contraseñas
- Credenciales
- Variables de entorno (`.env`)
- Claves privadas

**Si detecta algo sospechoso:**

```
🔒 Verificación de Seguridad:
⚠️  ADVERTENCIA: Archivos sospechosos detectados:
   ❌ .env
   ❌ secrets.json
❌ No se puede continuar con el push
```

El push se bloquea automáticamente.

## 📋 Instalación

✅ El hook ya está instalado en `.git/hooks/`

Se ejecuta automáticamente con cada `git push`.

### Verificar que funciona

```bash
git push origin main
```

Deberías ver el prompt de confirmación.

## ⚙️ Configuración

El hook está registrado en `.copilot/config.json`:

```json
{
  "name": "push-protection",
  "enabled": true,
  "trigger": "pre-push"
}
```

### Deshabilitar Temporalmente

Si necesitas hacer push sin confirmación (no recomendado):

**En Windows (PowerShell):**
```powershell
# Deshabilitar
Rename-Item .git\hooks\pre-push pre-push.disabled
git push origin main
# Restaurar
Rename-Item .git\hooks\pre-push.disabled pre-push
```

**En Linux/Mac:**
```bash
# Deshabilitar
mv .git/hooks/pre-push .git/hooks/pre-push.disabled
git push origin main
# Restaurar
mv .git/hooks/pre-push.disabled .git/hooks/pre-push
```

### Forzar Push sin Hook

Si REALMENTE necesitas hacerlo (solo si sabes qué haces):

```bash
git push --no-verify origin main
```

⚠️ **Advertencia**: Esto omite todas las verificaciones de seguridad.

## 📊 Ejemplos

### Ejemplo 1: Push Confirmado

```bash
$ git push origin main

🔐 PRE-PUSH CONFIRMATION HOOK
📋 Commits a Empujar: 1
...
❓ ¿Deseas proceder?
Tu respuesta: Y
✅ Push confirmado. Proceediendo...

[master abc1234] fix: Resolver bug
 1 file changed, 54 insertions(+)

Pushing to origin...
[resultado normal de git push]
```

### Ejemplo 2: Push Cancelado por Usuario

```bash
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

Los commits permanecen en tu máquina local y pueden ser empujados después.

### Ejemplo 3: Push Bloqueado por Seguridad

```bash
$ git push origin main

🔐 PRE-PUSH CONFIRMATION HOOK
📋 Commits a Empujar: 1
...
🔒 Verificación de Seguridad:
⚠️  ADVERTENCIA: Archivos sospechosos detectados:
   ❌ .env
   ❌ secrets.json
❌ No se puede continuar con el push
[git push abortado - seguridad]
```

Debe eliminar o renombrar esos archivos antes de hacer push.

## 🚀 Para Agentes de IA

Si eres un agente intentando hacer push:

### ✅ CORRECTO

1. Notifica al usuario qué vas a hacer:
   ```
   Tengo 3 commits listos:
   • fix: Resolver bug
   • docs: Actualizar README
   • test: Agregar pruebas
   
   ¿Deseas que haga git push origin main?
   ```

2. Espera confirmación explícita del usuario

3. Luego ejecuta `git push origin main`

4. El hook pedirá confirmación nuevamente (capa extra de protección)

### ❌ INCORRECTO

No intentes hacer push sin:
- Notificar al usuario
- Obtener confirmación explícita
- Documentar por qué haces push

### 🔧 Casos Especiales

Si necesitas hacer push sin interacción (en scripts automatizados):

```bash
git push --no-verify origin main
```

Pero **documenta claramente** por qué se necesita `--no-verify`.

## 📁 Archivos

| Archivo | Descripción |
|---------|-------------|
| `.git/hooks/pre-push` | Hook para Linux/Mac |
| `.git/hooks/pre-push.cmd` | Hook para Windows |
| `docs/skills/push-protection/SKILL.md` | Documentación técnica |
| `docs/skills/push-protection/.instructions.md` | Instrucciones para agentes |
| `.copilot/config.json` | Configuración del hook |

## 🔍 Troubleshooting

### "Hook not executing"

Verifica que los archivos existan:
```bash
ls -la .git/hooks/pre-push*
```

### "Permission denied" (Linux/Mac)

```bash
chmod +x .git/hooks/pre-push
```

### "Cannot prompt in non-interactive mode"

El hook requiere una terminal interactiva. No funciona con:
- Piping a otro proceso
- Scripts completamente no-interactivos
- CI/CD pipelines

### Falso Positivo: Archivo legítimo bloqueado

Si un archivo se llama `config.env` pero es seguro (sin secretos):

**Opción 1**: Agrégalo a `.gitignore`:
```bash
echo "config.env" >> .gitignore
```

**Opción 2**: Usa `--no-verify` después de revisar manualmente:
```bash
git push --no-verify origin main
```

## 💡 Ventajas

✅ **Previene Accidentes**
- No se pueden hacer push sin confirmación
- Commits permanecen seguros en local

✅ **Seguridad Integrada**
- Detecta credenciales automáticamente
- Bloquea archivos sospechosos

✅ **Transparencia**
- Muestra exactamente qué se va a empujar
- Permite revisar antes de confirmar

✅ **Auditoría**
- Registro de quién hizo push y cuándo
- Trazabilidad completa

## 📚 Más Información

- [SKILL.md](./SKILL.md) - Documentación técnica completa
- [.instructions.md](./.instructions.md) - Instrucciones técnicas
- [Git Hooks](https://git-scm.com/book/en/v2/Customizing-Git-Git-Hooks)

---

**Última actualización**: 2026-10-11  
**Versión**: 1.0.0  
**Estado**: ✅ Activo en repositorio
