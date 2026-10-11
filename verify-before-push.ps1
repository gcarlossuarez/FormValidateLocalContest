# Script de verificación de seguridad antes de git push
# Uso: .\verify-before-push.ps1
# Retorna: 0 (OK) o 1 (ERROR - hay secretos detectados)

param([switch]$Force)

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Verificando cambios antes de push..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Obtener commits que serán pusheados
$unpushedCommits = git log origin/main..HEAD --oneline 2>$null
if (-not $unpushedCommits) {
    Write-Host "[OK] No hay cambios locales para pushear." -ForegroundColor Green
    exit 0
}

Write-Host "Commits por pushear:" -ForegroundColor Yellow
Write-Host $unpushedCommits
Write-Host ""

# Patrones de secretos a detectar (solo en archivos de código, no en .md)
$secretPatterns = @(
    @{ name = "GitHub Token"; pattern = "gh[po]_[A-Za-z0-9_]{36,255}" },
    @{ name = "Personal Access Token"; pattern = "github_pat_[A-Za-z0-9_]{82}" },
    @{ name = "Private Key"; pattern = "-----BEGIN (RSA|OPENSSH|EC|PGP) PRIVATE KEY-----" },
    @{ name = "AWS Access Key"; pattern = "AKIA[0-9A-Z]{16}" }
)

# Buscar secretos en archivos de código (C#, XML, config, env, etc)
# EXCLUIR: .md, .txt, docs/
$foundSecrets = @()

# Obtener archivos reales que cambiarán (sin .md, sin docs/)
$changedFiles = git diff origin/main HEAD --name-only 2>$null | Where-Object { $_ -notmatch '\.md$|\.txt$|^docs/' }

if ($changedFiles) {
    foreach ($pattern in $secretPatterns) {
        foreach ($file in $changedFiles) {
            $matches = git show "HEAD:$file" 2>$null | Select-String -Pattern $pattern.pattern -ErrorAction SilentlyContinue
            if ($matches) {
                $foundSecrets += @{
                    name = $pattern.name
                    file = $file
                    count = $matches.Count
                    samples = $matches | Select-Object -First 3
                }
            }
        }
    }
}

# Reportar resultados
if ($foundSecrets.Count -gt 0) {
    Write-Host "[⚠️  ADVERTENCIA] Se detectaron posibles secretos:" -ForegroundColor Red
    Write-Host ""
    foreach ($secret in $foundSecrets) {
        Write-Host "  - $($secret.name) en $($secret.file): $($secret.count) coincidencia(s)" -ForegroundColor Red
        $secret.samples | ForEach-Object { Write-Host "    - $_" -ForegroundColor DarkRed }
    }
    Write-Host ""
    
    if ($Force) {
        Write-Host "[FORZAR] Push continuará (--Force usado)." -ForegroundColor Yellow
        exit 0
    } else {
        Write-Host "[BLOQUEADO] No se permite push con secretos detectados." -ForegroundColor Red
        Write-Host "Solución: Elimina los secretos de los commits." -ForegroundColor Yellow
        Write-Host "O corre con -Force para ignorar esta verificación." -ForegroundColor Yellow
        Write-Host ""
        exit 1
    }
} else {
    Write-Host "[OK] No se detectaron secretos. Seguro para pushear." -ForegroundColor Green
    Write-Host ""
    exit 0
}
