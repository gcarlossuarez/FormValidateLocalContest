@echo off
REM Push all local changes to GitHub (auto-sync)
setlocal enabledelayedexpansion

REM Stage all changes
git add .

REM Commit with a generic message (edit if needed)
git commit -m "Auto sync and push"

REM Pull and rebase from remote to avoid conflicts
git pull --rebase origin main

REM Push to GitHub (use credential manager or SSH)
git push -u origin main

if !errorlevel! neq 0 (
    echo.
    echo [ERROR] Push fallido. Opciones:
    echo 1. Configura Git Credential Manager: git config --global credential.helper manager-core
    echo 2. O usa SSH key: git remote set-url origin git@github.com:gcarlossuarez/FormValidateLocalContest.git
    echo 3. O copia tu GitHub token y pegalo cuando lo pida
    pause
) else (
    echo.
    echo [OK] Push exitoso!
)

pause
