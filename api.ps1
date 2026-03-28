<#
.SYNOPSIS
Robust local .NET API control script for memoWikis Backend.

.DESCRIPTION
Manages the local ASP.NET Core Backend API in a deterministic way.
Provides start, stop, restart, and status semantics.
Uses PID file and health endpoint to ensure accuracy.
Starts with 'dotnet watch' for hot-reload during development.

.PARAMETER Command
The action to perform: start, stop, restart, or status.

.EXAMPLE
.\api.ps1 start
.\api.ps1 stop
.\api.ps1 restart
.\api.ps1 status
#>

param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("start", "stop", "restart", "status")]
    [string]$Command
)

$Port = 5069
$BaseUrl = "http://localhost:$Port"
$HealthUrl = "$BaseUrl/healthcheck_backend"

$ApiDir = "$PSScriptRoot\src\Backend.Api"
$ProjectFile = "$ApiDir\Backend.Api.csproj"
$PidFile = "$PSScriptRoot\.api.pid"
$LogFile = "$PSScriptRoot\.api.log"

function Get-ApiStatus {
    $status = @{
        IsRunning = $false
        IsHealthy = $false
        Pid       = $null
        Message   = ""
    }

    if (Test-Path $PidFile) {
        $storedPid = Get-Content $PidFile -ErrorAction SilentlyContinue
        if ($storedPid -and $storedPid -match '^\d+$') {
            $process = Get-Process -Id $storedPid -ErrorAction SilentlyContinue
            if ($process) {
                $status.IsRunning = $true
                $status.Pid = $storedPid
            }
        }
    }

    # Fallback: check if something is listening on the port even without a PID file
    if (-not $status.IsRunning) {
        $connections = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue | Where-Object State -eq 'Listen'
        if ($connections) {
            $status.IsRunning = $true
            $status.Pid = $connections[0].OwningProcess
        }
    }

    if ($status.IsRunning) {
        try {
            $request = [System.Net.WebRequest]::Create($HealthUrl)
            $request.Timeout = 2000
            $response = $request.GetResponse()
            $statusCode = [int]$response.StatusCode
            if ($statusCode -eq 200) {
                $status.IsHealthy = $true
                $status.Message = "running and healthy (PID: $($status.Pid))"
            }
            else {
                $status.Message = "running but returned status $statusCode"
            }
            $response.Close()
        }
        catch {
            $status.Message = "running but unhealthy (health endpoint failed: $($_.Exception.Message))"
        }
    }
    else {
        $status.Message = "stopped"
    }

    return $status
}

function Stop-Api {
    $status = Get-ApiStatus
    if ($status.IsRunning -and $status.Pid) {
        Write-Host "Stopping API (PID: $($status.Pid))..." -ForegroundColor Yellow
        Stop-Process -Id $status.Pid -Force -ErrorAction SilentlyContinue
        Start-Sleep -Milliseconds 500
    }

    # Aggressive cleanup: kill anything still listening on the port
    $connections = Get-NetTCPConnection -LocalPort $Port -ErrorAction SilentlyContinue | Where-Object State -eq 'Listen'
    if ($connections) {
        foreach ($conn in $connections) {
            $process = Get-Process -Id $conn.OwningProcess -ErrorAction SilentlyContinue
            if ($process) {
                Write-Host "Force killing lingering process $($process.Name) (PID: $($process.Id)) on port $Port..." -ForegroundColor Red
                Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
            }
        }
        Start-Sleep -Milliseconds 500
    }

    if (Test-Path $PidFile) {
        Remove-Item $PidFile -Force -ErrorAction SilentlyContinue
    }
    Write-Host "API stopped." -ForegroundColor Green
}

function Start-Api {
    $status = Get-ApiStatus
    if ($status.IsRunning) {
        if ($status.IsHealthy) {
            Write-Host "API is already running and healthy (PID: $($status.Pid))." -ForegroundColor Green
            return
        }
        else {
            Write-Host "API is running but unhealthy. Stopping first..." -ForegroundColor Yellow
            Stop-Api
        }
    }

    Write-Host "Starting API with dotnet watch on $BaseUrl..." -ForegroundColor Cyan

    $watchArguments = @("watch", "run", "--project", "`"$ProjectFile`"")

    $process = Start-Process -FilePath "dotnet" `
        -ArgumentList $watchArguments `
        -WorkingDirectory $ApiDir `
        -PassThru `
        -NoNewWindow `
        -RedirectStandardOutput "$LogFile.out" `
        -RedirectStandardError "$LogFile.err"

    if ($process) {
        $process.Id | Out-File $PidFile -Encoding UTF8
        Write-Host "API process started with PID: $($process.Id). Waiting for health check..." -ForegroundColor Cyan

        # Wait up to 60 seconds for the API to become healthy
        $attempts = 0
        while ($attempts -lt 120) {
            Start-Sleep -Milliseconds 500
            $status = Get-ApiStatus
            if ($status.IsHealthy) {
                Write-Host "API is successfully started and healthy!" -ForegroundColor Green
                return
            }
            $attempts++
        }

        Write-Host "ERROR: API started but did not become healthy within 60 seconds." -ForegroundColor Red
        Write-Host "Check $LogFile.out and $LogFile.err for details." -ForegroundColor Red
        exit 1
    }
    else {
        Write-Host "Failed to start dotnet process." -ForegroundColor Red
        exit 1
    }
}

switch ($Command) {
    "status" {
        $status = Get-ApiStatus
        if ($status.IsHealthy) {
            Write-Host "API STATUS: $($status.Message)" -ForegroundColor Green
        }
        elseif ($status.IsRunning) {
            Write-Host "API STATUS: $($status.Message)" -ForegroundColor Yellow
        }
        else {
            Write-Host "API STATUS: $($status.Message)" -ForegroundColor Gray
        }
    }
    "start" {
        Start-Api
    }
    "stop" {
        Stop-Api
    }
    "restart" {
        Stop-Api
        Start-Api
    }
}
