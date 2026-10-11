@echo off
REM Push all local changes to GitHub (auto-sync)
setlocal enabledelayedexpansion

echo.
echo ========================================
echo Sincronizando cambios con GitHub...
echo ========================================

REM Stage all changes
echo [1/4] Preparando cambios...
git add .

REM Commit with a generic message
echo [2/4] Confirmando cambios...
git commit -m "Auto sync and push" 2>nul
if !errorlevel! equ 1 echo (sin cambios nuevos)

REM Pull and rebase from remote to avoid conflicts
echo [3/4] Actualizando desde remote...
git pull --rebase origin main 2>nul

REM Push to GitHub
echo [4/4] Subiendo a GitHub...
git push -u origin main

if !errorlevel! neq 0 (
    echo.
    echo [ERROR] Push fallido. Necesitas configurar credenciales.
    echo.
    echo Opcion 1 - Usar SSH (Recomendado):
    echo   git remote set-url origin git@github.com:gcarlossuarez/FormValidateLocalContest.git
    echo   (Requiere tener SSH keys configuradas)
    echo.
    echo Opcion 2 - Guardar credenciales en .netrc:
    echo   Ejecuta en PowerShell (como administrador):
    echo   @'
    echo   machine github.com
    echo   login gcarlossuarez
    echo   password TU_GITHUB_TOKEN
    echo   '@ ^| Out-File -FilePath $env:USERPROFILE\.netrc -Encoding ASCII
    echo   (Reemplaza TU_GITHUB_TOKEN con tu Personal Access Token)
    echo.
) else (
    echo.
    echo [OK] Push completado exitosamente!
    echo.
)

pause
