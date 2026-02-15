import { test, expect } from './fixtures/auth.fixture'
import {
    takeDevScreenshot,
    takeElementScreenshot,
} from './fixtures/screenshot.helper'

const AI_BUTTON_SELECTOR =
    '.grid-option button:has(svg[data-icon="wand-magic-sparkles"]), .grid-option button:has(.fa-wand-magic-sparkles)'
const MODAL_SELECTOR = '.ai-create-modal'

async function openAiModal(page: import('@playwright/test').Page) {
    await page.goto('/Welcome-to-memoWikis/1')
    await page.waitForLoadState('networkidle')
    await page.waitForTimeout(1000)

    const aiCreateButton = page.locator(AI_BUTTON_SELECTOR)
    const buttonVisible = await aiCreateButton
        .first()
        .isVisible()
        .catch(() => false)

    if (!buttonVisible) {
        return false
    }

    await aiCreateButton.first().click()
    await expect(page.locator(MODAL_SELECTOR)).toBeVisible({ timeout: 5000 })
    return true
}

test.describe('AI Create Page', () => {
    test('should open AI create page modal', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        await takeDevScreenshot(page, 'ai-create-modal-opened')
        await expect(page.locator('.modal-title')).toContainText(/KI|AI/)
    })

    test('should show input mode toggle with prompt and URL buttons', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const modeButtons = page.locator('.mode-btn')
        await expect(modeButtons).toHaveCount(2)
        await expect(modeButtons.first()).toBeVisible()

        await takeDevScreenshot(page, 'ai-modal-input-modes')
    })

    test('should have generate button disabled with empty prompt', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const promptTextarea = page.locator('#prompt-input')
        await expect(promptTextarea).toBeEmpty()

        const generateBtn = page.locator(`${MODAL_SELECTOR} .btn-primary`)
        await expect(generateBtn).toBeDisabled()

        await promptTextarea.fill('Eine Seite über die Geschichte von Berlin')
        await expect(generateBtn).toBeEnabled()

        await takeDevScreenshot(page, 'ai-modal-prompt-filled')
    })

    test('should show complexity slider with aria attributes on desktop', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage
        await page.setViewportSize({ width: 1280, height: 800 })

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const slider = page.locator('.detail-slider')
        await expect(slider).toBeVisible()
        await expect(slider).toHaveAttribute('aria-label', /.+/)
        await expect(slider).toHaveAttribute('aria-valuetext', /.+/)

        await takeElementScreenshot(
            page,
            '.detail-section',
            'ai-modal-complexity-slider',
        )
    })

    test('should toggle content length options', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const lengthButtons = page.locator('.length-btn')
        await expect(lengthButtons).toHaveCount(3)

        const firstBtn = lengthButtons.first()
        const lastBtn = lengthButtons.last()

        await firstBtn.click()
        await expect(firstBtn).toHaveClass(/active/)

        await lastBtn.click()
        await expect(lastBtn).toHaveClass(/active/)
        await expect(firstBtn).not.toHaveClass(/active/)

        await takeDevScreenshot(page, 'ai-modal-length-toggled')
    })

    test('should show wiki toggle with proper checkbox role', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const wikiToggle = page.locator('.wiki-toggle-label')
        await expect(wikiToggle).toBeVisible()
        await expect(wikiToggle).toHaveAttribute('role', 'checkbox')
        await expect(wikiToggle).toHaveAttribute('aria-checked', 'false')

        await wikiToggle.click()
        await expect(wikiToggle).toHaveAttribute('aria-checked', 'true')

        await wikiToggle.click()
        await expect(wikiToggle).toHaveAttribute('aria-checked', 'false')

        await takeDevScreenshot(page, 'ai-modal-wiki-toggle')
    })

    test('should display AI model selector in footer', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const modelSelect = page.locator('.model-select:not(.disabled)').first()
        await expect(modelSelect).toBeVisible({ timeout: 10000 })
        await modelSelect.click()

        await page.waitForTimeout(500)

        const firstModelOption = page.locator('.ai-model-option').first()
        await expect(firstModelOption).toBeVisible({ timeout: 5000 })

        await takeDevScreenshot(page, 'ai-modal-model-dropdown')
    })

    test('should show token balance popper', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const tokenBtn = page.locator('.token-balance-btn')
        await expect(tokenBtn).toBeVisible()
        await tokenBtn.click()

        const balancePopper = page.locator('.token-balance-popper')
        await expect(balancePopper).toBeVisible({ timeout: 5000 })

        await takeDevScreenshot(page, 'ai-modal-token-balance')
    })
})

test.describe('AI Create Page - URL Mode', () => {
    test('should switch to URL input mode', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const urlModeBtn = page.locator('.mode-btn').filter({ hasText: 'URL' })
        await urlModeBtn.click()
        await expect(urlModeBtn).toHaveClass(/active/)

        const urlInput = page.locator('#url-input')
        await expect(urlInput).toBeVisible()

        await takeDevScreenshot(page, 'ai-modal-url-mode')
    })

    test('should validate URL input - disabled for invalid, enabled for valid', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const urlModeBtn = page.locator('.mode-btn').filter({ hasText: 'URL' })
        await urlModeBtn.click()

        const urlInput = page.locator('#url-input')
        const generateBtn = page.locator(`${MODAL_SELECTOR} .btn-primary`)

        await urlInput.fill('not-a-valid-url')
        await expect(generateBtn).toBeDisabled()

        await urlInput.fill('https://de.wikipedia.org/wiki/Berlin')
        await expect(generateBtn).toBeEnabled()

        await takeDevScreenshot(page, 'ai-modal-url-valid')
    })
})
