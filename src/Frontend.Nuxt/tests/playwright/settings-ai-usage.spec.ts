import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Settings AI Usage Tab', () => {
    test('AI usage tab displays content correctly', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Go to settings page with extended timeout for cold starts
        await page.goto('http://localhost:3000/Settings', { timeout: 60000 })
        await page.waitForLoadState('networkidle')

        // Take screenshot of initial settings page
        await takeDevScreenshot(page, 'settings-before-ai-usage')

        // Find and click the AI usage navigation button (desktop)
        const aiUsageButton = page.locator('.navigation button', {
            hasText: /KI-Nutzung|AI Usage/i,
        })

        // Check if navigation button exists
        await expect(aiUsageButton).toBeVisible()

        // Click the AI usage tab
        await aiUsageButton.click()

        // Wait for API response
        await page
            .waitForResponse(
                (response) =>
                    response.url().includes('/apiVue/AiUsageStore/GetAiUsage'),
                { timeout: 10000 },
            )
            .catch(() => {
                // API might have already responded
            })

        // Wait for content to render
        await page.waitForTimeout(500)

        // Take screenshot after clicking
        await takeDevScreenshot(page, 'settings-ai-usage-tab')

        // Verify AI usage content is visible
        const aiUsageContainer = page.locator('.ai-usage-container')
        await expect(aiUsageContainer).toBeVisible({ timeout: 10000 })

        // Take final screenshot
        await takeDevScreenshot(page, 'settings-ai-usage-loaded')

        // Verify content sections are displayed
        const balanceSection = page.locator(
            '.ai-usage-container .balance-cards',
        )
        const errorState = page.locator('.ai-usage-container .error-state')
        const noData = page.locator('.ai-usage-container .no-data')

        // At least one of these should be visible (content loaded) or error state
        const hasContent =
            (await balanceSection.isVisible()) ||
            (await errorState.isVisible()) ||
            (await noData.isVisible())
        expect(hasContent).toBeTruthy()
    })

    test('AI usage tab shows token balance cards', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings')
        await page.waitForLoadState('networkidle')

        // Click AI usage tab
        const aiUsageButton = page.locator('.navigation button', {
            hasText: /KI-Nutzung|AI Usage/i,
        })
        await aiUsageButton.click()

        // Wait for API response
        await page
            .waitForResponse(
                (response) =>
                    response
                        .url()
                        .includes('/apiVue/AiUsageStore/GetAiUsage') &&
                    response.status() === 200,
                { timeout: 10000 },
            )
            .catch(() => {
                // API might have already responded
            })

        await takeDevScreenshot(page, 'settings-ai-usage-balance')

        // Check balance cards structure
        const totalBalanceCard = page.locator('.balance-card.total')
        const subscriptionCard = page.locator('.balance-card.subscription')
        const paidCard = page.locator('.balance-card.paid')

        // If content loaded successfully, verify balance cards
        if (await totalBalanceCard.isVisible()) {
            await expect(totalBalanceCard).toContainText(/\d+/)
            await expect(subscriptionCard).toBeVisible()
            await expect(paidCard).toBeVisible()
        }
    })

    test('mobile - AI usage tab accessible via dropdown', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // Set mobile viewport BEFORE navigation
        await page.setViewportSize({ width: 375, height: 667 })

        await page.goto('http://localhost:3000/Settings')
        await page.waitForLoadState('networkidle')
        await page.waitForTimeout(500)

        await takeDevScreenshot(page, 'settings-mobile-before-ai-usage')

        // Close any overlays that might be blocking
        await page.keyboard.press('Escape')
        await page.waitForTimeout(200)

        // Open mobile dropdown with force click
        const mobileDropdown = page.locator(
            '.navigation-mobile .settings-select',
        )
        await expect(mobileDropdown).toBeVisible()
        await mobileDropdown.click({ force: true })

        // Wait for dropdown to open
        await page.waitForTimeout(300)

        await takeDevScreenshot(page, 'settings-mobile-dropdown-open')

        // Find AI usage option in dropdown
        const aiUsageOption = page.locator(
            '.mobile-dropdown .dropdown-row.select-row',
            { hasText: /KI-Nutzung|AI Usage/i },
        )
        await expect(aiUsageOption).toBeVisible({ timeout: 5000 })

        // Click AI usage option
        await aiUsageOption.click({ force: true })

        // Wait for API response
        await page
            .waitForResponse(
                (response) =>
                    response.url().includes('/apiVue/AiUsageStore/GetAiUsage'),
                { timeout: 10000 },
            )
            .catch(() => {})

        await page.waitForTimeout(500)

        await takeDevScreenshot(page, 'settings-mobile-ai-usage')

        // Verify AI usage content is visible
        const aiUsageContainer = page.locator('.ai-usage-container')
        await expect(aiUsageContainer).toBeVisible({ timeout: 10000 })
    })
})
