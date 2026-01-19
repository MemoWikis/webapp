import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Settings Tab Routing', () => {
    test('direct URL to password tab works', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        // Navigate directly to password tab via URL
        await page.goto('http://localhost:3000/Settings?tab=password')
        await page.waitForLoadState('networkidle')

        // Password tab should be active
        const passwordButton = page
            .locator('.navigation button')
            .filter({ hasText: /Passwort|Password/ })
        await expect(passwordButton).toHaveClass(/active/)

        // Password inputs should be visible
        await expect(
            page.locator('.settings-input[type="password"]').first(),
        ).toBeVisible()

        await takeDevScreenshot(page, 'settings-route-password')
    })

    test('direct URL to ai-usage tab works', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings?tab=ai-usage')
        await page.waitForLoadState('networkidle')

        // AI Usage tab should be active
        const aiButton = page
            .locator('.navigation button')
            .filter({ hasText: /KI|AI/ })
        await expect(aiButton).toHaveClass(/active/)

        await takeDevScreenshot(page, 'settings-route-ai-usage')
    })

    test('direct URL to membership tab works', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings?tab=membership')
        await page.waitForLoadState('networkidle')

        const membershipButton = page
            .locator('.navigation button')
            .filter({ hasText: /Mitgliedschaft|Membership/ })
        await expect(membershipButton).toHaveClass(/active/)

        await takeDevScreenshot(page, 'settings-route-membership')
    })

    test('URL updates when clicking tabs', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings')
        await page.waitForLoadState('networkidle')

        // Default should be profile tab
        expect(page.url()).toContain('tab=profile')

        // Click password tab
        await page
            .locator('.navigation button')
            .filter({ hasText: /Passwort|Password/ })
            .click()
        await page.waitForTimeout(300)

        // URL should update
        expect(page.url()).toContain('tab=password')

        // Click AI usage tab
        await page
            .locator('.navigation button')
            .filter({ hasText: /KI|AI/ })
            .click()
        await page.waitForTimeout(300)

        expect(page.url()).toContain('tab=ai-usage')

        await takeDevScreenshot(page, 'settings-route-after-navigation')
    })

    test('browser back/forward works with tabs', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings?tab=profile')
        await page.waitForLoadState('networkidle')

        // Navigate to password
        await page
            .locator('.navigation button')
            .filter({ hasText: /Passwort|Password/ })
            .click()
        await page.waitForTimeout(300)
        expect(page.url()).toContain('tab=password')

        // Navigate to AI usage
        await page
            .locator('.navigation button')
            .filter({ hasText: /KI|AI/ })
            .click()
        await page.waitForTimeout(300)
        expect(page.url()).toContain('tab=ai-usage')

        // Go back
        await page.goBack()
        await page.waitForTimeout(300)
        expect(page.url()).toContain('tab=password')

        // Go back again
        await page.goBack()
        await page.waitForTimeout(300)
        expect(page.url()).toContain('tab=profile')

        // Go forward
        await page.goForward()
        await page.waitForTimeout(300)
        expect(page.url()).toContain('tab=password')
    })

    test('all tab routes work', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        const tabRoutes = [
            { route: 'profile', selector: /Profil bearbeiten|Edit Profile/ },
            { route: 'password', selector: /Passwort|Password/ },
            { route: 'membership', selector: /Mitgliedschaft|Membership/ },
            { route: 'ai-usage', selector: /KI|AI/ },
            { route: 'wishknowledge', selector: /Wunschwissen|Wish/ },
            {
                route: 'notifications',
                selector: /Wissensbericht|Knowledge Report/,
            },
            { route: 'support', selector: /Support/ },
            { route: 'delete', selector: /Profil löschen|Delete/ },
        ]

        for (const { route, selector } of tabRoutes) {
            await page.goto(`http://localhost:3000/Settings?tab=${route}`)
            await page.waitForLoadState('networkidle')

            const button = page
                .locator('.navigation button')
                .filter({ hasText: selector })
            await expect(button).toHaveClass(/active/)

            console.log(`✓ Route tab=${route} works`)
        }
    })
})
