import { type Page, expect as baseExpect } from '@playwright/test'
import * as fs from 'fs'
import * as path from 'path'

const SCREENSHOT_DIR = 'test-results/screenshots'

/**
 * Ensures the screenshot directory exists
 */
function ensureScreenshotDir(): void {
    if (!fs.existsSync(SCREENSHOT_DIR)) {
        fs.mkdirSync(SCREENSHOT_DIR, { recursive: true })
    }
}

/**
 * Takes a development screenshot and saves it with a descriptive name.
 * Useful for visual feedback during development.
 *
 * @param page - Playwright page instance
 * @param name - Descriptive name for the screenshot (will be sanitized)
 * @param options - Additional screenshot options
 */
export async function takeDevScreenshot(
    page: Page,
    name: string,
    options?: {
        fullPage?: boolean
        clip?: { x: number; y: number; width: number; height: number }
    },
): Promise<string> {
    ensureScreenshotDir()

    // Sanitize filename
    const sanitizedName = name.replace(/[^a-zA-Z0-9-_]/g, '-').toLowerCase()
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-')
    const filename = `${sanitizedName}-${timestamp}.png`
    const filePath = path.join(SCREENSHOT_DIR, filename)

    await page.screenshot({
        path: filePath,
        fullPage: options?.fullPage ?? false,
        clip: options?.clip,
    })

    console.log(`📸 Screenshot saved: ${filePath}`)
    return filePath
}

/**
 * Takes a screenshot of a specific element
 */
export async function takeElementScreenshot(
    page: Page,
    selector: string,
    name: string,
): Promise<string> {
    ensureScreenshotDir()

    const element = page.locator(selector)
    await element.waitFor({ state: 'visible', timeout: 5000 })

    const sanitizedName = name.replace(/[^a-zA-Z0-9-_]/g, '-').toLowerCase()
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-')
    const filename = `${sanitizedName}-${timestamp}.png`
    const filePath = path.join(SCREENSHOT_DIR, filename)

    await element.screenshot({ path: filePath })

    console.log(`📸 Element screenshot saved: ${filePath}`)
    return filePath
}

/**
 * Clears all screenshots from the development screenshot directory
 */
export function clearScreenshots(): void {
    if (fs.existsSync(SCREENSHOT_DIR)) {
        const files = fs.readdirSync(SCREENSHOT_DIR)
        for (const file of files) {
            if (file.endsWith('.png')) {
                fs.unlinkSync(path.join(SCREENSHOT_DIR, file))
            }
        }
        console.log(`🗑️ Cleared ${files.length} screenshots`)
    }
}

/**
 * Extended expect with screenshot comparison helpers
 */
export const expect = baseExpect
