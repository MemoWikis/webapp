---
name: reset-dev-database
description: Reset the development MySQL database by copying the test scenario SQL dump to the Docker init directory, replacing the database name from memoWikisTest to memoWikis_dev, and reinitializing the MySQL container. Use this skill when the user wants to reset, recreate, or reinitialize the development database.
---

# Reset Development Database

This skill automates the process of resetting the development MySQL database using the test scenario SQL dump.

## What this skill does

1. **Copies the SQL dump file**: Takes the test scenario dump from `src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql` and copies it to `src/Docker/Dev/mysql-init/schema.sql`

2. **Replaces the database name**: Changes all occurrences of `memoWikisTest` to `memoWikis_dev` in the schema file

3. **Reinitializes the MySQL container**: Stops the Docker containers, removes the MySQL data directory, and restarts the containers

## Step-by-step procedure

### Step 1: Copy and transform the SQL file

Read the source SQL file and replace the database name:

```powershell
# Read the SQL dump file
$sqlContent = Get-Content -Path "src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql" -Raw

# Replace database name from memoWikisTest to memoWikis_dev
$sqlContent = $sqlContent -replace 'memoWikisTest', 'memoWikis_dev'

# Write to the Docker init directory
$sqlContent | Set-Content -Path "src/Docker/Dev/mysql-init/schema.sql" -Encoding UTF8
```

### Step 2: Reinitialize the MySQL container

Run the following PowerShell commands from the workspace root:

```powershell
cd ./src/Docker/Dev; `
docker-compose down; `
Remove-Item -Recurse -Force C:\mysql-data\development; `
docker-compose up -d
```

## Important notes

- The MySQL data directory `C:\mysql-data\development` will be **completely deleted**
- The MySQL container will automatically execute `schema.sql` during initialization
- The database name in the SQL file must match the `MYSQL_DATABASE` value in the `.env` file (default: `memoWikis_dev`)

## When to use this skill

- When you need to reset the development database to a clean state
- After running the test `ScenarioBuilderTests.Deterministic_Tiny_Scenario()` to update the schema
- When the database schema has been updated and you need to apply changes
- When the development database is corrupted or in an inconsistent state
