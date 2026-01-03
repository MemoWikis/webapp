---
name: dev-database-create
description: Create a fresh development MySQL database by running the ScenarioBuilder test to generate a new schema.sql with test data, transforming it, and reinitializing the MySQL container. Use this skill when the user wants to create or update the development database with the latest test scenario.
---

# Create Development Database

This skill automates the process of creating a fresh development MySQL database using the ScenarioBuilder test to generate current test data.

## What this skill does

1. **Runs the ScenarioBuilder test**: Executes `ScenarioBuilderTests.Deterministic_Tiny_Scenario()` to generate a fresh SQL dump with test data from NHibernate mappings

2. **Copies the SQL dump file**: Takes the generated dump from `src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql` and copies it to `src/Docker/Dev/mysql-init/schema.sql`

3. **Replaces the database name**: Changes all occurrences of `memoWikisTest` to `memoWikis_dev` in the schema file

4. **Reinitializes the MySQL container**: Stops the Docker containers, removes the MySQL data directory, and restarts the containers

## Step-by-step procedure

### Step 1: Run the ScenarioBuilder test

Execute the test that generates the SQL dump from current NHibernate mappings:

```powershell
cd c:\Projects\memoWikis
dotnet test "src/Tests/Tests.csproj" --filter "FullyQualifiedName~ScenarioBuilderTests.Deterministic_Tiny_Scenario"
```

This will create the file `src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql`.

### Step 2: Copy and transform the SQL file

Read the source SQL file, replace the database name, and remove the FK_PageViewToPage constraint:

```powershell
# Read the SQL dump file
$sqlContent = Get-Content -Path "src/Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql" -Raw

# Replace database name from memoWikisTest to memoWikis_dev
$sqlContent = $sqlContent -replace 'memoWikisTest', 'memoWikis_dev'

# Remove FK_PageViewToPage constraint
# This FK must be removed because pageviews are kept for statistics even after page deletion.
# Despite the NHibernate mapping specifying .Not.ForeignKey(), the constraint is still created
# by NHibernate during schema generation (reason unclear, possibly a FluentNHibernate bug).
# The constraint prevents page deletion with: "Cannot delete or update a parent row: 
# a foreign key constraint fails (`memowikis_dev`.`pageview`, CONSTRAINT `FK_PageViewToPage`..."
$sqlContent = $sqlContent -replace ',\s*CONSTRAINT `FK_PageViewToPage` FOREIGN KEY \(`Page_id`\) REFERENCES `page` \(`Id`\)', ''

# Write to the Docker init directory
$sqlContent | Set-Content -Path "src/Docker/Dev/mysql-init/schema.sql" -Encoding UTF8
```

### Step 3: Reinitialize the MySQL container

Stop containers, remove MySQL data directory, and restart:

```powershell
cd src/Docker/Dev
docker-compose down
Remove-Item -Recurse -Force .\data\mysql -ErrorAction SilentlyContinue
docker-compose up -d
```

## Important notes

- The MySQL data directory will be **completely deleted**
- The ScenarioBuilder test will regenerate the SQL dump with the latest schema from NHibernate mappings
- The MySQL container will automatically execute `schema.sql` during initialization
- The database name in the SQL file must match the `MYSQL_DATABASE` value in the `.env` file (default: `memoWikis_dev`)
- The `FK_PageViewToPage` constraint must be manually removed despite `.Not.ForeignKey()` in the mapping (see Step 2 for explanation)
- For the first execution after mapping changes, you may need to delete reused test containers and volumes to force a fresh schema build

## When to use this skill

- When you need to create a new development database with the latest test data
- After making changes to NHibernate mappings and need to update the schema
- When the database schema has been updated in code and you need to regenerate the schema.sql
- When you want to ensure the development database matches the current test scenario structure

## Difference from dev-database-reset

- **dev-database-create**: Runs the ScenarioBuilder test to generate a fresh `schema.sql` with current test data, then creates the database
- **dev-database-reset**: Uses the existing `schema.sql` file as-is to recreate the database
