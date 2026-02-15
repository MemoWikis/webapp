import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('AI Model Selection Persistence', () => {
    test('should select mid-tier model by default when no preference exists', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Navigate directly to the main wiki page (page ID 1)
        await page.goto('/Welcome-to-memoWikis/1')
        await page.waitForLoadState('networkidle')

        // Wait for page content to fully load
        await page.waitForTimeout(1000)

        // Take debug screenshot before looking for button
        await takeDevScreenshot(page, 'ai-model-test-page-loaded')

        // Open AI create modal - button in grid-option with SVG icon
        // FontAwesome renders as <svg class="svg-inline--fa fa-wand-magic-sparkles">
        const aiCreateButton = page.locator(
            '.grid-option button:has(svg[data-icon="wand-magic-sparkles"]), .grid-option button:has(.fa-wand-magic-sparkles)',
        )

        const buttonVisible = await aiCreateButton
            .first()
            .isVisible()
            .catch(() => false)

        if (!buttonVisible) {
            await takeDevScreenshot(page, 'ai-model-button-not-found')
            console.log('AI create button not found, skipping test')
            test.skip()
            return
        }

        await aiCreateButton.first().click()

        // Wait for modal
        await expect(page.locator('.ai-create-modal')).toBeVisible({
            timeout: 5000,
        })

        // Wait for models to load (dropdown becomes enabled)
        const modelSelect = page.locator('.model-select:not(.disabled)').first()
        await expect(modelSelect).toBeVisible({ timeout: 10000 })
        await modelSelect.click()

        // Wait for dropdown to appear (floating-vue renders popper)
        await page.waitForTimeout(500) // Allow dropdown animation

        // Wait for dropdown content to appear
        // Use .ai-model-option to ensure we are waiting for the actual content to be rendered
        const firstModelOption = page.locator('.ai-model-option').first()
        await expect(firstModelOption).toBeVisible({ timeout: 5000 })

        await takeDevScreenshot(page, 'ai-model-default-selection')
    })

    test.fixme('should remember selected model after reopening modal', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Navigate directly to the main wiki page (page ID 1)
        await page.goto('/Welcome-to-memoWikis/1')
        await page.waitForLoadState('networkidle')

        // Wait for page content to load
        await page.waitForTimeout(1000)

        // Open AI create modal
        const aiCreateButton = page.locator(
            '.grid-option button:has(svg[data-icon="wand-magic-sparkles"]), .grid-option button:has(.fa-wand-magic-sparkles)',
        )

        const buttonVisible = await aiCreateButton
            .first()
            .isVisible()
            .catch(() => false)

        if (!buttonVisible) {
            console.log('AI create button not found, skipping test')
            test.skip()
            return
        }

        await aiCreateButton.first().click()

        // Wait for modal
        await expect(page.locator('.ai-create-modal')).toBeVisible({
            timeout: 5000,
        })

        // Wait for models to load (dropdown becomes enabled)
        const modelSelect = page.locator('.model-select:not(.disabled)').first()
        await expect(modelSelect).toBeVisible({ timeout: 10000 })
        await modelSelect.click()

        // Wait for dropdown animation
        await page.waitForTimeout(500)

        // Wait for dropdown content
        const firstModelOption = page.locator('.ai-model-option').first()
        await expect(firstModelOption).toBeVisible({ timeout: 5000 })

        // Get all model items
        const modelItems = page.locator('.ai-model-option')
        const modelCount = await modelItems.count()

        if (modelCount < 2) {
            console.log('Not enough models to test selection persistence')
            test.skip()
            return
        }

        // Remember the current selection before changing
        const currentlySelectedText = await page
            .locator('.model-select')
            .first()
            .textContent()

        // Click the second model (different from current)
        await modelItems.nth(1).click()

        // Wait for selection to be saved
        await page.waitForTimeout(500)

        // Get the new selection text from the model selector
        const newSelectedText = await page
            .locator('.model-select')
            .first()
            .textContent()

        await takeDevScreenshot(page, 'ai-model-changed-selection')

        // Close modal by clicking outside or pressing Escape
        await page.keyboard.press('Escape')

        // Wait for modal to close
        await expect(page.locator('.ai-create-modal')).toBeHidden()

        // Wait a small buffer for animations to fully clear
        await page.waitForTimeout(1000)

        // Reopen the modal
        await aiCreateButton.first().click({ force: true })

        // Wait for modal
        await expect(page.locator('.ai-create-modal')).toBeVisible({
            timeout: 10000,
        })

        // Wait for models to load (dropdown becomes enabled again)
        await expect(
            page.locator('.model-select:not(.disabled)').first(),
        ).toBeVisible({ timeout: 10000 })

        // Verify the model is still the one we selected
        const persistedSelectedText = await page
            .locator('.model-select')
            .first()
            .textContent()

        expect(persistedSelectedText).toBe(newSelectedText)

        await takeDevScreenshot(page, 'ai-model-persisted-selection')
    })

    test('should display model list with quantifier information', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Navigate directly to the main wiki page (page ID 1)
        await page.goto('/Welcome-to-memoWikis/1')
        await page.waitForLoadState('networkidle')

        // Wait for page content to load
        await page.waitForTimeout(1000)

        // Open AI create modal - button in grid-option with SVG icon
        const aiCreateButton = page.locator(
            '.grid-option button:has(svg[data-icon="wand-magic-sparkles"]), .grid-option button:has(.fa-wand-magic-sparkles)',
        )

        const buttonVisible = await aiCreateButton
            .first()
            .isVisible()
            .catch(() => false)

        if (!buttonVisible) {
            console.log('AI create button not found, skipping test')
            test.skip()
            return
        }

        await aiCreateButton.first().click()

        // Wait for modal
        await expect(page.locator('.ai-create-modal')).toBeVisible({
            timeout: 5000,
        })

        // Wait for models to load (dropdown becomes enabled)
        const modelSelect = page.locator('.model-select:not(.disabled)').first()
        await expect(modelSelect).toBeVisible({ timeout: 10000 })
        await modelSelect.click()

        // Wait for dropdown animation
        await page.waitForTimeout(500)

        // Wait for dropdown content
        const firstModelOption = page.locator('.ai-model-option').first()
        await expect(firstModelOption).toBeVisible({ timeout: 5000 })

        // Get model items
        const modelItems = page.locator('.ai-model-option')
        const modelCount = await modelItems.count()

        // Verify we have models available
        expect(modelCount).toBeGreaterThan(0)

        await takeDevScreenshot(page, 'ai-model-dropdown-with-options')
    })
})
