# Code Style Guide

For comprehensive naming conventions, file structure, and patterns, see **[Style Guide](.github/style-guide.md)**.

# Service Health Check

**IMPORTANT:** Before running E2E tests or debugging frontend issues, always verify services are running:

```powershell
# Quick one-liner for both ports
@(3000, 5069) | ForEach-Object {
    $r = Test-NetConnection localhost -Port $_ -WarningAction SilentlyContinue
    "$($_): $(if($r.TcpTestSucceeded){'Running'}else{'Not running'})"
}
```

**Expected Ports:**

- **Frontend (Nuxt):** http://localhost:3000
- **Backend (.NET):** http://localhost:5069
- **Hocuspocus (WebSocket):** ws://localhost:1234

If services are not running, use the `app-start` skill to start them.

## Quick Reference

- **Files/Folders:** kebab-case (`user-profile.store.ts`, `order-card.component.vue`)
- **Exports:** PascalCase for types/classes/components (`UserProfile`, `OrderCard`)
- **Exports:** camelCase for functions/composables (`useUserStore`, `formatDate`)
- **File Suffixes:** Use consistent suffixes for predictable discovery
  - Components: `*.component.vue`
  - Stores: `*.store.ts`
  - Enums: `*.enum.ts`
  - Types: `*.types.ts`
  - Utils: `*.utils.ts`

## Common Rules (All Languages)

- Please always write code comments in English.
- Always spell out variable names; no abbreviations.
- After if statements and loops, never use single line statements.

# C#

- Do not use namespaces in C#.
- Prefer Verify() for Tests
- Use \_testHarness.ApiCall("apiVue/{controller}/{action}") to do apicalls, do not init/resolve controllers in tests

# Unit Tests

- For API calls use Testharness.ApiCall(..)

## Running Backend Tests

**IMPORTANT:** Before running backend tests, stop the running Backend process to avoid DLL file locks:

```powershell
# Stop Backend process first
Get-Process -Name "MemoWikis.Backend.Api" -ErrorAction SilentlyContinue | Stop-Process -Force

# Then run tests
cd src/Tests; dotnet test --filter "TestClassName"
```

**Preferred:** Use the `runTests` tool instead of terminal commands – it handles process management automatically.

## NHibernate SQL Aggregate Functions

When using native SQL queries with `AliasToBeanResultTransformer`, use correct C# types for MySQL aggregates:

| SQL Function | MySQL Returns | C# Property Type |
| ------------ | ------------- | ---------------- |
| `COUNT(*)`   | BIGINT        | `long`           |
| `SUM()`      | DECIMAL       | `decimal`        |
| `AVG()`      | DECIMAL       | `decimal`        |

Example:

```csharp
public class MySummary
{
    public long RequestCount { get; set; }      // COUNT(*)
    public decimal TotalTokens { get; set; }    // SUM()
}
```

# Glossar

- `page`: a page
- `childpage`: a page that is a child of page or wiki
- `subpage`: subpage is a synonym for childpage and only used in user interface
- `wiki`: is a special kind of a page
- `orphaned page`: a page that does not have a parent page or wiki
- `wishknowledge`: knowledge that a user specifically wants to learn or has marked as desired to learn
- `wuwi`: abbreviation for "Wunschwissen" (wishknowledge), can be used in backend code, comments, and non-user-facing texts

# Frontend and Translations

- refer to the .copilotinstructions file in `../src/Frontend.Nuxt` for frontend specific instructions

# AI Token Usage System

Core components in `src/Backend.Core/Domain/AI/`:

- `Usage/TokenDeductionService.cs` - Token balance management, affordability checks, weekly quota calculation
- `Usage/AiUsageLogRepo.cs` - Usage logging with cost analytics, supports queries by model/date/user
- `Models/AiModelRegistry.cs` - Model whitelist & token cost multipliers, caching

**Weekly Quota System:**

- Subscribers: more tokens per week
- Free-tier: less tokens per week
- Quota resets every Monday at 00:00
- No accumulation of unused tokens

**Caching:**

- Weekly token usage is cached in `UserCacheItem.CurrentWeekTokenUsage`
- Loaded during cache initialization via `EntityCacheInitializer`
- Updated after each AI usage in `AiUsageLogRepo.AddUsage`
- `TokenDeductionService` reads from cache, never queries DB for usage

For detailed documentation including integration points, database schema, and token flow examples, see `docs/ai-token-usage-system.md`

# EntityCache Pattern

The EntityCache is the central in-memory cache for entities (Users, Pages, Questions).

**Key Principle:** Always read from cache, write to both DB and cache.

```csharp
// Reading: Use EntityCache
var user = EntityCache.GetUserById(userId);

// Writing: Update DB first, then cache
_userWritingRepo.Update(user);
EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
```

