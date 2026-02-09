# app-start Skill

## Aliases

This skill can be invoked with any of these names:

- `app-start`
- `start-app`
- `startup`
- `start` (when context is about the application)

## Description

Starts Backend and Frontend in separate VS Code integrated terminals with descriptive names. The skill checks if services are already running to avoid starting them multiple times.

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when the user invokes this skill:**

### Step 1: Start Backend Task

Use `run_task` with these parameters:

- **workspaceFolder:** `c:\Projects\memoWikis`
- **id:** `Backend`

### Step 2: Start Frontend Task

Use `run_task` with these parameters:

- **workspaceFolder:** `c:\Projects\memoWikis`
- **id:** `Frontend`

### Step 3: Confirm to User

Tell the user:

- Backend läuft auf http://localhost:5069
- Frontend läuft auf http://localhost:3000
- Die Terminals zeigen die Live-Logs

## Expected Result

After running this skill, the user should have:

- ✅ Two visible VS Code integrated terminals
- ✅ Backend terminal showing dotnet logs
- ✅ Frontend terminal showing npm/nuxt logs
- ✅ Both services accessible via their URLs
