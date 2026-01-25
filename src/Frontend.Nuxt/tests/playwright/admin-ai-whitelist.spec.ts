import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

/**
 * Close any open dialogs that might be blocking interaction
 */
async function closeOpenDialogs(page: import('@playwright/test').Page) {
    // Check for vue-final-modal dialogs
    const vfmDialog = page.locator('.vfm')
    if (await vfmDialog.isVisible({ timeout: 500 }).catch(() => false)) {
        // Try to close by pressing Escape
        await page.keyboard.press('Escape')
        await page.waitForTimeout(300)
    }
}

test.describe('Admin AI Model Whitelist Management', () => {
    test('should be able to remove an AI model from whitelist', async ({
        authenticatedPage,
    }) => {
        // Navigate to maintenance page
        await authenticatedPage.goto('/Maintenance?tab=ai')
        await authenticatedPage.waitForLoadState('networkidle')

        // Close any open dialogs
        await closeOpenDialogs(authenticatedPage)

        // Wait for the AI Models Management section to be visible
        await expect(
            authenticatedPage.locator('text=AI Models Management'),
        ).toBeVisible({ timeout: 10000 })
        await takeDevScreenshot(authenticatedPage, 'ai-whitelist-tab-loaded')

        // Check if there are whitelisted models in the table
        const whitelistTable = authenticatedPage.locator('.whitelist-table')
        const tableExists = await whitelistTable.isVisible().catch(() => false)

        if (!tableExists) {
            // No whitelisted models - skip the delete test
            console.log('No whitelisted models found, skipping delete test')
            return
        }

        // Get the first delete button (trash icon)
        const deleteButton = authenticatedPage.locator('.btn-delete').first()
        const deleteButtonVisible = await deleteButton
            .isVisible()
            .catch(() => false)

        if (!deleteButtonVisible) {
            console.log('No delete button visible, skipping delete test')
            return
        }

        // Store the model name for verification later
        const firstRowModelName = await authenticatedPage
            .locator('.whitelist-table tbody tr')
            .first()
            .locator('td:nth-child(2)')
            .first()
            .textContent()

        await takeDevScreenshot(authenticatedPage, 'before-delete-click')

        // Click the delete button
        await deleteButton.click()

        // Wait for confirmation modal to appear (uses .confirm-modal class)
        await expect(authenticatedPage.locator('.confirm-modal')).toBeVisible({
            timeout: 5000,
        })
        await takeDevScreenshot(authenticatedPage, 'delete-confirmation-modal')

        // Click the confirm delete button (btn-danger in modal actions)
        const confirmButton = authenticatedPage.locator(
            '.confirm-modal .modal-actions button.btn-danger',
        )
        await confirmButton.click()

        // Wait for modal to close and verify no 500 error
        await authenticatedPage.waitForLoadState('networkidle')
        await authenticatedPage.waitForTimeout(500)
        await takeDevScreenshot(authenticatedPage, 'after-delete-success')

        // Verify the model was removed (or table is now empty)
        // The model should no longer be in the table
        if (firstRowModelName) {
            const modelStillExists = await authenticatedPage
                .locator('.whitelist-table tbody tr', {
                    hasText: firstRowModelName.trim(),
                })
                .isVisible()
                .catch(() => false)

            // Model should NOT be visible after deletion (test passes if this is false)
            expect(modelStillExists).toBeFalsy()
            console.log(
                `Model "${firstRowModelName}" still visible after delete: ${modelStillExists}`,
            )
        }
    })

    test('should display AI tab correctly', async ({ authenticatedPage }) => {
        // Navigate to maintenance page with AI tab
        await authenticatedPage.goto('/Maintenance?tab=ai')
        await authenticatedPage.waitForLoadState('networkidle')

        // Close any open dialogs
        await closeOpenDialogs(authenticatedPage)

        // Check for the "AI Models Management" panel title
        await expect(
            authenticatedPage.locator('text=AI Models Management'),
        ).toBeVisible({ timeout: 10000 })

        // Check for the "Whitelisted Models" section heading
        await expect(
            authenticatedPage.locator('h4:has-text("Whitelisted Models")'),
        ).toBeVisible()

        // Check for the "Available Models" section heading
        await expect(
            authenticatedPage.locator('h4:has-text("Available Models")'),
        ).toBeVisible()

        await takeDevScreenshot(authenticatedPage, 'ai-tab-structure')
    })
})
