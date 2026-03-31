import { expect } from '@playwright/test'
import { test, TEST_USERS } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Page image upload modal', () => {
    test.skip(!TEST_USERS.admin.password, 'Skipped: set PLAYWRIGHT_ADMIN_PASSWORD in .playwright.env')

    test('visual check of image upload modal', async ({ authenticatedPage: page }) => {
        // Navigate to the first page in the dev database
        await page.goto('/Welcome-to-memoWikis/1', { waitUntil: 'networkidle' })

        // Click the page header image area to open the upload modal
        // The edit overlay intercepts pointer events, so click it directly with force
        const headerImage = page.locator('.page-header-image').first()
        await headerImage.waitFor({ state: 'visible', timeout: 10000 })
        await headerImage.click({ force: true })

        // Wait for the modal to appear
        const modal = page.locator('.modal-default-container')
        await modal.waitFor({ state: 'visible', timeout: 10000 })

        // Screenshot: Wikimedia mode (default)
        await takeDevScreenshot(page, 'image-upload-modal-wikimedia-mode')

        // Switch to Custom upload mode
        const customTab = page.locator('.mode-tab').nth(1)
        await customTab.click()

        await page.waitForTimeout(500)

        // Screenshot: Custom mode with dropzone
        await takeDevScreenshot(page, 'image-upload-modal-custom-mode')

        // Verify the dropzone is visible
        const dropzone = page.locator('.imageupload-dropzone')
        await expect(dropzone).toBeVisible()

        // Verify paste hint is visible
        const pasteHint = page.locator('.paste-hint')
        await expect(pasteHint).toBeVisible()

        // Verify the choose-file button is visible
        const chooseFileButton = dropzone.locator('.dropzone-btn')
        await expect(chooseFileButton).toBeVisible()

        // Screenshot: Full page for layout context
        await takeDevScreenshot(page, 'image-upload-modal-fullpage', { fullPage: true })
    })
})
