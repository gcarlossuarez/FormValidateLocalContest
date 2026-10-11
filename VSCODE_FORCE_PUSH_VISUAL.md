# Guía Visual: Force Push desde VS Code

## 🎯 Objetivo
Hacer force push a GitHub desde VS Code para reemplazar commits con credenciales.

## ✅ Verificación Inicial
- [x] VS Code abierto
- [x] Proyecto FormValidateLocalContest cargado
- [x] Historial local limpio (commit 44d4fb8)
- [x] 2 commits adelante de origin/main

---

## 📸 Pasos Visuales

### PASO 1: Abrir Source Control
**Ubicación**: Barra izquierda de VS Code

```
VS Code Sidebar:
┌─────────────────────┐
│ 📁 Explorer         │  ← Explorador de archivos
│ 🔍 Search           │  ← Búsqueda
│ 🌿 Source Control   │  ← AQUÍ (hacer click)
│ ⏯️  Run and Debug    │  ← Debugging
│ 🧩 Extensions       │  ← Extensiones
└─────────────────────┘

O presiona: Ctrl+Shift+G
```

### PASO 2: Ver el Panel de Source Control

Una vez haces click en "Source Control", deberías ver:

```
╔════════════════════════════════╗
║ SOURCE CONTROL                 ║
╠════════════════════════════════╣
║ 📌 Current Branch: main        ║
║                                ║
║ ┌──────────────────────────┐   ║
║ │ [↑ Push]  [↕ Sync]  [⋯]  │   ║
║ └──────────────────────────┘   ║
║                                ║
║ 📋 CHANGES (0)                 ║
║ (No cambios sin stagear)       ║
║                                ║
║ 📝 COMMIT MESSAGE:             ║
║ ┌──────────────────────────┐   ║
║ │                          │   ║
║ └──────────────────────────┘   ║
║                                ║
║ [Commit] [Discard All]         ║
╚════════════════════════════════╝
```

### PASO 3: Click en los 3 Puntos (⋯)

Busca el botón **⋯** (tres puntos) en el panel Source Control.

```
Ubicación del botón:
   [↑ Push]  [↕ Sync]  [⋯]  ← CLICK AQUÍ
```

Click en el botón ⋯ abrirá un menú desplegable.

### PASO 4: Seleccionar "Push (Force)"

El menú mostrará varias opciones:

```
┌─────────────────────────────────────────┐
│ Create Branch from HEAD...              │
│ Checkout Branch...                      │
│ Discard All Changes...                  │
│ Stash...                                │
│ Publish Branch                          │
│ Push to...                              │
│ ⭐ Push (Force)      ← SELECCIONAR ESTA │
│ Pull (Rebase)                           │
│ Pull (Merge)                            │
│ Sync (Force)                            │
└─────────────────────────────────────────┘
```

Click en **"Push (Force)"**

### PASO 5: Esperar Autenticación (si se pide)

VS Code puede mostrar:

```
┌─────────────────────────────────────────┐
│ Authenticate with GitHub                │
│ https://github.com              [↗]     │
│                                         │
│ A browser window will open to log in.   │
│                                         │
│     [Sign in with GitHub]               │
│     [Or use an existing token]          │
└─────────────────────────────────────────┘
```

**Opciones**:
- Click "Sign in with GitHub" (abre navegador)
- O click "Use existing token" si tienes uno

### PASO 6: Ver Progreso

VS Code mostrará:

```
┌─────────────────────────────────────────┐
│ ⏳ Publishing branch...                 │
│ ⏳ Pushing to origin                    │
│ ⏳ Authenticating...                    │
└─────────────────────────────────────────┘
```

Espera a que termine (usualmente 10-30 segundos).

### PASO 7: Confirmación de Éxito

Cuando termine, verás:

```
✅ Branch 'main' published successfully
```

O en el panel Source Control:

```
╔════════════════════════════════╗
║ SOURCE CONTROL                 ║
╠════════════════════════════════╣
║ 📌 Current Branch: main        ║
║    [up to date with origin]    ║  ← YA NO muestra números
║                                ║
│ ✅ Push completed successfully │
╚════════════════════════════════╝
```

---

## ✅ Verificación Post-Push

### En VS Code
1. Debería ver "main" sin número de commits adelante
2. Terminal mostrará: "Successfully published branch main"

### En GitHub (5-10 segundos después)
1. Ve a: https://github.com/gcarlossuarez/FormValidateLocalContest/commits/main
2. Verifica que aparezcan:
   - ✅ Commit 44d4fb8 (el limpio - arriba)
   - ✅ Commit 261c519 (debajo)
   - ✅ Commit 66bb200 (abajo)
3. Verifica que NO aparezcan:
   - ❌ Commit 15df803 (debe estar removido)
   - ❌ Commit ab22118 (debe estar removido)
   - ❌ Commit ae6d3e8 (debe estar removido)

---

## 🆘 Si Algo Sale Mal

### Problema: "Push (Force)" no aparece en menú

**Solución**:
1. Click en "Sync (Force)" en su lugar (hace lo mismo)
2. O intenta: `Ctrl+Shift+P` → tipo "Force Push" → Enter

### Problema: "Authentication failed"

**Solución**:
1. Click: `Ctrl+Shift+P` → tipo "Git: Sign Out"
2. Presiona Enter
3. Cierra VS Code completamente (Ctrl+Q)
4. Reabre VS Code: `code .`
5. Intenta force push nuevamente

### Problema: VS Code pide contraseña

**Solución**:
1. NO ingreses tu contraseña de GitHub
2. En su lugar, usa un Personal Access Token (PAT):
   - Ve a: https://github.com/settings/tokens
   - Genera un token nuevo
   - Usa el token como contraseña

### Problema: "Permission denied (403)"

**Solución**:
1. Cierra VS Code (`Ctrl+Q`)
2. Abre PowerShell
3. Ejecuta: `cmdkey /delete:git:https://github.com`
4. Abre VS Code nuevamente: `code .`
5. Intenta force push (VS Code pedirá nuevas credenciales)

---

## ⏱️ Resumen

| Paso | Acción | Tiempo |
|------|--------|--------|
| 1 | Ctrl+Shift+G (Source Control) | 5 seg |
| 2 | Ver panel | 2 seg |
| 3 | Click ⋯ (tres puntos) | 2 seg |
| 4 | Seleccionar "Push (Force)" | 2 seg |
| 5 | Autenticación (si se pide) | 10-30 seg |
| 6 | Ver progreso | 10-30 seg |
| 7 | Confirmación de éxito | 5 seg |
| **TOTAL** | | **1-2 min** |

---

## 🎉 Después de Completar

Una vez que veas "✅ Push completed successfully":

1. ✅ GitHub tiene historial limpio (sin credenciales)
2. ✅ El código está seguro
3. ✅ Los 3 commits viejos fueron reemplazados
4. ✅ Proyecto listo para colaboración

**Próxima acción (OPCIONAL)**: Revocar token viejo por seguridad
- Ve a: https://github.com/settings/tokens
- Encuentra el token que usaste
- Click "Delete"
- Confirma

---

*Guía Visual: Force Push desde VS Code*
*Última actualización: 2025-10-11*
