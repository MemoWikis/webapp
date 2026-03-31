import { expect } from '@playwright/test'
import { test, TEST_USERS } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Page image upload modal', () => {
    test.skip(!TEST_USERS.admin.password, 'Skipped: set PLAYWRIGHT_ADMIN_PASSWORD in .playwright.env')

    test('visual check of image upload modal', async ({ authenticatedPage: page }) => {
        await page.goto('/1/test', { waitUntil: 'networkidle' })

        // Hover over the page header image to show the edit overlay
        const headerImage = page.locator('.page-header-image').first()
        await headerImage.waitFor({ state: 'visible', timeout: 10000 })
        await headerImage.hover()

        // Click the edit overlay to open the upload modal
        const editOverlay = page.locator('.edit-overlay').first()
        await editOverlay.waitFor({ state: 'visible', timeout: 5000 })
        await editOverlay.click()

        // Wait for the modal to appear
        const modal = page.locator('.modal-default-container')
        await modal.waitFor({ state: 'visible', timeout: 10000 })

        // Screenshot: Wikimedia mode (default)
        await takeDevScreenshot(page, 'image-upload-modal-wikimedia-mode')

        // Switch to Custom upload mode
        const customRadio = page.locator('label').filter({ hasText: /eigenes Bild|Custom|upload/i })
        if (await customRadio.isVisible()) {
            await customRadio.click()
        }

        await page.waitForTimeout(500)

        // Screenshot: Custom mode with dropzone
        await takeDevScreenshot(page, 'image-upload-modal-custom-mode')

        // Verify the dropzone is visible
        const dropzone = page.locator('.imageupload-dropzone')
        await expect(dropzone).toBeVisible()

        // Verify paste hint is visible
        const pasteHint = page.locator('.paste-hint')
        await expect(pasteHint).toBeVisible()

        // Screenshot: Full page for layout context
        await takeDevScreenshot(page, 'image-upload-modal-fullpage', { fullPage: true })
    })
})
