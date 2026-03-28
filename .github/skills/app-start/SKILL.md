# app-start Skill

## Aliases

This skill can be invoked with any of these names:

- `app-start`
- `start-app`
- `startup`
- `start` (when context is about the application)

## Description

Starts Backend (via `api.ps1` with PID tracking and health check) and Frontend in a VS Code terminal. The Backend script automatically detects if the API is already running and skips starting it again.

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when the user invokes this skill:**

### Step 1: Start Backend via api.ps1

Use `run_in_terminal`:

```powershell
cd c:\Projects\memoWikis; .\api.ps1 start
```

The script will:
- Check if the API is already running (PID file + port check)
- If already healthy, skip starting
- If not running, launch `dotnet watch run` and wait for the health endpoint
- Store the PID in `.api.pid` for reliable stop/restart

### Step 2: Start Frontend Task

Use `run_task` with these parameters:

- **workspaceFolder:** `c:\Projects\memoWikis`
- **id:** `Frontend`

### Step 3: Confirm to User

Tell the user:

- Backend läuft auf http://localhost:5069 (mit dotnet watch für Hot-Reload)
- Frontend läuft auf http://localhost:3000
- Backend-Logs in `.api.log.out` / `.api.log.err`

## Expected Result

After running this skill, the user should have:

- ✅ Backend running with `dotnet watch` (hot-reload enabled)
- ✅ Backend PID tracked in `.api.pid`
- ✅ Frontend terminal showing npm/nuxt logs
- ✅ Both services accessible via their URLs
