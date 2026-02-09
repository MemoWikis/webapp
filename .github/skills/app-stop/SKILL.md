# app-stop Skill

## Aliases

This skill can be invoked with any of these names:
- `app-stop`
- `stop-app`
- `stop`

## Description

Stops the running Backend and Frontend services. Useful before running backend tests (to avoid DLL locks) or when restarting services.

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when the user invokes this skill:**

### Step 1: Stop Backend Process

Use `run_in_terminal` with these parameters:
```powershell
Get-Process -Name "MemoWikis.Backend.Api" -ErrorAction SilentlyContinue | Stop-Process -Force
```

### Step 2: Stop Frontend Process (if running on port 3000)

Use `run_in_terminal` with these parameters:
```powershell
Get-NetTCPConnection -LocalPort 3000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | ForEach-Object { Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue }
```

### Step 3: Verify Services Stopped

Use `run_in_terminal` to confirm:
```powershell
@(3000, 5069) | ForEach-Object { 
    $r = Test-NetConnection localhost -Port $_ -WarningAction SilentlyContinue
    "$($_): $(if($r.TcpTestSucceeded){'Still running'}else{'Stopped'})"
}
```

### Step 4: Confirm to User

Tell the user:
- Backend gestoppt (Port 5069)
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
