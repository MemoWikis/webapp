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

    test('should show content type tabs for Page, Wiki, Flashcards', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const tabs = page.locator('.content-tab')
        await expect(tabs).toHaveCount(3)
        await expect(tabs.first()).toHaveClass(/active/)

        await takeDevScreenshot(page, 'ai-modal-content-tabs')
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

        const slider = page.locator('.detail-slider').first()
        await expect(slider).toBeVisible()
        await expect(slider).toHaveAttribute('aria-label', /.+/)
        await expect(slider).toHaveAttribute('aria-valuetext', /.+/)

        const complexitySection = page.locator('.detail-section').first()
        await takeDevScreenshot(page, 'ai-modal-complexity-slider')
    })

    test('should show content length slider on desktop', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage
        await page.setViewportSize({ width: 1280, height: 800 })

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const sliders = page.locator('.detail-slider')
        await expect(sliders).toHaveCount(2)

        const lengthSlider = sliders.nth(1)
        await expect(lengthSlider).toBeVisible()
        await expect(lengthSlider).toHaveAttribute('aria-label', /.+/)
        await expect(lengthSlider).toHaveAttribute('aria-valuetext', /.+/)

        await takeDevScreenshot(page, 'ai-modal-length-slider')
    })

    test('should switch content type to Wiki tab', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const wikiTab = page.locator('.content-tab').nth(1)
        await wikiTab.click()
        await expect(wikiTab).toHaveClass(/active/)

        await takeDevScreenshot(page, 'ai-modal-wiki-tab')
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
    test('should show URL input when clicking add from URL link', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        const opened = await openAiModal(page)
        if (!opened) {
            test.skip()
            return
        }

        const addUrlBtn = page.locator('.add-url-btn')
        await expect(addUrlBtn).toBeVisible()
        await addUrlBtn.click()

        const urlInput = page.locator('#url-input')
        await expect(urlInput).toBeVisible()

        await takeDevScreenshot(page, 'ai-modal-url-expanded')
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

        const addUrlBtn = page.locator('.add-url-btn')
        await addUrlBtn.click()

        const urlInput = page.locator('#url-input')
        const generateBtn = page.locator(`${MODAL_SELECTOR} .btn-primary`)

        await urlInput.fill('not-a-valid-url')
        await expect(generateBtn).toBeDisabled()

        await urlInput.fill('https://de.wikipedia.org/wiki/Berlin')
        await expect(generateBtn).toBeEnabled()

        await takeDevScreenshot(page, 'ai-modal-url-valid')
    })
})
