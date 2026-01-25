import { test, expect } from '@playwright/test'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

// Generate unique email for each test run to avoid conflicts
function generateTestEmail(): string {
    const timestamp = Date.now()
    const random = Math.random().toString(36).substring(2, 8)
    return `test-${timestamp}-${random}@example.com`
}

test.describe('User Registration', () => {
    test('registration page loads correctly', async ({ page }) => {
        // Navigate to registration page (use English URL)
        await page.goto('/Register')

        // Verify page title is visible
        await expect(
            page.locator('h1.register-title'),
        ).toBeVisible({ timeout: 10000 })

        // Verify form inputs are present
        await expect(page.locator('input[name="login"]').first()).toBeVisible()
        await expect(page.locator('input[name="password"]')).toBeVisible()

        // Verify social login buttons are present
        await expect(page.locator('#GoogleRegister')).toBeVisible()
        await expect(page.locator('#FacebookRegister')).toBeVisible()

        await takeDevScreenshot(page, 'registration-page-loaded')
    })

    test('registration with valid data succeeds', async ({ page }) => {
        // Navigate to registration page (use English URL)
        await page.goto('/Register')

        // Wait for the form to be fully loaded and interactive
        await page.waitForTimeout(1000)

        // Generate unique test data
        const testEmail = generateTestEmail()
        const testUsername = `TestUser${Date.now()}`
        const testPassword = 'TestPassword123!'

        // Fill in the registration form using getByRole for better reliability
        // Username field (type="text")
        const usernameInput = page.locator('fieldset input[type="text"]')
        await usernameInput.waitFor({ state: 'visible', timeout: 10000 })
        await usernameInput.click()
        await usernameInput.fill(testUsername)
        
        // Verify username was filled
        await expect(usernameInput).toHaveValue(testUsername)

        // Email input (type="email")
        const emailInput = page.locator('fieldset input[type="email"]')
        await emailInput.click()
        await emailInput.fill(testEmail)
        
        // Verify email was filled
        await expect(emailInput).toHaveValue(testEmail)

        // Password input
        const passwordInput = page.locator('fieldset input[name="password"]')
        await passwordInput.click()
        await passwordInput.fill(testPassword)
        
        // Verify password was filled
        await expect(passwordInput).toHaveValue(testPassword)

        await takeDevScreenshot(page, 'registration-form-filled')

        // Click register button (the one inside the form, col-sm-12 class)
        const registerButton = page.locator('fieldset button.btn-primary.col-sm-12')
        await registerButton.click()

        // Wait for navigation or success/error state
        // After successful registration, user should be redirected to their wiki or homepage
        // Allow time for API call
        try {
            await page.waitForURL((url) => {
                // Should redirect away from registration page
                const path = url.pathname.toLowerCase()
                return !path.includes('registrieren') && !path.includes('register')
            }, { timeout: 30000 })

            await takeDevScreenshot(page, 'registration-success')

            // Verify user is logged in (header should show user icon)
            await expect(
                page.locator('.header-btn:has(.header-author-icon)'),
            ).toBeVisible({ timeout: 10000 })
        } catch (error) {
            // Take error screenshot to see what went wrong
            await takeDevScreenshot(page, 'registration-failed')
            
            // Check if there's an error message on the page
            const errorMessage = await page.locator('.alert-danger').textContent().catch(() => null)
            if (errorMessage) {
                throw new Error(`Registration failed with error: ${errorMessage}`)
            }
            throw error
        }
    })

    test('registration with invalid email shows error', async ({ page }) => {
        // Navigate to registration page
        await page.goto('/Register')

        await page.locator('input[name="login"]').first().waitFor({ state: 'visible', timeout: 10000 })

        // Fill in form with invalid email
        const usernameInput = page.locator('input[name="login"]').first()
        await usernameInput.fill('TestUser')

        const emailInput = page.locator('input[type="email"]')
        await emailInput.fill('invalid-email')

        const passwordInput = page.locator('input[name="password"]')
        await passwordInput.fill('TestPassword123!')

        // Click register button
        const registerButton = page.locator('fieldset button.btn-primary.col-sm-12')
        await registerButton.click()

        // Wait for error message to appear
        await expect(
            page.locator('.alert-danger'),
        ).toBeVisible({ timeout: 5000 })

        await takeDevScreenshot(page, 'registration-invalid-email-error')
    })

    test('registration with empty fields shows validation', async ({ page }) => {
        // Navigate to registration page
        await page.goto('/Register')

        await page.locator('input[name="login"]').first().waitFor({ state: 'visible', timeout: 10000 })

        // Click register button without filling any fields
        const registerButton = page.locator('fieldset button.btn-primary.col-sm-12')
        await registerButton.click()

        // Wait for error message or validation feedback
        await page.waitForTimeout(500)

        await takeDevScreenshot(page, 'registration-empty-fields')

        // Should still be on registration page
        expect(page.url().toLowerCase()).toMatch(/register/i)
    })

    test('can navigate to login from registration page', async ({ page }) => {
        // Navigate to registration page
        await page.goto('/Register')

        // Wait for page to load
        await page.locator('h1.register-title').waitFor({ state: 'visible', timeout: 10000 })

        // Click the login button (the btn-link that opens login modal)
        const loginLink = page.locator('fieldset .btn-link')
        await loginLink.click()

        // Wait for login modal to appear
        await expect(
            page.locator('.login-inputs').first(),
        ).toBeVisible({ timeout: 10000 })

        await takeDevScreenshot(page, 'registration-to-login-modal')
    })
})
