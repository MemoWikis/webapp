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
- **C#:** No namespaces. Prefer `Verify()` for tests. Use `_testHarness.ApiCall("apiVue/{controller}/{action}")` for API calls in tests.
- **LESS:** Verify variables in `src/Frontend.Nuxt/assets/includes/colors.less`. Do NOT guess names (e.g. `@memo-dark` does not exist; use `@memo-grey-dark` or `@memo-grey-darkest`).
- **Tests:** Always create/update tests for features and bug fixes. Use `runTests` tool (handles process management automatically).

For naming conventions, file suffixes, and patterns, see **[Style Guide](.github/style-guide.md)**.

# Debugging

- **HTTP 500:** Always check Backend console output first (`get_task_output` for "shell: Backend" task). The stack trace reveals the root cause.
- **Floating-vue & Playwright:** Components render outside DOM hierarchy. Wait for `.v-popper__popper--shown` before interacting:
  ```typescript
  await dropdown.click();
  await expect(page.locator(".v-popper__popper--shown")).toBeVisible();
  await page.locator(".item").click();
  ```
- **Service health:** Before E2E tests, verify ports 3000 (Frontend), 5069 (Backend), 1234 (Hocuspocus). Use `app-start` skill if not running.

# Architecture Overview

## EntityCache

Central in-memory cache for entities (Users, Pages, Questions). **Read from cache, write to both DB and cache.** For full patterns and pitfalls, use the `entity-cache-pattern` skill.

## AI Token Usage

Core files in `src/Backend.Core/Domain/AI/`. Weekly quota system with cache in `ExtendedUserCacheItem.CurrentWeekTokenUsage`. See `docs/ai-token-usage-system.md` for details.

## Content Editor & Collaboration

TipTap editor with Y.js CRDT and HocuspocusProvider for real-time collaboration. See `docs/editor-system-overview.md` for architecture documentation.

# Frontend Development

For **any task in `src/Frontend.Nuxt/`**, use the `frontend-workflow` skill.

# Backend Development

For **any task in `src/Backend.Core/` or `src/Backend.Api/`**, use the `backend-workflow` skill.

# Playwright E2E Tests

- **Config location:** `playwright.config.ts` at project root (NOT in `Frontend.Nuxt/`)
- **Always run from project root:** `cd c:\Projects\memoWikis; npx playwright test`
- **Test users:** Admin: `admin@memowikis.net` / `test`, User: `user@memowikis.net` / `test`
- For running tests, use the `playwright-run` skill.
