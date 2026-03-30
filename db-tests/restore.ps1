<#
.SYNOPSIS
    Restore a MySQL dump into the prod-test Docker container (fast, volume-mount based).

.DESCRIPTION
    This script starts the mysql-prod-test Docker container with the dump file's
    directory mounted into the container, waits for MySQL readiness, then runs
    the import INSIDE the container (no Docker stdin pipe). This is 10-20x faster
    than piping through Docker's stdin for multi-GB dump files.

    MySQL is configured with aggressive import-speed optimizations (skip-log-bin,
    performance-schema=OFF, large buffers, etc.) via docker-compose.yml.

.PARAMETER DumpFilePath
    Path to the .sql dump file to import. Defaults to 'C:\Files\dump.sql'.

.PARAMETER Fresh
    If set, removes the existing prod-test MySQL data directory before starting
    the container, forcing a clean database initialization.

.EXAMPLE
    .\restore.ps1
    .\restore.ps1 -DumpFilePath "D:\dumps\memowikis_prod_20260301.sql"
    .\restore.ps1 -DumpFilePath "D:\dumps\dump.sql" -Fresh
#>

param(
    [string]$DumpFilePath = 'C:\Files\dump.sql',
    [switch]$Fresh
)

# ──────────────────────────────────────────
# Ensure docker stderr warnings don't become terminating errors
# (VS Code terminals often set $ErrorActionPreference = 'Stop')
$ErrorActionPreference = 'Continue'

# ──────────────────────────────────────────
# Configuration
[string]$dockerComposeDirectory = Join-Path $PSScriptRoot '..\src\Docker\Dev'
[string]$containerName = 'mem-mysql-prod-test'
[string]$databaseUsername = 'root'
[string]$databasePassword = 'root'
[string]$databaseName = 'memowikis_prod'
[string]$mysqlDataDirectory = 'C:\mysql-data\prod-test'
[int]$mysqlReadinessTimeoutSeconds = 300
[int]$mysqlReadinessIntervalSeconds = 3

# ──────────────────────────────────────────
# Preconditions
if (-not (Test-Path -Path $DumpFilePath)) {
    throw "Dump file not found: $DumpFilePath"
}

$dumpFileSize = (Get-Item -Path $DumpFilePath).Length
Write-Host ("Dump file       : $DumpFilePath")
Write-Host ("Dump file size  : {0:N2} GB ({1:N0} bytes)" -f ($dumpFileSize / 1GB), $dumpFileSize)

# ──────────────────────────────────────────
# Fresh start: remove data directory if requested
if ($Fresh) {
    Write-Host "`nFresh mode: removing existing MySQL data directory..."
    Push-Location $dockerComposeDirectory
    try {
        docker compose --profile prod-test stop mysql-prod-test 2>&1 | ForEach-Object { Write-Host $_ }
    }
    finally {
        Pop-Location
    }

    if (Test-Path -Path $mysqlDataDirectory) {
        Remove-Item -Recurse -Force $mysqlDataDirectory
        Write-Host "Removed: $mysqlDataDirectory"
    }
}

# ──────────────────────────────────────────
# Ensure the prod-test container is running (with dump directory mounted)
Write-Host "`nStarting prod-test container..."

# Set the import directory so docker-compose mounts the dump file's parent dir
$dumpDirectory = Split-Path -Parent (Resolve-Path $DumpFilePath)
$dumpFileName = Split-Path -Leaf $DumpFilePath
$env:MYSQL_IMPORT_DIR = $dumpDirectory -replace '\\', '/'

Write-Host ("Import mount    : $dumpDirectory -> /import (read-only)")

Push-Location $dockerComposeDirectory
try {
    docker compose --profile prod-test up -d mysql-prod-test 2>&1 | ForEach-Object { Write-Host $_ }
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to start mysql-prod-test container (exit code: $LASTEXITCODE)"
    }
}
finally {
    Pop-Location
}