For the full pattern with examples and pitfalls, use the `entity-cache-pattern` skill.

# Content Editor & Collaboration System

Core components in `src/Frontend.Nuxt/components/page/content/`:

- `ContentEditor.vue` - TipTap rich-text editor with real-time collaboration
- `pageStore.ts` - State management, content persistence, auto-save (3s debounce)
- Real-time collaboration via HocuspocusProvider & Y.js CRDT
- Offline editing with IndexedDB persistence
- Hash-based content versioning with configurable conflict resolution

Key Technologies: TipTap (editor), Y.js (CRDT), HocuspocusProvider (WebSocket), IndexedDB (offline cache)

Conflict Strategies: ServerWins (default), ClientWins, Timestamp, UserChoice (UI pending)

**📚 Documentation:**

- **[Editor System Overview](../docs/editor-system-overview.md)** - Architecture, quick start, file structure
- **[Collaboration System](../docs/editor-collaboration-system.md)** - Real-time editing, WebSocket events, reconnection
- **[Conflict Resolution](../docs/editor-conflict-resolution.md)** - Versioning, strategies, hash-based comparison
- **[Known Issues & Solutions](../docs/editor-issues.md)** - Hydration mismatches, stale cache, debugging tips

For optimal LLM performance, documentation is split into focused files. Start with the overview, then dive into specific topics as needed.

# Skills

Skills are domain-specific automation workflows that help with common development tasks.

**Important:** All skills, documentation, and descriptions must be written in **English**.

## App Management Skills

- **app-start** (aliases: start-app, startup, start): Starts Backend (port 5069) and Frontend (port 3000) in foreground terminals. Checks if services are already running before starting them.
- **app-stop** (aliases: stop-app, stop): Stops Backend and Frontend processes. Use before running backend tests to avoid DLL file locks.

## Database Skills

- **dev-database-create**: Create a fresh dev database with latest test data (runs ScenarioBuilder test, generates schema.sql, reinitializes MySQL)
- **dev-database-reset**: Reset the dev database from existing schema.sql (just reinitializes MySQL without updating schema.sql)

## Architecture Skills

- **entity-cache-pattern**: EntityCache read/write patterns, cache+DB synchronization, common pitfalls

## Testing Skills

- **playwright-run** (aliases: run-e2e, e2e-test, visual-test): Run Playwright E2E tests with screenshots saved to `test-results/screenshots/` for visual feedback during development

# Playwright E2E Tests

## Overview

Playwright tests are located in `src/Frontend.Nuxt/tests/playwright/`. Screenshots are automatically saved to `test-results/screenshots/` for monitoring.

## Key Files

- `playwright.config.ts` - Main configuration (**root level, not in Frontend.Nuxt!**)
- `fixtures/auth.fixture.ts` - Reusable login fixture with `authenticatedPage`
- `fixtures/screenshot.helper.ts` - Screenshot utilities for development feedback

## Running Tests

**CRITICAL:** Always run Playwright from the project root directory where `playwright.config.ts` is located:

```powershell
# Correct - from project root
cd c:\Projects\memoWikis
npx playwright test settings-ai-usage.spec.ts

# Wrong - baseURL will be undefined
cd src/Frontend.Nuxt
npx playwright test  # ❌ Error: Cannot navigate to invalid URL
```

```bash
# Run all Playwright tests
npx playwright test

# Run specific test file
npx playwright test ai-create-page.spec.ts

# Run with headed browser (visible)
npx playwright test --headed

# Run in debug mode
npx playwright test --debug

# Run only chromium (skip webkit/mobile if not installed)
npx playwright test --project=chromium
```

## Usage Pattern

```typescript
import { test, expect } from "../fixtures/auth.fixture";
import { takeDevScreenshot } from "../fixtures/screenshot.helper";

test("my test", async ({ authenticatedPage }) => {
  // authenticatedPage is already logged in as admin
  await authenticatedPage.goto("/Settings"); // Use localized paths: /Settings, /Einstellungen
  await takeDevScreenshot(authenticatedPage, "descriptive-name");
});
```

## Key Selectors

- **Login button (header):** `.login-btn`
- **Login modal input:** `input[name="login"]`, `input[name="password"]`
- **Modal submit button:** `.modal-default-footer .btn-primary`
- **Logged-in user indicator:** `.header-btn:has(.header-author-icon)`

## Test Users (dev database)

- Admin: `admin@memowikis.net` / `test`
- User: `user@memowikis.net` / `test`

## Screenshot Monitoring

Screenshots are saved to `test-results/screenshots/` with timestamps. Monitor this folder during development for visual feedback.
