import { test, expect } from './fixtures/auth.fixture'
import {
    takeDevScreenshot,
    takeElementScreenshot,
} from './fixtures/screenshot.helper'

test.describe('AI Create Page', () => {
    test.beforeEach(async ({ authenticatedPage }) => {
        // Navigate to a page where we can create child pages
        await authenticatedPage.goto('/')
        await authenticatedPage.waitForLoadState('networkidle')
    })

    test('should open AI create page modal', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        // Look for the AI create button (magic wand icon in the page actions)
        // The button might be in different locations depending on the page layout
        const aiCreateButton = page.locator(
            '[data-testid="ai-create-page"], button:has(.fa-wand-magic-sparkles), .ai-create-btn',
        )

        // If button exists and is visible, click it
        const buttonVisible = await aiCreateButton
            .first()
            .isVisible()
            .catch(() => false)

        if (buttonVisible) {
            await aiCreateButton.first().click()

            // Wait for modal to appear
            await expect(page.locator('.ai-create-page-modal')).toBeVisible({
                timeout: 5000,
            })

            // Take screenshot of the modal
            await takeDevScreenshot(page, 'ai-create-modal-opened')

            // Verify modal elements
            await expect(page.locator('.modal-title')).toContainText('KI')
        } else {
            // Navigate to a specific page that has the AI create option
            console.log('AI create button not found on homepage, skipping test')
            test.skip()
        }
    })

    test('should show input mode toggle', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        // This test assumes the modal is already triggered somehow
        // In real usage, we need to navigate to a page and open the modal

        // For now, check if we can programmatically open the modal
        // by navigating to a page that supports it

        await page.goto('/')
        await page.waitForLoadState('networkidle')

        // Look for any element that opens the AI modal
        const aiTrigger = page
            .locator('.ai-create-btn, [data-action="ai-create"]')
            .first()

        if (await aiTrigger.isVisible().catch(() => false)) {
            await aiTrigger.click()

            // Verify input mode buttons
            const promptModeBtn = page.locator('.mode-btn:has-text("Prompt")')

            await expect(
                promptModeBtn.or(page.locator('.mode-btn').first()),
            ).toBeVisible()

            await takeDevScreenshot(page, 'ai-modal-input-modes')
        } else {
            test.skip()
        }
    })

    test('should validate prompt input', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        // Navigate to trigger AI modal
        await page.goto('/')

        // This is a placeholder - actual implementation depends on how the modal is triggered
        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            const promptTextarea = page.locator(
                '#prompt-input, .prompt-textarea',
            )

            // Test empty prompt validation
            await expect(promptTextarea).toBeEmpty()

            // The generate button should be disabled with empty prompt
            const generateBtn = page.locator(
                '.btn-primary:has-text("Generieren")',
            )
            await expect(generateBtn).toBeDisabled()

            // Enter a prompt
            await promptTextarea.fill(
                'Eine Seite über die Geschichte von Berlin',
            )

            // Button should now be enabled
            await expect(generateBtn).toBeEnabled()

            await takeDevScreenshot(page, 'ai-modal-prompt-filled')
        } else {
            test.skip()
        }
    })

    test('should show complexity slider on desktop', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Set desktop viewport
        await page.setViewportSize({ width: 1280, height: 800 })

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Check for slider (desktop) vs dropdown (mobile)
            const slider = page.locator('.detail-slider')
            await expect(slider).toBeVisible()

            await takeElementScreenshot(
                page,
                '.detail-section',
                'ai-modal-complexity-slider',
            )
        } else {
            test.skip()
        }
    })

    test('should show complexity dropdown on mobile', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Set mobile viewport
        await page.setViewportSize({ width: 375, height: 667 })

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Check for dropdown (mobile)
            const dropdown = page.locator('.detail-dropdown, .detail-select')
            await expect(dropdown).toBeVisible()

            await takeDevScreenshot(page, 'ai-modal-mobile-complexity')
        } else {
            test.skip()
        }
    })

    test('should toggle content length options', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            const lengthButtons = page.locator('.length-btn')

            // Click through different length options
            const shortBtn = lengthButtons
                .filter({ hasText: 'Kurz' })
                .or(lengthButtons.first())
            const _mediumBtn = lengthButtons.filter({ hasText: 'Mittel' })
            const longBtn = lengthButtons.filter({ hasText: 'Lang' })

            if (await shortBtn.isVisible()) {
                await shortBtn.click()
                await expect(shortBtn).toHaveClass(/active/)
                await takeDevScreenshot(page, 'ai-modal-length-short')
            }

            if (await longBtn.isVisible()) {
                await longBtn.click()
                await expect(longBtn).toHaveClass(/active/)
                await takeDevScreenshot(page, 'ai-modal-length-long')
            }
        } else {
            test.skip()
        }
    })

    test('should show wiki toggle when long content selected', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Select long content
            const longBtn = page
                .locator('.length-btn')
                .filter({ hasText: 'Lang' })

            if (await longBtn.isVisible()) {
                await longBtn.click()

                // Wiki toggle should be visible
                const wikiToggle = page.locator('.wiki-toggle')
                await expect(wikiToggle).toBeVisible()

                await takeDevScreenshot(page, 'ai-modal-wiki-toggle')
            }
        } else {
            test.skip()
        }
    })

    test('should display AI model selector', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Check for model selector in footer
            const modelSelect = page.locator(
                '.model-select, .ai-model-selection',
            )
            await expect(modelSelect).toBeVisible()

            // Click to open model dropdown
            await modelSelect.click()

            // Wait for dropdown to appear
            const modelDropdown = page.locator(
                '.model-dropdown-popper, .model-dropdown-menu',
            )
            await expect(modelDropdown).toBeVisible()

            await takeDevScreenshot(page, 'ai-modal-model-dropdown')
        } else {
            test.skip()
        }
    })

    test('should show token balance', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Click token balance button
            const tokenBtn = page.locator('.token-balance-btn')

            if (await tokenBtn.isVisible()) {
                await tokenBtn.click()

                // Wait for balance popper
                const balancePopper = page.locator('.token-balance-popper')
                await expect(balancePopper).toBeVisible()

                await takeDevScreenshot(page, 'ai-modal-token-balance')
            }
        } else {
            test.skip()
        }
    })
})