# ──────────────────────────────────────────
# Helper function: run docker command via .NET Process to avoid
# PowerShell stderr-as-error issues with docker CLI warnings
function Invoke-DockerCommand {
    param(
        [string]$Arguments,
        [string]$StdinInput = $null
    )
    $dockerPath = (Get-Command docker).Source
    $startInfo = New-Object System.Diagnostics.ProcessStartInfo
    $startInfo.FileName = $dockerPath
    $startInfo.Arguments = $Arguments
    $startInfo.UseShellExecute = $false
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    $startInfo.RedirectStandardInput = ($null -ne $StdinInput)
    $startInfo.CreateNoWindow = $true

    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $startInfo
    $null = $process.Start()

    if ($null -ne $StdinInput) {
        $process.StandardInput.Write($StdinInput)
        $process.StandardInput.Close()
    }

    $stdout = $process.StandardOutput.ReadToEnd()
    $stderr = $process.StandardError.ReadToEnd()
    $process.WaitForExit()

    return @{
        Output   = $stdout
        Error    = $stderr
        ExitCode = $process.ExitCode
    }
}

# ──────────────────────────────────────────
# Wait for MySQL to be ready
Write-Host "Waiting for MySQL readiness (timeout: ${mysqlReadinessTimeoutSeconds}s)..."
$readinessStartTime = Get-Date
$mysqlIsReady = $false

while (((Get-Date) - $readinessStartTime).TotalSeconds -lt $mysqlReadinessTimeoutSeconds) {
    $pingResult = Invoke-DockerCommand -Arguments "exec $containerName mysqladmin ping -u$databaseUsername -p$databasePassword"
    if ($pingResult.Output -match 'mysqld is alive') {
        $mysqlIsReady = $true
        break
    }
    Write-Host "  MySQL not ready yet, retrying in ${mysqlReadinessIntervalSeconds}s..."
    Start-Sleep -Seconds $mysqlReadinessIntervalSeconds
}

if (-not $mysqlIsReady) {
    throw "MySQL did not become ready within ${mysqlReadinessTimeoutSeconds} seconds."
}
Write-Host "MySQL is ready."

# ──────────────────────────────────────────
# Apply import optimizations and create database if needed
Write-Host "`nPreparing database for import..."
$prepareStatements = @"
SET GLOBAL innodb_flush_log_at_trx_commit = 0;
SET GLOBAL foreign_key_checks = 0;
SET GLOBAL unique_checks = 0;
CREATE DATABASE IF NOT EXISTS ``$databaseName``;
"@

$prepareResult = Invoke-DockerCommand -Arguments "exec -i $containerName mysql -u$databaseUsername -p$databasePassword" -StdinInput $prepareStatements
if ($prepareResult.Output) { Write-Host $prepareResult.Output }
if ($prepareResult.Error -and $prepareResult.Error -notmatch 'Using a password') { Write-Host $prepareResult.Error }

# ──────────────────────────────────────────
# Verify the dump file is accessible inside the container
Write-Host "`nVerifying dump file is accessible inside container..."
$verifyResult = Invoke-DockerCommand -Arguments "exec $containerName ls -la /import/$dumpFileName"
if ($verifyResult.ExitCode -ne 0) {
    throw "Dump file not accessible inside container at /import/$dumpFileName. Check volume mount."
}
Write-Host ("  Found: " + $verifyResult.Output.Trim())

# ──────────────────────────────────────────
# Import the dump INSIDE the container (no Docker stdin pipe!)
# This reads directly from the mounted volume - 10-20x faster than piping.
Write-Host "`nStarting import (in-container, volume-mount)..."
$importStartTime = Get-Date
Write-Host "Import started  : $importStartTime"

# Build bash command that runs inside the container:
# Use shell redirect + --init-command for session optimizations.
# This reads the dump directly from the mounted volume (no Docker pipe).
$bashCommand = "mysql -u$databaseUsername -p$databasePassword --max-allowed-packet=256M --init-command=`'SET foreign_key_checks=0; SET unique_checks=0; SET autocommit=0;`' $databaseName < /import/$dumpFileName"

