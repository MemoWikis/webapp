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
    - [4. Run the Nuxt Frontend (Nuxt 3)](#4-run-the-nuxt-frontend-nuxt-3)
    - [5. Launch the .NET Backend Debug Session](#5-launch-the-net-backend-debug-session)
      - [Additional Tips](#additional-tips)
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

### 4. Run the Nuxt Frontend (Nuxt 3)
The Nuxt frontend project is located in the `Frontend.Nuxt` folder (inside the `src` folder). To start the Nuxt 3 development server, run:

```bash
cd ./Frontend.Nuxt
npm install  # Run this if dependencies are not yet installed
npm run dev
```
> [!NOTE]  
> Note: Make sure your Node.js version meets Nuxt 3 requirements.

### 5. Launch the .NET Backend Debug Session
The backend solution file is located at the root of the `src` folder as `memoWikis.sln`. To run and debug the backend:

1. Open memoWikis.sln in Visual Studio.
2. Select your preferred debug configuration.
3. Start the Backend.Api debug session.

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
- Nuxt Configuration: For frontend customizations, review the `nuxt.config.ts` file in the `Frontend.Nuxt` folder.
  

# ⚖️ License

This software is **free** for non-commercial use. You may use it within your business, as long as you do not charge others for it. See the full license details here: https://github.com/memoWikis/webapp/blob/master/licence.md.

