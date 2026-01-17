import { test, expect } from '@playwright/test'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

/**
 * Debug test to understand the login flow and fix selectors
 */
test.describe('Debug Login Flow', () => {
    test('check homepage and login button', async ({ page }) => {
        // Navigate to homepage
        await page.goto('http://localhost:3000/')
        await page.waitForLoadState('networkidle')

        // Take screenshot of initial state
        await takeDevScreenshot(page, '01-homepage-loaded')

        // Check what login button variants exist
        const loginBtnClass = page.locator('.login-btn')
        const loginBtnText = page.locator('button:has-text("Log in")')
        const anmeldenBtn = page.locator('button:has-text("Anmelden")')

        console.log('Login button with .login-btn class:', await loginBtnClass.count())
        console.log('Login button with "Log in" text:', await loginBtnText.count())
        console.log('Login button with "Anmelden" text:', await anmeldenBtn.count())

        // Try to click the login button
        const loginButton = page.locator('.login-btn').first()

        if ((await loginButton.count()) > 0) {
            await loginButton.click()
            console.log('Clicked .login-btn')
        } else {
            console.log('No .login-btn found, trying alternative selectors')
            const altButton = page
                .locator('button:has-text("Log in"), button:has-text("Anmelden")')
                .first()
            if ((await altButton.count()) > 0) {
                await altButton.click()
                console.log('Clicked alternative login button')
            } else {
                console.log('ERROR: No login button found!')
                await takeDevScreenshot(page, '01-no-login-button')
                return
            }
        }

        // Wait for modal to appear
        await page.waitForTimeout(1500)
        await takeDevScreenshot(page, '02-after-login-click')

        // Check if modal appeared
        const modal = page.locator('#LoginModalComponent, .modal, [role="dialog"]')
        console.log('Modal elements found:', await modal.count())

        // Check for login input
        const loginInput = page.locator('input[name="login"]')
        const emailInput = page.locator('input[type="email"]')
        const passwordInput = page.locator('input[name="password"]')

        console.log('input[name="login"]:', await loginInput.count())
        console.log('input[type="email"]:', await emailInput.count())
        console.log('input[name="password"]:', await passwordInput.count())

        // If login input exists, try to fill it
        if ((await loginInput.count()) > 0) {
            await loginInput.fill('admin@memowikis.net')
            await passwordInput.fill('test')
            await takeDevScreenshot(page, '03-form-filled')

            // Find submit button in modal footer (not in header!)
            const modalFooter = page.locator('.modal-footer')
            const submitBtn = modalFooter.locator('.btn-primary, button')
            console.log('Submit buttons in modal footer:', await submitBtn.count())

            if ((await submitBtn.count()) > 0) {
                await submitBtn.first().click()
                await page.waitForTimeout(2000)
                await takeDevScreenshot(page, '04-after-submit')

                // Check if login succeeded
                const userDropdown = page.locator('.header-user-dropdown')
                if ((await userDropdown.count()) > 0) {
                    console.log('SUCCESS: Login completed!')
                } else {
                    console.log('Login may have failed - checking for errors')
                    const errorMsg = page.locator('.errorMessage, .error, .alert-danger')
                    if ((await errorMsg.count()) > 0) {
                        console.log('Error message:', await errorMsg.first().textContent())
                    }
                }
            }
        } else {
            console.log('ERROR: Login input not found in modal')
        }
    })
})
