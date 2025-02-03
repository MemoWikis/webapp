## Table of Contents

- [memoWikis - Local Development (WIP)](#memowikis---local-development-wip)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
    - [1. Clone the Repository](#1-clone-the-repository)
    - [2. Configure Environment Files & Start Docker Services](#2-configure-environment-files--start-docker-services)
    - [3. Set Up Your Anthropc API Key](#3-set-up-your-anthropc-api-key)
    - [4. Run the Nuxt Frontend (Nuxt 3)](#4-run-the-nuxt-frontend-nuxt-3)
    - [5. Launch the .NET Backend Debug Session](#5-launch-the-net-backend-debug-session)
- [Work in Progress](#work-in-progress)
- [⚖️ License](#️-license)


# memoWikis - Local Development (WIP)

This project is a web application that includes several services (Dockerized MySQL, Redis, Meilisearch, and Hocuspocus for Tiptap), a Nuxt 3 frontend, and a .NET backend. **Note:** This documentation is a work in progress (WIP), and a `Contributing.md` file is not available yet.

## Prerequisites

- **Git** – for cloning the repository  
- **Docker & docker-compose** – to run the backend services  
- **Node.js & npm** – for the Nuxt 3 frontend  
- **Visual Studio** – for debugging the .NET backend  
- A terminal of your choice (e.g., PowerShell on Windows or Bash on Linux)

## Setup

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/MemoWikis/webapp.git
cd webapp
```


### 2. Configure Environment Files & Start Docker Services
The project uses example configuration files that need to be copied to their corresponding development files.

#### On Windows (PowerShell):
```ppwershell
cp ./TrueOrFalse.Frontend.Web/appsettings.Development.json.example ./TrueOrFalse.Frontend.Web/appsettings.Development.json -Force; `
cp ./Docker/Dev/.env.example ./Docker/Dev/.env -Force; `
cd ./Docker/Dev; `
docker-compose up -d
```

#### On Linux / macOS (Bash):
```bash
cp ./TrueOrFalse.Frontend.Web/appsettings.Development.json.example ./TrueOrFalse.Frontend.Web/appsettings.Development.json
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

### 3. Set Up Your Anthropc API Key
To enable the AI functions within the application, you must supply a valid Anthropc API key. Open the file `TrueOrFalse.Frontend.Web/appsettings.Development.json` and update the **Anthropic** section with your API key as follows:

```json
{
  "Anthropic": {
    "ApiKey": "your-actual-api-key"
  }
}
```
> [!WARNING]  
> **Without a valid API key, the AI functions in the application will not work.**

### 4. Run the Nuxt Frontend (Nuxt 3)
The Nuxt frontend project is located in the `TrueOrFalse.Frontend.Web.Nuxt` folder (inside the `src` folder). To start the Nuxt 3 development server, run:

```bash
cd ./TrueOrFalse.Frontend.Web.Nuxt
npm install  # Run this if dependencies are not yet installed
npm run dev
```
> [!NOTE]  
> Note: Make sure your Node.js version meets Nuxt 3 requirements.

### 5. Launch the .NET Backend Debug Session
The backend solution file is located at the root of the `src` folder as `TrueOrFalse.sln`. To run and debug the backend:

1. Open TrueOrFalse.sln in Visual Studio.
2. Select your preferred debug configuration.
3. Start the debug session.

#### Additional Tips
- Anthropic API Key for AI Functions: Ensure your valid Anthropc API key is inserted in the appsettings.Development.json file. Without this key, the AI features will not work.

- Docker Management:
  - To check the status of your containers:
`bash
docker-compose ps
`
  - To view logs for any service:
     `bash
docker-compose logs [service-name]`
- Nuxt Configuration: For frontend customizations, review the `nuxt.config.ts` file in the `TrueOrFalse.Frontend.Web.Nuxt` folder.

## Work in Progress
This documentation is a work in progress (WIP), and additional instructions or a Contributing.md file are not available yet.

## ⚖️ License
https://github.com/MemoWikis/webapp/blob/master/licence.txt
