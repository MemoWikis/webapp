---
name: dev-database-create
description: Create a fresh development MySQL database by running the ScenarioBuilder test to generate a new schema.sql with test data, transforming it, and reinitializing the MySQL container. Use this skill when the user wants to create or update the development database with the latest test scenario.
---

# Create Development Database

This skill automates the process of creating a fresh development MySQL database using the ScenarioBuilder test to generate current test data.

## What this skill does

1. **Runs the ScenarioBuilder test**: Executes `ScenarioBuilderTests.Deterministic_Tiny_Scenario()` to generate a fresh SQL dump with test data

2. **Copies the SQL dump file**: Takes the generated dump from `src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql` and copies it to `src/Docker/Dev/mysql-init/schema.sql`

3. **Replaces the database name**: Changes all occurrences of `memoWikisTest` to `memoWikis_dev` in the schema file

4. **Reinitializes the MySQL container**: Stops the Docker containers, removes the MySQL data directory, and restarts the containers

## Step-by-step procedure

### Step 1: Run the ScenarioBuilder test

Execute the test that generates the SQL dump:

```powershell
dotnet test "src/Tests/Tests.csproj" --filter "FullyQualifiedName~ScenarioBuilderTests.Deterministic_Tiny_Scenario"
```

This will create the file `src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql`.

### Step 2: Copy and transform the SQL file

Read the source SQL file and replace the database name:

```powershell
# Read the SQL dump file
$sqlContent = Get-Content -Path "src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql" -Raw

# Replace database name from memoWikisTest to memoWikis_dev
$sqlContent = $sqlContent -replace 'memoWikisTest', 'memoWikis_dev'

# Write to the Docker init directory
$sqlContent | Set-Content -Path "src/Docker/Dev/mysql-init/schema.sql" -Encoding UTF8
```ScenarioBuilder test will regenerate the SQL dump with the latest test data structure
- The MySQL container will automatically execute `schema.sql` during initialization
- The database name in the SQL file must match the `MYSQL_DATABASE` value in the `.env` file (default: `memoWikis_dev`)

## When to use this skill

- When you need to create a new development database with the latest test data
- After making changes to the ScenarioBuilder test and need to update the schema
- When the database schema has been updated in code and you need to regenerate the schema.sql
- When you want to ensure the development database matches the current test scenario structure

## Difference from dev-database-reset

- **dev-database-create**: Runs the ScenarioBuilder test to generate a fresh `schema.sql` with current test data, then creates the database
- **dev-database-reset**: Uses the existing `schema.sql` file as-is to recreate the database
```bash
cd ./src/Docker/Dev
docker-compose down
sudo rm -rf /var/lib/mysql/development  # Adjust path if you use a different volume mount
docker-compose up -d
```

## Important notes

- The MySQL data directory `C:\mysql-data\development` will be **completely deleted**
- The MySQL container will automatically execute the `schema.sql` file from `./src/Docker/Dev/mysql-init/` during initialization
- The database name in the SQL file must match the `MYSQL_DATABASE` value in the `.env` file (default: `memoWikis_dev`)
- The `schema.sql` file must already exist in the `./src/Docker/Dev/mysql-init/` directory

## When to use this skill

- When you need to create a new development database from scratch
- After manually updating the `schema.sql` file and need to apply the changes
- When you want to recreate the database without updating from the test scenario
- When the development database needs to be reinitialized with the current schema

## Difference from dev-database-reset

- **dev-database-create**: Uses the existing `schema.sql` file as-is to create the database
- **dev-database-reset**: First copies and transforms the test scenario SQL dump, then creates the database with test data
