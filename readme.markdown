# memoWikis <!-- omit from toc -->

We love **wikis**, and we believe that **knowledge management** is fundamental to achieving countless goals. Now, we’re reimagining both. If you’d like to use memoWikis, visit https://memoWikis.net. If you’re interested in hosting or improving it, check out the sections below.


## Knowledge management <!-- omit from toc -->
We view wikis as excellent knowledge-management tools, yet we’ve always believed there’s more to knowledge management than simply organizing documents. While document organization remains at its core, it’s equally important to understand—and visualize—what you already know. Providing the right tools and training to facilitate learning is core aspect of memoWikis.

# Table of Contents <!-- omit from toc -->


- [Hosting](#hosting)
- [Local Development](#local-development)
  - [Prerequisites / TechStack](#prerequisites--techstack)
  - [Setup](#setup)
    - [1. Clone the Repository](#1-clone-the-repository)
    - [2. Configure Environment Files \& Start Docker Services](#2-configure-environment-files--start-docker-services)
      - [On Linux / macOS (Bash):](#on-linux--macos-bash)
      - [On Windows (PowerShell):](#on-windows-powershell)
    - [3. Set Up Your Anthropc API Key](#3-set-up-your-anthropc-api-key)
  - [Running the Application](#running-the-application)
    - [Visual Studio / Rider](#visual-studio--rider)
    - [VS Code](#vs-code)
  - [Additional Tips](#additional-tips)
  - [Common Tasks](#common-tasks)
    - [Recreate Development Database](#recreate-development-database)
      - [On Windows (PowerShell):](#on-windows-powershell-1)
      - [On Linux / macOS (Bash):](#on-linux--macos-bash-1)
    - [Update the schema.sql File](#update-the-schemasql-file)
- [⚖️ License](#️-license)

# Hosting
We provide Docker images for the backend and frontend.

- https://github.com/MemoWikis/webapp/pkgs/container/mem-backend
- https://github.com/MemoWikis/webapp/pkgs/container/mem-nuxt

For configuration, see "Local development" for instructions. It is best to adjust the Docker Compose files to suit your needs. If you need further help, just get in contact 🙂.

If you have a publicly accessible memoWikis instance, we’d be happy to link your page here. We’re also curious to hear about your experiences, so please drop us a note!

# Local Development

This web application is built around a Nuxt 3 frontend and a .NET backend, complemented by additional services including Dockerized MySQL, Redis, Meilisearch, and Hocuspocus for Tiptap.

## Prerequisites / TechStack

- **Git** – for cloning the repository  
- **Docker & docker-compose** – to run the backend services  
- **Node.js & npm** – for the Nuxt 3 frontend  
- **Rider / Visual Studio** – for debugging the .NET backend  
- A terminal of your choice (e.g., PowerShell on Windows or Bash on Linux)

## Setup

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/memoWikis/webapp.git
cd webapp
```


### 2. Configure Environment Files & Start Docker Services
The project uses example configuration files that need to be copied to their corresponding development files.

#### On Linux / macOS (Bash):
```bash
cp ./Backend/appsettings.Development.json.example ./Backend/appsettings.Development.json
cp ./Docker/Dev/.env.example ./Docker/Dev/.env
cd ./Docker/Dev
docker-compose up -d
```
> [!NOTE] 
> #### What This Does:
> 
> - **Copies the example config files:**
>     - `appsettings.Development.json.example` → `appsettings.Development.json`
>     - `.env.example` → `.env`
> - **Starts the Docker services using `docker-compose up -d`, which include:**
>     - Hocuspocus (for Tiptap collaboration)
>     - Redis (for state management for Tiptap Hocuspocus)
>     - MySQL (running in Docker)
>     - Meilisearch (to power the search functionality)

#### On Windows (PowerShell):
```ppwershell
cp ./Backend/appsettings.Development.json.example ./Backend/appsettings.Development.json -Force; `
cp ./Docker/Dev/.env.example ./Docker/Dev/.env -Force; `
cd ./Docker/Dev; `
docker-compose up -d
```

### 3. Set Up Your Anthropc API Key
To enable the AI functions within the application, you must supply a valid Anthropc API key. Open the file `Backend/appsettings.Development.json` and update the **Anthropic** section with your API key as follows:

```json
{
  "Anthropic": {
    "ApiKey": "your-actual-api-key"
  }
}
```
> [!WARNING]  
> **Without a valid API key, the AI functions in the application will not work.**

## Running the Application

After completing the setup, you can run the application using your preferred IDE:

| Service  | URL                    | Command       |
|----------|------------------------|---------------|
| Backend  | http://localhost:5069  | `dotnet run`  |
| Frontend | http://localhost:3000  | `npm run dev` |

### Visual Studio / Rider

**Frontend:**
```bash
cd ./src/Frontend.Nuxt
npm install  # Run this if dependencies are not yet installed
npm run dev
```

**Backend:**
1. Open `memoWikis.sln` in Visual Studio or Rider
2. Select your preferred debug configuration
3. Start the Backend.Api debug session

> [!NOTE]  
> Make sure your Node.js version meets Nuxt 3 requirements.

### VS Code

VS Code users can start both services with a single action:

**Option 1: Run and Debug (F5)**
1. Press `F5` or open the "Run and Debug" sidebar
2. Select **"Backend + Frontend"** from the dropdown
3. Both services start in visible, dedicated terminals

**Option 2: GitHub Copilot**
Simply say `start app` or `Anwendung starten` in Copilot Chat.

**Option 3: Run Tasks Manually**
1. Press `Ctrl+Shift+P` → "Tasks: Run Task"
2. Select **"Backend"** or **"Frontend"**

## Additional Tips
- Anthropic API Key for AI Functions: Ensure your valid Anthropc API key is inserted in the appsettings.Development.json file. Without this key, the AI features will not work.

- Docker Management:
  - To check the status of your containers:
`bash
docker-compose ps
`
  - To view logs for any service:
     `bash
docker-compose logs [service-name]`
- Nuxt Configuration: For frontend customizations, review the `nuxt.config.ts` file in the `Frontend.Nuxt` folder.

## Common Tasks

### Recreate Development Database

To apply a new version of the SQL schema after you've completed the initial setup:

> [!IMPORTANT]
> **Precondition:** You must have completed the [Setup](#setup) steps above and have Docker services running.

> [!TIP]
> **GitHub Copilot Users:** You can use automated skills in Copilot to simplify common tasks:
> - `dev database reset` - Reset the database from the current schema.sql
> - `dev database create` - Generate a fresh database with the latest test data from ScenarioBuilder
> 
> Learn more about [GitHub Copilot Skills](https://code.visualstudio.com/docs/copilot/copilot-customization#_agent-skill-sets).

#### On Windows (PowerShell):
```powershell
cd ./src/Docker/Dev; `
docker-compose down; `
Remove-Item -Recurse -Force C:\mysql-data\development; `
docker-compose up -d
```

#### On Linux / macOS (Bash):
```bash
cd ./Docker/Dev
docker-compose down
sudo rm -rf /var/lib/mysql/development  # Adjust path if you use a different volume mount
docker-compose up -d
```

The MySQL container will automatically execute the `schema.sql` file from the `./Docker/Dev/mysql-init/` directory during initialization.

### Update the schema.sql File

The `schema.sql` file can be generated from the test suite:

1. **Run the test that creates the database dump:**
   
   Execute the test `ScenarioBuilderTests.Deterministic_Tiny_Scenario()` in your test runner.

2. **Copy the generated dump file:**
   
   After the test runs, a SQL dump file will be created in:
   ```
   /Tests/TestData/Dumps/memowikis-test-scenario_tiny.sql
   ```

3. **Replace and rename the database:**
   
   Copy the dump file to `./Docker/Dev/mysql-init/schema.sql` and update the database name inside the file from `memoWikisTest` to `memoWikis_dev`.

> [!TIP]
> **GitHub Copilot Users:** You can use the `dev-database-create` skill to automate this entire process. Just type `create dev database` in Copilot.
> 
> The database name in the SQL file must match the `MYSQL_DATABASE` value in your `.env` file (default: `memoWikis_dev`).

# ⚖️ License

This software is **free** for non-commercial use. You may use it within your business, as long as you do not charge others for it. See the full license details here: https://github.com/memoWikis/webapp/blob/master/licence.md.

