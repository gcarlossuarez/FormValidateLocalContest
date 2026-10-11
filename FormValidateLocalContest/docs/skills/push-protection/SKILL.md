# Skill: push-protection

---yaml
name: "push-protection"
description: "Verificación de seguridad ANTES de hacer push - Scripts para agentes, libertad para usuario"
author: "FormValidateLocalContest Team"
version: "2.0.0"
type: "verification-script"
---

## Overview

El skill **push-protection 2.0** proporciona verificación de seguridad **sin bloquear al usuario**:
- ✅ **Usuario** (tú): Libertad total en consola, sin hooks automáticos
- ✅ **Agentes de IA**: Verificación automática antes de cada push
- ✅ **Seguridad**: Detecta tokens, credenciales, API keys, archivos `.env`

## Estrategia

| Actor | Herramienta | Comportamiento |
|-------|-------------|---|
| **Tú (Usuario)** | `git push` directo | Sin restricciones, push libre |
| **Yo (Agente)** | `.\safe-push.cmd` | Verifica → Pushea automático |
| **Otro agente** | `.\verify-before-push.ps1` | Verifica (sin pushear) |

## Scripts Disponibles

### 1. `verify-before-push.ps1` (Verificación sin Push)

**Ubicación**: Raíz del proyecto

**Uso**:
```powershell
# Verificar si hay secretos (NO hace push)
.\verify-before-push.ps1

# Con --Force ignora advertencias
.\verify-before-push.ps1 -Force
```

**Retorna**:
- `0` = OK, seguro para pushear
- `1` = Secretos detectados, NO pushear

**Detecta**:
- GitHub Tokens (`gh_*`, `ghp_*`, `github_pat_*`)
- AWS Keys (`AKIA*`)
- Private Keys (RSA, OpenSSH, PGP)
- API Keys y Passwords
- Archivos `.env`

**Ejemplo salida**:
```
========================================
Verificando cambios antes de push...
========================================

Commits por pushear:
3a6bc04 Auto sync and push
fd67f59 Auto sync and push

[OK] No se detectaron secretos. Seguro para pushear.
```

### 2. `safe-push.cmd` (Push Seguro para Agentes)

**Ubicación**: Raíz del proyecto

**Uso**:
```batch
.\safe-push.cmd
```

**Workflow**:
1. Llama `verify-before-push.ps1`
2. Si hay secretos → Bloquea (exit 1)
3. Si está limpio → Hace push automático
4. Agrega, commitea, rebase, push

## Cómo Yo (Agente) lo Uso

**Antes de hacer push automático**:

```powershell
# Opción 1: Verificar primero
.\verify-before-push.ps1
if ($LASTEXITCODE -eq 0) {
    # Hacer push manualmente
    git add .
    git commit -m "Auto sync"
    git push -u origin main
}

# Opción 2: Usar script combinado
.\safe-push.cmd
```

**En código de agente**:
```powershell
# Pre-push verification step
$verifyResult = & ".\verify-before-push.ps1"
if ($LASTEXITCODE -ne 0) {
    Write-Host "[BLOQUEADO] No se permite push con secretos detectados"
    exit 1
}
Write-Host "[OK] Procediendo con push..."
git push -u origin main
```

## Para Ti (Usuario)

**TÚ TIENES LIBERTAD TOTAL**:
```bash
# Esto funciona sin restricciones:
git push origin main
git push --force origin main
git push --all
```

Los scripts de verificación son **opcionales para ti**. Usúalos si quieres verificar, pero no son obligatorios.

## Patrones de Secretos Detectados

| Patrón | Descripción | Ejemplo |
|--------|-------------|---------|
| `gh_[A-Za-z0-9_]{36}` | GitHub Classic Token | `gh_16C7e42F292c6912E7710c838347Ae178B4a` |
| `ghp_[A-Za-z0-9_]{36}` | GitHub Personal Access Token | `ghp_1B3FDScd5mK8zL9nOpQrS4tUvWxYz2jk` |
| `github_pat_[A-Za-z0-9_]{82}` | GitHub PAT v2 | `github_pat_11AAAA...` |
| `AKIA[0-9A-Z]{16}` | AWS Access Key ID | `AKIAIOSFODNN7EXAMPLE` |
| `api[_-]?key` | API Key genérica | `api_key=abc123`, `apiKey: secret` |
| `password` | Contraseña | `password="MySecure123"` |
| `-----BEGIN.*PRIVATE KEY-----` | Clave privada | RSA, OpenSSH, PGP |
| `.env` | Variables de entorno | `.env`, `.env.local`, `.env.production` |

## Casos de Uso

### Caso 1: Yo (Agente) haciendo Push
```powershell
# Pre-push verification
.\verify-before-push.ps1

# Salida:
# [OK] No se detectaron secretos. Seguro para pushear.
# (retorna 0)

# Luego procedo con push
git push -u origin main
```

### Caso 2: Secreto Detectado
```powershell
.\verify-before-push.ps1

# Salida:
# [⚠️  ADVERTENCIA] Se detectaron posibles secretos:
#   - GitHub Token: 1 coincidencia(s)
# [BLOQUEADO] No se permite push con secretos detectados.

# (retorna 1, no permito push)
```

### Caso 3: Tú (Usuario) haciendo Push Manual
```bash
git push origin main

# Va directo, sin verificaciones
# (libertad total)
```

## Instalación

Los scripts ya están en la raíz del proyecto:
- ✅ `verify-before-push.ps1`
- ✅ `safe-push.cmd`

No requieren instalación adicional.

## Diferencia con v1.0.0

| Aspecto | v1.0.0 (Hooks) | v2.0.0 (Scripts) |
|--------|---|---|
| **Bloquea usuario** | ❌ Sí, hook automático | ✅ No, scripts opcionales |
| **Bloquea agente** | ❌ No | ✅ Sí, si hay secretos |
| **Ejecuta automático** | ✅ Sí (hook pre-push) | ❌ No, llamada explícita |
| **Interactividad** | ⚠️ Problemas en batch | ✅ No requiere interacción |
| **Configuración usuario** | ❌ Hook rompe flujo | ✅ Usuario tiene libertad |

## Futuras Mejoras

- [ ] Integración con GitHub push protection nativa
- [ ] Análisis de diff antes de push
- [ ] Registro de auditoría automático
- [ ] Validación de firmas GPG
- [ ] Notificaciones de push exitosos

---

**Última actualización**: 2026-10-11  
**Versión**: 2.0.0  
**Estado**: Activo - Scripts sin hooks automáticos
