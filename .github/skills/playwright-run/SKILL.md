# playwright-run Skill

## Aliases

This skill can be invoked with any of these names:

- `playwright-run`
- `run-e2e`
- `e2e-test`
- `visual-test`
- `run-playwright`

## Description

Runs Playwright E2E tests with screenshots saved to `test-results/screenshots/` for visual feedback during development. The agent can then monitor the screenshot folder to see test results visually.

## Prerequisites

- Backend running on http://localhost:5069
- Frontend running on http://localhost:3000
- Playwright installed (`npm install` in root or Frontend.Nuxt)

## Copilot Execution Steps

**IMPORTANT: Follow these steps exactly when the user invokes this skill:**

### Step 1: Verify Services Running

Check if Backend and Frontend are running. If not, suggest using the `app-start` skill first.

### Step 2: Run Tests

**CRITICAL:** Always run from project root where `playwright.config.ts` is located!

Use `run_in_terminal` with these parameters:

```powershell
cd c:\Projects\memoWikis; npx playwright test --project=chromium --reporter=list
```

For a specific test file:

```powershell
cd c:\Projects\memoWikis; npx playwright test ai-create-page.spec.ts --project=chromium --reporter=list
```

For headed mode (visible browser):

```powershell
cd c:\Projects\memoWikis; npx playwright test --project=chromium --headed --reporter=list
```

**Note:** Use `--project=chromium` to skip webkit/mobile tests if those browsers are not installed.

### Step 3: Review Screenshots

After tests complete, check the `test-results/screenshots/` folder for visual feedback.

Use `list_dir` to see available screenshots:

- **path:** `c:\Projects\memoWikis\test-results\screenshots`

### Step 4: Report Results

Tell the user:

- Which tests passed/failed
- Location of screenshots: `test-results/screenshots/`
- Location of HTML report: `test-results/html-report/`

**Note:** If tests fail with a **TimeoutError** waiting for a selector, it often means the Nuxt page crashed (e.g., 500 Internal Server Error due to build errors like missing LESS variables).

- **ACTION:** Check the **Frontend terminal output** for build errors or exceptions!
- **ACTION:** Do not assume the selector is just missing; assume the page failed to render.

## Common Test Commands

| Command                                                         | Description                             |
| --------------------------------------------------------------- | --------------------------------------- |
| `npx playwright test --project=chromium`                        | Run all tests (chromium only)           |
| `npx playwright test ai-create-page.spec.ts --project=chromium` | Run specific file                       |
| `npx playwright test --project=chromium --headed`               | Run with visible browser                |
| `npx playwright test --debug`                                   | Run in debug mode                       |
| `npx playwright test --project=mobile`                          | Run mobile tests only (requires webkit) |

**Note:** Always run from project root (`c:\Projects\memoWikis`), not from `src/Frontend.Nuxt`!

## Test Structure

```
src/Frontend.Nuxt/tests/playwright/
├── fixtures/
│   ├── auth.fixture.ts       # Login helpers
│   └── screenshot.helper.ts  # Screenshot utilities
├── ai-create-page.spec.ts    # AI page creation tests
└── settings-layout.spec.ts   # Settings page tests
```

## Screenshot Helper Usage

In tests, use the screenshot helper for development feedback:

```typescript
import { takeDevScreenshot } from "../fixtures/screenshot.helper";

// Take a full page screenshot
await takeDevScreenshot(page, "descriptive-name");

// Take element screenshot
await takeElementScreenshot(page, ".my-element", "element-name");
```

## Test Users

See `copilot-instructions.md` for test user credentials.

## Expected Result

After running this skill, the user should have:

- ✅ Test results in terminal
- ✅ Screenshots in `test-results/screenshots/`
- ✅ HTML report in `test-results/html-report/`
- ✅ Clear pass/fail status for each test

## Troubleshooting

- **Tests timing out:** Ensure Backend (5069) and Frontend (3000) are running. Use `app-start` skill.
- **Login failing:** Verify dev database has test users, check if login modal selectors changed.
- **TimeoutError on selector:** Often means the Nuxt page crashed. Check **Frontend terminal output** for build errors.