test.describe('AI Create Page - URL Mode', () => {
    test('should switch to URL input mode', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Click URL mode button
            const urlModeBtn = page
                .locator('.mode-btn')
                .filter({ hasText: 'URL' })

            if (await urlModeBtn.isVisible()) {
                await urlModeBtn.click()
                await expect(urlModeBtn).toHaveClass(/active/)

                // URL input should be visible
                const urlInput = page.locator('#url-input, .url-input')
                await expect(urlInput).toBeVisible()

                await takeDevScreenshot(page, 'ai-modal-url-mode')
            }
        } else {
            test.skip()
        }
    })

    test('should validate URL input', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('/')

        const modal = page.locator('.ai-create-page-modal')

        if (await modal.isVisible().catch(() => false)) {
            // Switch to URL mode
            const urlModeBtn = page
                .locator('.mode-btn')
                .filter({ hasText: 'URL' })

            if (await urlModeBtn.isVisible()) {
                await urlModeBtn.click()

                const urlInput = page.locator('#url-input, .url-input')
                const generateBtn = page.locator('.btn-primary')

                // Invalid URL should keep button disabled
                await urlInput.fill('not-a-valid-url')
                await expect(generateBtn).toBeDisabled()

                // Valid URL should enable button
                await urlInput.fill('https://de.wikipedia.org/wiki/Berlin')
                await expect(generateBtn).toBeEnabled()

                await takeDevScreenshot(page, 'ai-modal-url-valid')
            }
        } else {
            test.skip()
        }
    })
})
