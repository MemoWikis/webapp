import { test, expect } from '@playwright/test';

test.describe('Settings Page Layout', () => {
  test.beforeEach(async ({ page }) => {
    await page.setViewportSize({ width: 1280, height: 800 });
    // Navigate to homepage
    await page.goto('http://localhost:3000/');

    // Open login modal - prefer HeaderGuest button if visible
    let loginButton = page.locator('.guest-header-container .login-btn');
    if (!await loginButton.isVisible()) {
         loginButton = page.locator('.nav-options-container .login-btn');
    }
    
    if (await loginButton.count() > 0) {
        await loginButton.first().click();
        
        // Fill login form
        await page.locator('input[name="login"]').fill('admin@memowikis.net');
        await page.locator('input[name="password"]').fill('test');
        
        // Find submit button in modal (using text 'Anmelden' or class)
        // Usually Modal has a primary button.
        await page.getByText('Anmelden').click();
        
        // Wait for login to complete
        // The header should verify login (e.g. .header-user-dropdown visible)
        await expect(page.locator('.header-user-dropdown')).toBeVisible({ timeout: 15000 });
    }
  });

  test('mobile layout alignment check', async ({ page }) => {
    // Go to settings
    await page.goto('http://localhost:3000/user/user-settings'); // Assuming URL (might be localized?)
    
    // Check if the URL redirected, maybe it contains 'settings' or 'einstellungen'?
    // The user-settings.vue page mounts at /user/user-settings by default (folder structure).
    
    // Set viewport to mobile
    await page.setViewportSize({ width: 375, height: 667 });
    
    // Wait for content
    await expect(page.locator('.settings-header')).toBeVisible();

    const titleH1 = page.locator('.settings-header h1');
    const firstSection = page.locator('.settings-section').first();
    const profilePic = page.locator('.profile-picture');
    
    const titleBox = await titleH1.boundingBox();
    const sectionBox = await firstSection.boundingBox();
    
    console.log(`Mobile Viewport (375px):`);
    console.log(`Title H1 X: ${titleBox?.x}`);
    console.log(`First Section X: ${sectionBox?.x}`);

    // Expect alignment (within 1px)
    expect(titleBox?.x).toBeCloseTo(sectionBox?.x || 0, 1);
  });
});
