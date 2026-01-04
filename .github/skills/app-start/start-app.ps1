# app-start.ps1
# Checks if Backend and Frontend are running and reports status
# Actual terminal creation is handled by Copilot's run_in_terminal tool

param(
    [string]$WorkspacePath = "c:\Projects\memoWikis"
)

# Return codes for Copilot to handle
$script:StartBackend = $false
$script:StartFrontend = $false

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  app-start - Start Backend & Frontend  " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Define paths
$backendPath = Join-Path $WorkspacePath "src\Backend.Api"
$frontendPath = Join-Path $WorkspacePath "src\Frontend.Nuxt"

# Helper function: Fast port check using netstat
function Test-PortInUse {
    param(
        [int]$Port
    )
    
    try {
        $netstat = netstat -ano | Select-String ":$Port " | Select-String "LISTENING"
        return $null -ne $netstat
    }
    catch {
        return $false
    }
}

# Backend Check
Write-Host "Checking Backend (Port 5069)..." -ForegroundColor Yellow
$backendRunning = Test-PortInUse -Port 5069

if ($backendRunning) {
    Write-Host "[OK] Backend is already running on port 5069" -ForegroundColor Green
}
else {
    Write-Host "[ACTION] Backend needs to be started" -ForegroundColor Yellow
    $script:StartBackend = $true
}

Write-Host ""

# Frontend Check  
Write-Host "Checking Frontend (Port 3000)..." -ForegroundColor Yellow
$frontendRunning = Test-PortInUse -Port 3000

if ($frontendRunning) {
    Write-Host "[OK] Frontend is already running on port 3000" -ForegroundColor Green
}
else {
    Write-Host "[ACTION] Frontend needs to be started" -ForegroundColor Yellow
    $script:StartFrontend = $true
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Status" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Backend:  $(if ($backendRunning) { '[OK] Running' } else { '[*] Will start in VS Code Terminal' })" -ForegroundColor $(if ($backendRunning) { 'Green' } else { 'Yellow' })
Write-Host "Frontend: $(if ($frontendRunning) { '[OK] Running' } else { '[*] Will start in VS Code Terminal' })" -ForegroundColor $(if ($frontendRunning) { 'Green' } else { 'Yellow' })
Write-Host ""
Write-Host "Backend:  http://localhost:5069" -ForegroundColor Cyan
Write-Host "Frontend: http://localhost:3000" -ForegroundColor Cyan
Write-Host ""

# Output JSON for Copilot to parse
$result = @{
    startBackend = $script:StartBackend
    startFrontend = $script:StartFrontend
    backendPath = $backendPath
    frontendPath = $frontendPath
} | ConvertTo-Json -Compress

Write-Host "COPILOT_ACTION:$result" -ForegroundColor DarkGray