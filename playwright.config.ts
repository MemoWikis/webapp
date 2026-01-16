import { defineConfig, devices } from '@playwright/test'

export default defineConfig({
  testDir: './src/Frontend.Nuxt/tests/playwright',
  timeout: 30000,
  use: {
    baseURL: 'http://localhost:3000',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
})
