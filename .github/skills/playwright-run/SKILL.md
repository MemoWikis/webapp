# playwright-run Skill

## Aliases

This skill can be invoked with any of these names:
- `playwright-run`
- `run-e2e`
- `e2e-test`
- `visual-test`
- `run-playwright`
- `Playwright ausführen`

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

Use `run_in_terminal` with these parameters:
```powershell
cd c:\Projects\memoWikis; npx playwright test --reporter=list
```

For a specific test file:
```powershell
cd c:\Projects\memoWikis; npx playwright test ai-create-page.spec.ts --reporter=list
```

For headed mode (visible browser):
```powershell
cd c:\Projects\memoWikis; npx playwright test --headed --reporter=list
```

### Step 3: Review Screenshots

After tests complete, check the `test-results/screenshots/` folder for visual feedback.

Use `list_dir` to see available screenshots:
- **path:** `c:\Projects\memoWikis\test-results\screenshots`

### Step 4: Report Results

Tell the user:
- Which tests passed/failed
- Location of screenshots: `test-results/screenshots/`
- Location of HTML report: `test-results/html-report/`

## Common Test Commands

| Command | Description |
|---------|-------------|
| `npx playwright test` | Run all tests |
| `npx playwright test ai-create-page.spec.ts` | Run specific file |
| `npx playwright test --headed` | Run with visible browser |
| `npx playwright test --debug` | Run in debug mode |
| `npx playwright test --project=mobile` | Run mobile tests only |

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
import { takeDevScreenshot } from '../fixtures/screenshot.helper'

// Take a full page screenshot
await takeDevScreenshot(page, 'descriptive-name')

// Take element screenshot
await takeElementScreenshot(page, '.my-element', 'element-name')
```

## Test Users

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@memowikis.net | test |
| User | user@memowikis.net | test |

## Expected Result

After running this skill, the user should have:
- ✅ Test results in terminal
- ✅ Screenshots in `test-results/screenshots/`
- ✅ HTML report in `test-results/html-report/`
- ✅ Clear pass/fail status for each test

## Troubleshooting

### Tests timing out
- Ensure Backend is running on port 5069
- Ensure Frontend is running on port 3000
- Check for network issues

### Login failing
- Verify dev database has test users
- Check if login modal selectors have changed

### Screenshots not saving
- Check `test-results/screenshots/` folder exists
- Verify disk space available
