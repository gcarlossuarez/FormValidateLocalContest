@echo off
REM Script para que agentes de IA hagan push seguro
REM Verifica secretos ANTES de pushear
REM Uso: safe-push.cmd [--force]

setlocal enabledelayedexpansion

echo.
echo ========================================
echo [AGENTE] Push Seguro con Verificacion
echo ========================================
echo.

REM Ejecutar verificación de secretos
powershell -NoProfile -ExecutionPolicy Bypass -File "verify-before-push.ps1" %*

if !errorlevel! neq 0 (
    echo.
    echo [BLOQUEADO] Verificacion fallida. No se permite push.
    echo.
    pause
    exit /b 1
)

REM Si verificacion paso, proceder con push
echo.
echo [AGENTE] Verificacion exitosa. Procediendo con push...
echo.

REM Stage, commit, pull, push
git add .
git commit -m "Auto sync and push" 2>nul

git pull --rebase origin main 2>nul

echo Subiendo cambios...
git push -u origin main

if !errorlevel! neq 0 (
    echo.
    echo [ERROR] Push fallido.
    exit /b 1
) else (
    echo.
    echo [OK] Push completado exitosamente.
    echo.
)
