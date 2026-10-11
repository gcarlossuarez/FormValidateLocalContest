@echo off
REM Force Push to GitHub (limpia historial con credenciales)
REM Este script NO guarda el token - lo toma de variable de entorno

setlocal enabledelayedexpansion

REM Verificar si token está en variable de entorno
if "%GITHUB_TOKEN%"=="" (
    echo.
    echo [ERROR] GITHUB_TOKEN no configurada
    echo.
    echo Para usar este script:
    echo 1. Abre PowerShell o CMD
    echo 2. Ejecuta: set GITHUB_TOKEN=tu_token_aqui
    echo 3. Luego ejecuta este script
    echo.
    echo O en PowerShell:
    echo   $env:GITHUB_TOKEN = 'tu_token_aqui'
    echo   .\push_force.cmd
    echo.
    pause
    exit /b 1
)

cd /d "%~dp0FormValidateLocalContest"

echo.
echo ========================================
echo Force Push a GitHub (historial limpio)
echo ========================================
echo.

REM Ver estado
git log --oneline -3

echo.
echo Haciendo force push (removiendo historial con credenciales)...
echo.

REM Usar token desde variable de entorno (no guardado en archivo)
git push --force https://gcarlossuarez:%GITHUB_TOKEN%@github.com/gcarlossuarez/FormValidateLocalContest.git main

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] Force push completado exitosamente!
    echo.
    echo Los commits problemáticos han sido reemplazados en GitHub.
    echo.
) else (
    echo.
    echo [ERROR] Fallo el push. Verifica:
    echo - Token valido y con permisos 'repo'
    echo - Conexion a internet
    echo.
)

pause
