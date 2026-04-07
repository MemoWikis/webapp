# Glossary

- `page`: a page
- `childpage`: a page that is a child of page or wiki
- `subpage`: synonym for childpage, only used in user interface
- `wiki`: a special kind of a page
- `orphaned page`: a page without a parent page or wiki
- `wishknowledge`: knowledge a user wants to learn or has marked as desired
- `wuwi`: abbreviation for "Wunschwissen" (wishknowledge), used in backend code and comments

# Critical Rules

- All code comments in English
- Spell out variable names; no abbreviations
- Always use braces after if/loops (no single-line statements)
- **Do NOT restart the backend** (`.\api.ps1 start/restart`) after pure frontend changes. The backend restart is slow and unnecessary when only files in `src/Frontend.Nuxt/` were modified. Only restart the backend when backend code (`src/Backend.Api/`, `src/Backend.Core/`) was changed.
- **C#:** No namespaces. Prefer `Verify()` for tests. Use `_testHarness.ApiCall("apiVue/{controller}/{action}")` for API calls in tests.
- **LESS:** Verify variables in `src/Frontend.Nuxt/assets/includes/colors.less`. Do NOT guess names (e.g. `@memo-dark` does not exist; use `@memo-grey-dark` or `@memo-grey-darkest`).
- **Tests:** Always create/update tests for features and bug fixes. Use `runTests` tool (handles process management automatically).

For naming conventions, file suffixes, and patterns, see **[Style Guide](.github/style-guide.md)**.

# Debugging

- **HTTP 500:** Always check Backend console output first (`.api.log.out` / `.api.log.err`). The stack trace reveals the root cause.
- **Floating-vue & Playwright:** Components render outside DOM hierarchy. Wait for `.v-popper__popper--shown` before interacting:
  ```typescript
  await dropdown.click();
  await expect(page.locator(".v-popper__popper--shown")).toBeVisible();
  await page.locator(".item").click();
  ```
- **Service health:** Before E2E tests, verify ports 3000 (Frontend), 5069 (Backend), 1234 (Hocuspocus). Start them if not running (see "Starting / Stopping the App").

# Backend API Control

Use `api.ps1` at the project root to manage the Backend process:

```powershell
.\api.ps1 start    # Start with dotnet watch (hot-reload), waits for health check
.\api.ps1 stop     # Stop by PID (with port fallback), cleans up .api.pid
.\api.ps1 restart  # Stop + Start
.\api.ps1 status   # Check if running and healthy
```

The script uses a `.api.pid` file for reliable process tracking and checks the `/healthcheck_backend` endpoint to confirm the API is healthy.
**Always prefer `.\api.ps1 stop` over `Get-Process | Stop-Process` to stop the Backend.**

# Starting / Stopping the App

**Start Backend + Frontend:**

1. `run_in_terminal`: `cd c:\Projects\memoWikis; .\api.ps1 start`
2. `run_task` with id `Frontend` (workspaceFolder: `c:\Projects\memoWikis`)

**Stop Backend + Frontend:**

1. `run_in_terminal`: `cd c:\Projects\memoWikis; .\api.ps1 stop`
2. `run_in_terminal`: `Get-NetTCPConnection -LocalPort 3000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | ForEach-Object { Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue }`

**Restart:** Run stop then start steps above, or `.\api.ps1 restart` for Backend only.

# Architecture Overview

## EntityCache

Central in-memory cache for entities (Users, Pages, Questions). **Read from cache, write to both DB and cache.** For full patterns and pitfalls, use the `entity-cache-pattern` skill.

### Cache-First Rule

**Always check the in-memory cache before writing SQL queries.** The `ExtendedUserCacheItem` (loaded at login, 10h sliding expiration) already contains per-user data:

| Data                       | Cache Location                                        | SQL needed? |
| -------------------------- | ----------------------------------------------------- | ----------- |
| Answer counts per question | `ExtendedUserCacheItem.AnswerCounter`                 | No          |
| Daily learning activity    | `ExtendedUserCacheItem.ActivityCounts`                | No          |
| Question valuations        | `ExtendedUserCacheItem.QuestionValuations`            | No          |
| Page valuations            | `ExtendedUserCacheItem.PageValuations`                | No          |
| AI token usage             | `UserCacheItem.CurrentWeekTokenUsage`                 | No          |
| Page/Question entities     | `EntityCache.GetPage()` / `EntityCache.GetQuestion()` | No          |

When adding new per-user data: **extend the cache** (`ExtendedUserCacheItem`), populate it in `ExtendedUserCache.CreateExtendedUserCacheItem()`, and update it on writes (see `AnswerCache.AddAnswerToCache` pattern).

## Service Registration (IoC)

Services are registered via the marker interface `IRegisterAsInstancePerLifetime` (one instance per HTTP request). **No manual Autofac registration needed** — the container scans for this interface automatically.

```csharp
// Correct: implement marker interface, inject dependencies via primary constructor
public class MyService(SomeDependency _dependency, AnotherRepo _repo) : IRegisterAsInstancePerLifetime
{
    public Result DoWork(int userId) { ... }
}
```

**Keep business logic in services (`Backend.Core`), not in controllers (`Backend.Api`).** Controllers should only call services, check auth, and return responses.

## AI Token Usage

Core files in `src/Backend.Core/Domain/AI/`. Weekly quota system with cache in `ExtendedUserCacheItem.CurrentWeekTokenUsage`. See `docs/ai-token-usage-system.md` for details.

## Content Editor & Collaboration

TipTap editor with Y.js CRDT and HocuspocusProvider for real-time collaboration. See `docs/editor-system-overview.md` for architecture documentation.

# Frontend Development

For **any task in `src/Frontend.Nuxt/`**, use the `frontend-workflow` skill.

# Database Migrations

When changing the database schema (adding/removing/renaming columns or tables, changing types), **always** create a migration step:

1. **Migration step:** Create `UpdateToVsXXX.cs` in `src/Backend.Core/Infrastructure/Update/Steps/`. Increment the version number from the last existing step.
2. **Register:** Add `.Add(XXX, () => UpdateToVsXXX.Run(_nhibernateSession))` in `src/Backend.Core/Infrastructure/Update/Update.cs`.
3. **Keep in sync:** The `CREATE TABLE` in the original migration step must match the current NHibernate mapping (`*Map.cs`). If the mapping changed since the original step, create a **new** migration step that `ALTER TABLE`s the difference — never silently fix columns only in the DB.
4. **MySQL compatibility:** Use `ADD COLUMN` (not `ADD COLUMN IF NOT EXISTS` — that's MariaDB-only). `CREATE TABLE IF NOT EXISTS` is fine.
5. **Dev schema:** Also update `src/Docker/Dev/mysql-init/schema.sql` if it exists for the affected table.

# Backend Development

For **any task in `src/Backend.Core/` or `src/Backend.Api/`**, use the `backend-workflow` skill.

# Playwright E2E Tests

- **Config location:** `playwright.config.ts` at project root (NOT in `Frontend.Nuxt/`)
- **Always run from project root:** `cd c:\Projects\memoWikis; npx playwright test`
- **Test users:** Admin: `admin@memowikis.net` / `test`, User: `user@memowikis.net` / `test`
- For running tests, use the `playwright-run` skill.
