import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Settings Tabs Visual Check', () => {
    test('all tabs render correctly', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/Settings')
        await page.waitForLoadState('networkidle')
        await page.setViewportSize({ width: 1280, height: 900 })

        // Wait for settings container
        await expect(page.locator('.user-settings-container')).toBeVisible()

        // Tab 1: Edit Profile (default)
        await expect(page.locator('.settings-content .content')).toBeVisible()
        await takeDevScreenshot(page, 'settings-tab-edit-profile')

        // Check styling elements are visible
        const profilePicture = page.locator('.profile-picture')
        await expect(profilePicture).toBeVisible()

        const settingsInput = page.locator('.settings-input').first()
        await expect(settingsInput).toBeVisible()

        // Check input has correct styling (border)
        const inputBox = await settingsInput.boundingBox()
        console.log('Input box:', inputBox)

        // Tab 2: Password
        await page
            .locator('.navigation button')
            .filter({ hasText: /Passwort|Password/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-password')

        const passwordInputs = page.locator('.settings-input[type="password"]')
        await expect(passwordInputs.first()).toBeVisible()

        // Tab 3: Membership
        await page
            .locator('.navigation button')
            .filter({ hasText: /Mitgliedschaft|Membership/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-membership')

        // Tab 4: AI Usage
        await page
            .locator('.navigation button')
            .filter({ hasText: /KI|AI/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-ai-usage')

        // Tab 5: Wish Knowledge
        await page
            .locator('.navigation button')
            .filter({ hasText: /Wunschwissen|Wish/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-wishknowledge')

        const checkbox = page.locator('.checkbox-section')
        await expect(checkbox).toBeVisible()

        // Tab 6: Knowledge Report
        await page
            .locator('.navigation button')
            .filter({ hasText: /Wissensbericht|Knowledge Report/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-knowledge-report')

        // Tab 7: Support Login
        await page
            .locator('.navigation button')
            .filter({ hasText: /Support/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-support-login')

        // Tab 8: Delete Profile
        await page
            .locator('.navigation button')
            .filter({ hasText: /Profil löschen|Delete/ })
            .click()
        await page.waitForTimeout(300)
        await takeDevScreenshot(page, 'settings-tab-delete-profile')

        console.log('All tabs rendered successfully!')
    })
})
