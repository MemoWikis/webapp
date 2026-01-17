import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('Settings Page Layout', () => {
    test('mobile layout alignment check', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        // Go to settings
        await page.goto('http://localhost:3000/user/user-settings')

        // Set viewport to mobile
        await page.setViewportSize({ width: 375, height: 667 })

        // Wait for content
        await expect(page.locator('.settings-header')).toBeVisible()

        await takeDevScreenshot(page, 'settings-mobile-layout')

        const titleH1 = page.locator('.settings-header h1')

        // On mobile, navigation is the dropdown
        const mobileNavLabel = page.locator('.settings-select > div').first()

        const titleBox = await titleH1.boundingBox()
        const navBox = await mobileNavLabel.boundingBox()

        console.log(`Mobile Viewport (375px):`)
        console.log(`Title H1 X: ${titleBox?.x}`)
        console.log(`Mobile Nav Label X: ${navBox?.x}`)

        // Expect alignment (within 1px)
        expect(navBox?.x).toBeCloseTo(titleBox?.x || 0, 1)
    })

    test('desktop layout alignment check', async ({ authenticatedPage }) => {
        const page = authenticatedPage

        await page.goto('http://localhost:3000/user/user-settings')
        await page.setViewportSize({ width: 1280, height: 800 })

        await expect(page.locator('.settings-header')).toBeVisible()

        await takeDevScreenshot(page, 'settings-desktop-layout')

        const titleH1 = page.locator('.settings-header h1')

        // On desktop, navigation is the list of buttons
        const desktopNavTitle = page.locator('.navigation .overline-s').first()
        const desktopNavButton = page.locator('.navigation button').first()

        const titleBox = await titleH1.boundingBox()
        const navTitleBox = await desktopNavTitle.boundingBox()
        const navButtonBox = await desktopNavButton.boundingBox()

        console.log(`Desktop Viewport ({ width: 1280, height: 800 }):`)
        console.log(`Title H1 X: ${titleBox?.x}`)
        console.log(`Nav Title X: ${navTitleBox?.x}`)
        console.log(`Nav Button X: ${navButtonBox?.x}`)
    })
})
