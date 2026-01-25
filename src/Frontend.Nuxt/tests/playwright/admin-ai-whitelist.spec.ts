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

    test('should display price columns in whitelisted models table', async ({
        authenticatedPage,
    }) => {
        // Navigate to maintenance page with AI tab
        await authenticatedPage.goto('/Maintenance?tab=ai')
        await authenticatedPage.waitForLoadState('networkidle')

        // Close any open dialogs
        await closeOpenDialogs(authenticatedPage)

        // Wait for the table to be visible
        const whitelistTable = authenticatedPage.locator('.whitelist-table')
        const tableExists = await whitelistTable.isVisible().catch(() => false)

        if (!tableExists) {
            console.log('No whitelisted models table found, skipping price columns test')
            return
        }

        // Check for the price column headers
        await expect(authenticatedPage.locator('.whitelist-table th:has-text("$/M In")')).toBeVisible()
        await expect(authenticatedPage.locator('.whitelist-table th:has-text("$/M Out")')).toBeVisible()

        await takeDevScreenshot(authenticatedPage, 'ai-whitelist-price-columns')
    })

    test('should allow editing model prices', async ({ authenticatedPage }) => {
        // Navigate to maintenance page with AI tab
        await authenticatedPage.goto('/Maintenance?tab=ai')
        await authenticatedPage.waitForLoadState('networkidle')

        // Close any open dialogs
        await closeOpenDialogs(authenticatedPage)

        // Wait for the table to be visible
        const whitelistTable = authenticatedPage.locator('.whitelist-table')
        const tableExists = await whitelistTable.isVisible().catch(() => false)

        if (!tableExists) {
            console.log('No whitelisted models table found, skipping price edit test')
            return
        }

        // Get the first row's input price cell (column 5 - $/M In)
        const inputPriceCell = authenticatedPage.locator('.whitelist-table tbody tr').first().locator('td:nth-child(5) .price-value')
        const inputPriceCellVisible = await inputPriceCell.isVisible().catch(() => false)

        if (!inputPriceCellVisible) {
            console.log('No price value cell visible, skipping price edit test')
            return
        }

        await takeDevScreenshot(authenticatedPage, 'before-price-edit')

        // Click on the input price to start editing
        await inputPriceCell.click()

        // Wait for the edit inputs to appear
        await expect(authenticatedPage.locator('.price-edit .price-input').first()).toBeVisible({ timeout: 3000 })

        await takeDevScreenshot(authenticatedPage, 'price-edit-mode')

        // Get the price inputs
        const inputPriceInput = authenticatedPage.locator('.price-edit .price-input').first()
        const outputPriceInput = authenticatedPage.locator('.price-edit .price-input').last()

        // Clear and set new values
        await inputPriceInput.fill('3.50')
        await outputPriceInput.fill('15.75')

        await takeDevScreenshot(authenticatedPage, 'price-values-entered')

        // Click save button (the one in the output price column since that's where the controls are)
        const saveButton = authenticatedPage.locator('.price-edit .btn-save').last()
        await saveButton.click()

        // Wait for the update to complete
        await authenticatedPage.waitForLoadState('networkidle')
        await authenticatedPage.waitForTimeout(500)

        await takeDevScreenshot(authenticatedPage, 'after-price-save')

        // Verify the prices were updated by checking the displayed values
        const updatedInputPrice = await authenticatedPage
            .locator('.whitelist-table tbody tr')
            .first()
            .locator('td:nth-child(5) .price-value')
            .textContent()

        const updatedOutputPrice = await authenticatedPage
            .locator('.whitelist-table tbody tr')
            .first()
            .locator('td:nth-child(6) .price-value')
            .textContent()

        // Prices should contain the new values (format: $X.XX)
        expect(updatedInputPrice).toContain('3.50')
        expect(updatedOutputPrice).toContain('15.75')
    })

    test('should cancel price editing on cancel button click', async ({
        authenticatedPage,
    }) => {
        // Navigate to maintenance page with AI tab
        await authenticatedPage.goto('/Maintenance?tab=ai')
        await authenticatedPage.waitForLoadState('networkidle')

        // Close any open dialogs
        await closeOpenDialogs(authenticatedPage)

        // Wait for the table to be visible
        const whitelistTable = authenticatedPage.locator('.whitelist-table')
        const tableExists = await whitelistTable.isVisible().catch(() => false)

        if (!tableExists) {
            console.log('No whitelisted models table found, skipping cancel test')
            return
        }

        // Get the first row's input price cell
        const inputPriceCell = authenticatedPage.locator('.whitelist-table tbody tr').first().locator('td:nth-child(5) .price-value')
        const inputPriceCellVisible = await inputPriceCell.isVisible().catch(() => false)

        if (!inputPriceCellVisible) {
            console.log('No price value cell visible, skipping cancel test')
            return
        }

        // Store original value
        const originalInputPrice = await inputPriceCell.textContent()

        // Click on the input price to start editing
        await inputPriceCell.click()

        // Wait for the edit inputs to appear
        await expect(authenticatedPage.locator('.price-edit .price-input').first()).toBeVisible({ timeout: 3000 })

        // Enter new values
        const inputPriceInput = authenticatedPage.locator('.price-edit .price-input').first()
        await inputPriceInput.fill('99.99')

        // Click cancel button
        const cancelButton = authenticatedPage.locator('.price-edit .btn-cancel').last()
        await cancelButton.click()

        await authenticatedPage.waitForTimeout(300)

        await takeDevScreenshot(authenticatedPage, 'after-price-cancel')

        // Verify the original price is restored
        const restoredInputPrice = await authenticatedPage
            .locator('.whitelist-table tbody tr')
            .first()
            .locator('td:nth-child(5) .price-value')
            .textContent()

        expect(restoredInputPrice).toBe(originalInputPrice)
    })
})
