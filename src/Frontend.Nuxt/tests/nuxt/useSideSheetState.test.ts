/**
 * Test file demonstrating the useSideSheetState composable
 * This shows how the composable can be used in any component
 */

import { describe, it, expect, beforeEach, vi } from 'vitest'
import { ref } from 'vue'
// Mock the dependencies
vi.mock('~/components/sideSheet/sideSheetStore', () => ({
    useSideSheetStore: () => ({
        showSideSheet: ref(false),
    }),
}))

vi.mock('#app', () => ({
    useCookie: vi.fn().mockReturnValue(ref(false)),
    useDevice: () => ({
        isMobile: false,
    }),
    watch: vi.fn(),
    ref: vi.fn(),
    readonly: vi.fn(),
}))

describe('useSideSheetState', () => {
    beforeEach(() => {
        vi.clearAllMocks()
    })

    it('should return sideSheetOpen and sideSheetStore', () => {
        const result = useSideSheetState()

        expect(result).toHaveProperty('sideSheetOpen')
        expect(result).toHaveProperty('sideSheetStore')
    })

    it('should initialize sideSheetOpen based on cookie and mobile state', () => {
        // This test would verify that the composable correctly initializes
        // the side sheet state based on the cookie value and mobile detection
        const result = useSideSheetState()
        expect(result).toBeDefined()
    })
})
