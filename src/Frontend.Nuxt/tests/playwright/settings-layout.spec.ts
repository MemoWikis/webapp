import { test, expect } from '@playwright/test'

test.describe('Settings Page Layout', () => {
    test.beforeEach(async ({ page }) => {
        await page.setViewportSize({ width: 1280, height: 800 })
        // Navigate to homepage
        await page.goto('http://localhost:3000/')

        // Open login modal
        const loginBtn = page.locator('.login-btn').first()
        if (await loginBtn.isVisible()) {
            await loginBtn.click()
            await page.waitForTimeout(1000) // Wait for animation

            // Fill login form
            await page.getByPlaceholder('').first().fill('admin@memowikis.net') // Input often has empty placeholder or use name
            // Better use name selector if robust
            const loginInput = page.locator('input[name="login"]')
            await loginInput.waitFor({ state: 'visible', timeout: 5000 })
            await loginInput.fill('admin@memowikis.net')
            await page.locator('input[name="password"]').fill('test')

            // Find submit button in modal (using text 'Anmelden' or class)
            // Usually Modal has a primary button.
            await page.getByText('Anmelden').click()

            // Wait for login to complete
            // The header should verify login (e.g. .header-user-dropdown visible)
            await expect(page.locator('.header-user-dropdown')).toBeVisible({
                timeout: 15000,
            })
        }
    })

    test('mobile layout alignment check', async ({ page }) => {
        // Go to settings
        await page.goto('http://localhost:3000/user/user-settings')

        // Set viewport to mobile
        await page.setViewportSize({ width: 375, height: 667 })

        // Wait for content
        await expect(page.locator('.settings-header')).toBeVisible()

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

    test('desktop layout alignment check', async ({ page }) => {
        await page.goto('http://localhost:3000/user/user-settings')
        await page.setViewportSize({ width: 1280, height: 800 })

        await expect(page.locator('.settings-header')).toBeVisible()

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
