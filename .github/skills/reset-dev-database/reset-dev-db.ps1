# Reset Development Database Script
# This script copies the test scenario SQL dump to the Docker init directory,
# replaces the database name, and reinitializes the MySQL container.

param(
    [string]$WorkspaceRoot = (Get-Location).Path,
    [string]$MysqlDataPath = "C:\mysql-data\development"
)

$ErrorActionPreference = "Stop"

Write-Host "=== Reset Development Database ===" -ForegroundColor Cyan

# Define paths
$sourceSqlPath = Join-Path $WorkspaceRoot "src\Tests\TestData\Dumps\memowikis-test-scenario_tiny.sql"
$targetSqlPath = Join-Path $WorkspaceRoot "src\Docker\Dev\mysql-init\schema.sql"
$dockerDevPath = Join-Path $WorkspaceRoot "src\Docker\Dev"

# Step 1: Check if source file exists
if (-not (Test-Path $sourceSqlPath)) {
    Write-Error "Source SQL file not found: $sourceSqlPath"
    exit 1
}

Write-Host "Step 1: Reading SQL dump from $sourceSqlPath" -ForegroundColor Yellow

# Step 2: Read and transform SQL content
$sqlContent = Get-Content -Path $sourceSqlPath -Raw
$sqlContent = $sqlContent -replace 'memoWikisTest', 'memoWikis_dev'

Write-Host "Step 2: Replaced 'memoWikisTest' with 'memoWikis_dev'" -ForegroundColor Yellow

# Step 3: Write to target location
$sqlContent | Set-Content -Path $targetSqlPath -Encoding UTF8
Write-Host "Step 3: Written schema to $targetSqlPath" -ForegroundColor Yellow

# Step 4: Stop Docker containers
Write-Host "Step 4: Stopping Docker containers..." -ForegroundColor Yellow
Push-Location $dockerDevPath
try {
    docker-compose down
    
    # Step 5: Remove MySQL data directory
    Write-Host "Step 5: Removing MySQL data directory: $MysqlDataPath" -ForegroundColor Yellow
    if (Test-Path $MysqlDataPath) {
        Remove-Item -Recurse -Force $MysqlDataPath
        Write-Host "  MySQL data directory removed." -ForegroundColor Green
    } else {
        Write-Host "  MySQL data directory does not exist, skipping removal." -ForegroundColor Gray
    }
    
    # Step 6: Start Docker containers
    Write-Host "Step 6: Starting Docker containers..." -ForegroundColor Yellow
    docker-compose up -d
    
    Write-Host ""
    Write-Host "=== Development database reset complete! ===" -ForegroundColor Green
    Write-Host "The MySQL container will initialize with the new schema." -ForegroundColor Cyan
}
finally {
    Pop-Location
}
