---
name: dev-database-reset
description: Reset the development MySQL database by removing the existing MySQL data directory and reinitializing the MySQL container with the current schema.sql file. Use this skill when the user wants to reset or recreate the development database from the existing schema.
---

# Reset Development Database

This skill automates the process of resetting the development MySQL database from the existing schema file.

## What this skill does

1. **Stops Docker containers**: Brings down all running Docker services

2. **Removes existing MySQL data**: Deletes the MySQL data directory to ensure a clean state

3. **Starts Docker containers**: Brings up the services again, which automatically initializes the database using the existing `schema.sql` file

## Step-by-step procedure

### Execute the database reset

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
For Linux/macOS:

```bash
cd ./src/Docker/Dev
docker-compose down
sudo rm -rf /var/lib/mysql/development  # Adjust path if you use a different volume mount
docker-compose up -d
```

## Important notes

- The MySQL data directory `C:\mysql-data\development` will be **completely deleted**
- The MySQL container will automatically execute the existing `schema.sql` file from `./src/Docker/Dev/mysql-init/` during initialization
- The database name in the SQL file must match the `MYSQL_DATABASE` value in the `.env` file (default: `memoWikis_dev`)
- The `schema.sql` file must already exist in the `./src/Docker/Dev/mysql-init/` directory

## When to use this skill

- When you need to reset the development database to the current schema state
- After manually editing the `schema.sql` file and need to apply changes
- When you want to recreate the database without regenerating the schema
- When the development database is corrupted or in an inconsistent state

## Difference from dev-database-create

- **dev-database-reset**: Uses the existing `schema.sql` file as-is to recreate the database
- **dev-database-create**: Runs the ScenarioBuilder test to generate a fresh `schema.sql` with current test data, then creates the databas