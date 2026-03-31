import { test as base, expect, type Page } from '@playwright/test'
import * as fs from 'fs'
import * as path from 'path'
import { fileURLToPath } from 'url'

const __filename = fileURLToPath(import.meta.url)
const __dirname = path.dirname(__filename)
const projectRoot = path.resolve(__dirname, '../../../../..')

/**
 * Detects whether the backend is configured for dev or prod database
 * by reading the ConnectionString port from appsettings.Development.json.
 * Port 3306 = dev, Port 3307 = prod.
 */
function detectEnvironment(): 'dev' | 'prod' {
    const settingsPath = path.resolve(projectRoot, 'src/Backend.Api/appsettings.Development.json')

    if (fs.existsSync(settingsPath)) {
        const content = fs.readFileSync(settingsPath, 'utf-8')
        const portMatch = content.match(/Port=(\d+)/)
        if (portMatch) {
            const port = parseInt(portMatch[1], 10)
            if (port === 3306) {
                return 'dev'
            }
        }
    }

    return 'prod'
}

/**
 * Parses a simple key=value env file. Ignores comments and empty lines.
 */
function parseEnvFile(filePath: string): Record<string, string> {
    const envVars: Record<string, string> = {}

    if (!fs.existsSync(filePath)) {
        return envVars
    }

    const content = fs.readFileSync(filePath, 'utf-8')
    for (const line of content.split('\n')) {
        const trimmed = line.trim()
        if (trimmed && !trimmed.startsWith('#')) {
            const eqIndex = trimmed.indexOf('=')
            if (eqIndex > 0) {
                const key = trimmed.substring(0, eqIndex).trim()
                const value = trimmed.substring(eqIndex + 1).trim()
                envVars[key] = value
            }
        }
    }

    return envVars
}

/**
 * Loads credentials from the environment-specific .playwright.env file.
 * - Dev: .playwright.env.dev (committed, known test credentials)
 * - Prod: .playwright.env.prod (gitignored, personal credentials)
 */
function loadPlaywrightEnv(): { env: Record<string, string>; environment: 'dev' | 'prod' } {
    const environment = detectEnvironment()
    const envFile = `.playwright.env.${environment}`
    const envPath = path.resolve(projectRoot, envFile)

    console.log(`🔍 Detected environment: ${environment} → loading ${envFile}`)

    const env = parseEnvFile(envPath)

    if (Object.keys(env).length === 0) {
        console.warn(`⚠️  No credentials found in ${envFile}. Tests requiring login will be skipped.`)
    }

    return { env, environment }
}

const { env: playwrightEnv, environment: detectedEnvironment } = loadPlaywrightEnv()
export { detectedEnvironment }

// Test user credentials: loaded from environment-specific .playwright.env file
export const TEST_USERS = {
    admin: {
        email: playwrightEnv['PLAYWRIGHT_ADMIN_EMAIL'] ?? 'admin@memowikis.net',
        password: playwrightEnv['PLAYWRIGHT_ADMIN_PASSWORD'] ?? 'test',
    },
    user: {
        email: playwrightEnv['PLAYWRIGHT_USER_EMAIL'] ?? 'user@memowikis.net',
        password: playwrightEnv['PLAYWRIGHT_USER_PASSWORD'] ?? 'test',
    },
}

export type TestUserKey = keyof typeof TEST_USERS

export interface AuthFixtures {
    authenticatedPage: Page
    loginAs: (userKey: TestUserKey) => Promise<void>
}

/**
 * Closes any error dialog that might have appeared
 */
async function closeErrorDialogIfPresent(page: Page): Promise<void> {
    const errorDialog = page.locator('dialog:visible, [role="dialog"]:visible')
    const backButton = errorDialog.locator(
        'button:has-text("Zurück"), button:has-text("Back")',
    )

    if (await backButton.isVisible({ timeout: 500 }).catch(() => false)) {
        await backButton.click()
        await page.waitForTimeout(300)
    }
}

/**
 * Performs login via the UI
 * Waits for the header user dropdown to appear as confirmation
 */
async function performLogin(
    page: Page,
    email: string,
    password: string,
): Promise<void> {
    // Wait for page to be fully loaded
    await page.waitForLoadState('networkidle')

    // Open login modal
    const loginButton = page.locator('.login-btn').first()
    await loginButton.waitFor({ state: 'visible', timeout: 10000 })
    await loginButton.click()

    // Wait for the login input to become visible (modal is fully loaded)
    const loginInput = page.locator('input[name="login"]')
    await loginInput.waitFor({ state: 'visible', timeout: 15000 })
    await loginInput.fill(email)

    const passwordInput = page.locator('input[name="password"]')
    await passwordInput.waitFor({ state: 'visible', timeout: 5000 })
    await passwordInput.fill(password)

    // Find submit button in modal footer (.modal-default-footer is the correct class)
    const submitButton = page
        .locator('.modal-default-footer .btn-primary')
        .first()
    await submitButton.waitFor({ state: 'visible', timeout: 5000 })
    await submitButton.click()

    // Wait for modal to close
    await page.waitForTimeout(500)

    // Close any error dialogs that might have appeared
    await closeErrorDialogIfPresent(page)

    // Wait for login to complete - check for user name in header
    // The logged-in state shows a header-btn with user's profile picture
    await expect(
        page.locator('.header-btn:has(.header-author-icon)'),
    ).toBeVisible({ timeout: 15000 })
}

/**
 * Extended test fixture with authentication helpers
 */
export const test = base.extend<AuthFixtures>({
    /**
     * A page that is already logged in as admin
     */
    authenticatedPage: async ({ page }, use) => {
        await page.goto('/')
        await performLogin(
            page,
            TEST_USERS.admin.email,
            TEST_USERS.admin.password,
        )
        await use(page)
    },

    /**
     * Function to login as any test user
     */
    loginAs: async ({ page }, use) => {
        const login = async (userKey: TestUserKey) => {
            const user = TEST_USERS[userKey]
            await page.goto('/')
            await performLogin(page, user.email, user.password)
        }
        await use(login)
    },
})

export { expect }
