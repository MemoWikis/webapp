````skill
# app-restart Skill

## Aliases

This skill can be invoked with any of these names:
- `app-restart`
- `restart-app`
- `restart`

## Description

Stops and restarts Backend and Frontend services. Useful when cache or DLLs need refreshing, or after configuration changes.

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when the user invokes this skill:**

### Step 1: Stop Backend Process

Use `run_in_terminal`:
```powershell
Get-Process -Name "MemoWikis.Backend.Api" -ErrorAction SilentlyContinue | Stop-Process -Force
```

### Step 2: Stop Frontend Process

Use `run_in_terminal`:
```powershell
Get-NetTCPConnection -LocalPort 3000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | ForEach-Object { Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue }
```

### Step 3: Start Backend Task

Use `run_task` with:
- **workspaceFolder:** `c:\Projects\memoWikis`
- **id:** `Backend`

### Step 4: Start Frontend Task

Use `run_task` with:
- **workspaceFolder:** `c:\Projects\memoWikis`
- **id:** `Frontend`

### Step 5: Confirm to User

Tell the user:
- Backend restarted on http://localhost:5069
- Frontend restarted on http://localhost:3000

## Expected Result

After running this skill:
- Backend and Frontend processes restarted cleanly
- Both services accessible via their URLs
- Live logs visible in VS Code terminals

## Use Cases

1. **After config changes** - `appsettings.json` or `nuxt.config.ts` modified
2. **DLL refresh** - After building backend changes
3. **Cache reset** - When EntityCache needs reinitializing
4. **Zombie processes** - When ports are stuck from previous sessions
````
