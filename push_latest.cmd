@echo off
REM Push all local changes to GitHub (auto-sync)

REM Stage all changes
git add .

REM Commit with a generic message (edit if needed)
git commit -m "Auto sync and push"

REM Pull and rebase from remote to avoid conflicts
git pull --rebase origin main

REM Push to GitHub
git push -u origin main

pause
