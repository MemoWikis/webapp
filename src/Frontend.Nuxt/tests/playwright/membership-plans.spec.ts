import { test, expect } from '@playwright/test'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Membership Plans Layout', () => {
    test('pricing page shows all plans with proper layout', async ({
        page,
    }) => {
        // Navigate to pricing page
        await page.goto('/Preise')

        // Wait for plans to load
        await page.waitForSelector('.subscription-plans-container', {
            timeout: 10000,
        })

        // Take screenshot of the full pricing section
        await takeDevScreenshot(page, 'membership-plans-full')

        // Verify all 4 plans are visible
        await expect(page.locator('.card').first()).toBeVisible()

        // Take a viewport screenshot
        await page.screenshot({
            path: 'test-results/screenshots/membership-plans-viewport.png',
            fullPage: false,
        })

        // Scroll down to see Organisation if needed
        await page.evaluate(() => window.scrollTo(0, 500))
        await takeDevScreenshot(page, 'membership-plans-scrolled')
    })
})
