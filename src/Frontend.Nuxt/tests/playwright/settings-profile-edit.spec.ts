import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Settings Profile Edit', () => {
    test('can save profile information', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings?tab=profile')
        await page.waitForLoadState('networkidle')

        await takeDevScreenshot(page, 'profile-edit-initial')

        // Get current username
        const usernameInput = page.locator('#username')
        await expect(usernameInput).toBeVisible()

        // Click save button without changes (should still work)
        const saveButton = page.locator('.memo-button.btn-primary').filter({ hasText: /Speichern|Save/ })
        await expect(saveButton).toBeVisible()

        await saveButton.click()

        // Wait for response
        await page.waitForTimeout(2000)

        await takeDevScreenshot(page, 'profile-edit-after-save')

        // Check if there's a success alert
        const successAlert = page.locator('.alert-success')
        const hasSuccess = await successAlert.count() > 0

        expect(hasSuccess).toBeTruthy()
    })
})