# Run the import as a background docker exec so we can monitor progress
$dockerExecutable = (Get-Command docker).Source
$processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
$processStartInfo.FileName = $dockerExecutable
$processStartInfo.Arguments = "exec $containerName bash -c `"$bashCommand`""
$processStartInfo.UseShellExecute = $false
$processStartInfo.RedirectStandardOutput = $true
$processStartInfo.RedirectStandardError = $true
$processStartInfo.CreateNoWindow = $true

$importProcess = New-Object System.Diagnostics.Process
$importProcess.StartInfo = $processStartInfo

# Capture stderr asynchronously
$stderrBuilder = New-Object System.Text.StringBuilder
$importProcess.add_ErrorDataReceived({
        param($sender, $eventArgs)
        if ($eventArgs.Data) {
            [void]$stderrBuilder.AppendLine($eventArgs.Data)
        }
    })

$null = $importProcess.Start()
$importProcess.BeginErrorReadLine()

# Read stdout asynchronously to prevent buffer deadlock
$stdoutBuilder = New-Object System.Text.StringBuilder
$importProcess.add_OutputDataReceived({
        param($sender, $eventArgs)
        if ($eventArgs.Data) {
            [void]$stdoutBuilder.AppendLine($eventArgs.Data)
        }
    })
$importProcess.BeginOutputReadLine()

# Monitor progress by polling table count
Write-Host "Reading dump directly from /import/$dumpFileName inside container..."
$lastTableCount = 0
while (-not $importProcess.HasExited) {
    Start-Sleep -Seconds 10

    $elapsed = (Get-Date) - $importStartTime
    $elapsedFormatted = "{0:hh\:mm\:ss}" -f $elapsed

    try {
        $tableCountResult = Invoke-DockerCommand -Arguments "exec $containerName mysql -u$databaseUsername -p$databasePassword -N -s -e `"SELECT COUNT(*) FROM information_schema.TABLES WHERE TABLE_SCHEMA = '$databaseName';`""
        $currentTableCount = [int]($tableCountResult.Output.Trim())
    }
    catch {
        $currentTableCount = $lastTableCount
    }

    if ($currentTableCount -ne $lastTableCount) {
        Write-Host ("  [{0}] Tables created: {1}" -f $elapsedFormatted, $currentTableCount)
        $lastTableCount = $currentTableCount
    }
    else {
        Write-Host ("  [{0}] Import running... (tables: {1})" -f $elapsedFormatted, $currentTableCount)
    }
}

# ──────────────────────────────────────────
# Restore global settings
Write-Host "`nRestoring global MySQL settings..."
$restoreStatements = @"
SET GLOBAL innodb_flush_log_at_trx_commit = 1;
SET GLOBAL foreign_key_checks = 1;
SET GLOBAL unique_checks = 1;
"@
$restoreResult = Invoke-DockerCommand -Arguments "exec -i $containerName mysql -u$databaseUsername -p$databasePassword" -StdinInput $restoreStatements
if ($restoreResult.Output) { Write-Host $restoreResult.Output }
if ($restoreResult.Error -and $restoreResult.Error -notmatch 'Using a password') { Write-Host $restoreResult.Error }

# ──────────────────────────────────────────
# Report results
$importEndTime = Get-Date
$importElapsed = $importEndTime - $importStartTime
$exitCode = $importProcess.ExitCode

if ($exitCode -ne 0) {
    Write-Warning "mysql import exited with code $exitCode"
    $stderrText = $stderrBuilder.ToString().Trim()
    if ($stderrText -and $stderrText -notmatch '^\s*mysql: \[Warning\] Using a password') {
        Write-Warning "Error output:"
        Write-Host $stderrText
    }
}
else {
    Write-Host "`nImport completed successfully!"
}

Write-Host ""
Write-Host "Import finished : $importEndTime"
Write-Host ("Elapsed time    : {0:hh\:mm\:ss}" -f $importElapsed)
Write-Host ("Throughput      : {0:N0} MB/s" -f (($dumpFileSize / $importElapsed.TotalSeconds) / 1MB))
Write-Host ""
Write-Host "Connection info:"
Write-Host "  Host     : localhost"
Write-Host "  Port     : 3307"
Write-Host "  Database : $databaseName"
Write-Host "  User     : admin / admin (or root / root)"
Write-Host ""
Write-Host "To use this database in the app, update appsettings.Development.json:"
Write-Host '  "ConnectionString": "Server=localhost;Port=3307;Database=memowikis_prod;uid=admin;pwd=admin"'
Write-Host ""
Write-Host "TIP: To skip the import next time, just start the container:"
Write-Host "  cd src\Docker\Dev; docker compose --profile prod-test up -d mysql-prod-test"
Write-Host "  The data persists in $mysqlDataDirectory"

exit $exitCode
