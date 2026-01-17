import { test as base, expect, type Page } from '@playwright/test'

// Test user credentials (from dev database)
export const TEST_USERS = {
    admin: {
        email: 'admin@memowikis.net',
        password: 'test',
    },
    user: {
        email: 'user@memowikis.net',
        password: 'test',
    },
} as const

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
    const backButton = errorDialog.locator('button:has-text("Zurück"), button:has-text("Back")')
    
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
    const submitButton = page.locator('.modal-default-footer .btn-primary').first()
    await submitButton.waitFor({ state: 'visible', timeout: 5000 })
    await submitButton.click()

    // Wait for modal to close
    await page.waitForTimeout(500)

    // Close any error dialogs that might have appeared
    await closeErrorDialogIfPresent(page)

    // Wait for login to complete - check for either user dropdown or profile image in header
    await expect(
        page.locator('.header-user-dropdown, .profile-image-container')
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
