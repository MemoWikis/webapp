# app-stop Skill

## Aliases

This skill can be invoked with any of these names:

- `app-stop`
- `stop-app`
- `stop`

## Description

Stops the running Backend (via `api.ps1`) and Frontend services.

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when the user invokes this skill:**

### Step 1: Stop Backend via api.ps1

Use `run_in_terminal`:

```powershell
cd c:\Projects\memoWikis; .\api.ps1 stop
```

The script will:
- Find the process by PID file or port fallback
- Kill the process and any lingering listeners on port 5069
- Clean up the `.api.pid` file

### Step 2: Stop Frontend Process (if running on port 3000)

Use `run_in_terminal`:

```powershell
Get-NetTCPConnection -LocalPort 3000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | ForEach-Object { Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue }
```

### Step 3: Confirm to User

Tell the user:

- Backend gestoppt (Port 5069)
- Frontend gestoppt (Port 3000)
- Frontend gestoppt (Port 3000)
- Services können mit `app-start` neu gestartet werden

## Expected Result

After running this skill:

- ✅ Backend process terminated
- ✅ Frontend process terminated
- ✅ Ports 3000 and 5069 are free

## Use Cases

1. **Before running backend tests** - Avoids DLL file lock errors
2. **Restarting services** - Stop first, then use `app-start`
3. **Freeing up ports** - When ports are blocked by zombie processes
