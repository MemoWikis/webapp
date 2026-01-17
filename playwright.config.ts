import { defineConfig, devices } from '@playwright/test'

export default defineConfig({
  testDir: './src/Frontend.Nuxt/tests/playwright',
  timeout: 30000,

  // Screenshot settings for development feedback
  outputDir: './test-results/artifacts',

  use: {
    baseURL: 'http://localhost:3000',

    // Capture screenshots on failure and optionally on success
    screenshot: 'only-on-failure',

    // Capture trace on failure for debugging
    trace: 'on-first-retry',

    // Video recording (optional, useful for complex flows)
    video: 'retain-on-failure',
  },

  // Reporter configuration
  reporter: [
    ['list'],
    ['html', { outputFolder: './playwright-report', open: 'never' }],
  ],

  // Expect configuration
  expect: {
    // Screenshot comparison threshold
    toHaveScreenshot: {
      maxDiffPixels: 100,
    },
  },

  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
    {
      name: 'mobile',
      use: { ...devices['iPhone 13'] },
    },
  ],
})
