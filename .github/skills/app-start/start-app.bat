# Quick Start Batch File
# Dieses Script kann direkt doppelgeklickt werden, um die Anwendung zu starten

@echo off
cd /d "%~dp0..\..\..\"
powershell -NoProfile -ExecutionPolicy Bypass -File ".github\skills\app-start\start-app.ps1"
pause
