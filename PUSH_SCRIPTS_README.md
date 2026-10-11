# Scripts de Push a GitHub

Este directorio contiene scripts para facilitar hacer push de cambios a GitHub.

## Scripts Disponibles

### 1. `push_latest.cmd` (Original)

Script de propósito general que:
- ✅ Agrega todos los cambios: `git add .`
- ✅ Crea un commit automático: `git commit -m "Auto sync and push"`
- ✅ Sincroniza con remoto: `git pull --rebase origin main`
- ❌ Intenta hacer push: `git push -u origin main`

**Cuándo usar**: Para sincronización rápida manual desde terminal.

**Problema**: En entornos sin interactividad, falla en el paso de autenticación.

**Cómo usar**:
```cmd
double-click push_latest.cmd
```

---

### 2. `push_with_token.cmd` (Mejorado)

Script automatizado que soporta Personal Access Token (PAT) de GitHub.

**Ventajas**:
- ✅ Funciona en entornos sin interactividad
- ✅ Completamente automatizado
- ✅ Ideal para scripts y automatización
- ✅ Soporte para múltiples repositorios

**Configuración requerida**:
```batch
SET GITHUB_TOKEN=tu_token_aqui
SET GITHUB_USER=gcarlossuarez
SET GITHUB_REPO=FormValidateLocalContest
```

**Cómo usar**:

#### Paso 1: Generar Personal Access Token en GitHub
1. Ve a: GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click: "Generate new token (classic)"
3. Nombre: `FormValidateLocalContest-Push` (o el que prefieras)
4. Marca: ☑ repo (acceso completo al repositorio)
5. Click: "Generate token"
6. **COPIA el token** (solo aparecerá una vez)

#### Paso 2: Configurar el Script
1. Abre `push_with_token.cmd` con Notepad
2. Busca esta línea:
   ```batch
   SET GITHUB_TOKEN=tu_token_aqui
   ```
3. Reemplaza `tu_token_aqui` con el token que copiaste
4. Guarda el archivo

#### Paso 3: Ejecutar
```cmd
double-click push_with_token.cmd
```

O desde línea de comandos:
```cmd
cd D:\...\FormValidateLocalContest
push_with_token.cmd
```

---

## Alternativas (Sin Scripts)

### Opción 1: VS Code (Recomendado) ⭐⭐⭐

**Ventajas**:
- ✅ Más seguro (sin credenciales en archivos)
- ✅ Interfaz gráfica
- ✅ Manejo automático de credenciales Windows
- ✅ 30 segundos total

**Pasos**:
1. Abre VS Code
2. `Ctrl+Shift+G` (Source Control)
3. Click en "Push" (↑)
4. Ingresa credenciales si se pide

---

### Opción 2: PowerShell Interactiva

**Pasos**:
```powershell
$env:GITHUB_TOKEN = 'tu_token_aqui'
git push origin main
```

---

### Opción 3: Git CLI Directo

```bash
git push https://usuario:token@github.com/usuario/repo.git main
```

---

## Seguridad y Mejores Prácticas

### ⚠️ Importante

- **Nunca** compartir el token con terceros
- **Nunca** commitear archivos con tokens en Git
- **Revocar** token inmediatamente si se expone
- **Usar** tokens con expiración (30 días recomendado)
- **Scope** mínimo necesario (`repo` es estándar)

### Recomendaciones

1. **Para uso local**: Usa VS Code (más seguro)
2. **Para automatización**: Usa `push_with_token.cmd` con variable de entorno
3. **Para CI/CD**: Usa secretos de GitHub Actions, no tokens en archivos
4. **Para máquinas compartidas**: Usa SSH keys en lugar de tokens

---

## Troubleshooting

### Error: "fatal: Cannot prompt because user interactivity has been disabled"

**Causa**: Entorno de ejecución no permite interactividad.

**Soluciones**:
- ✅ Usa VS Code (interfaz gráfica)
- ✅ Usa `push_with_token.cmd` (automatizado)
- ✅ Abre PowerShell normal (no como script)

---

### Error: "invalid password"

**Causa**: Token inválido o expirado.

**Solución**:
1. Revoca el token en GitHub: Settings → Developer settings → Tokens → Delete
2. Genera uno nuevo
3. Reemplaza en el script

---

### Error: "Repository not found"

**Causa**: Usuario o repositorio incorrecto.

**Solución**:
- Verifica `GITHUB_USER` y `GITHUB_REPO` en el script
- Ejemplo correcto: `gcarlossuarez` y `FormValidateLocalContest`

---

## Referencia

- GitHub Docs: [Personal Access Tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token)
- Git Documentation: [git-push](https://git-scm.com/docs/git-push)
- VS Code: [Source Control](https://code.visualstudio.com/docs/sourcecontrol/overview)

---

## Historial

| Fecha | Script | Acción |
|-------|--------|--------|
| 2025-10-11 | push_latest.cmd | Ejecutado y testeado |
| 2025-10-11 | push_with_token.cmd | Creado como alternativa |

---

*Última actualización: 2025-10-11*
*Mantenedor: Copilot*
