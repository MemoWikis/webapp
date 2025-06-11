import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest'
import { ref, nextTick } from 'vue'
import { useScrollbarSuppression } from '~/composables/useScrollbarSuppression'

// Mock underscore throttle
vi.mock('underscore', () => ({
    throttle: vi.fn((fn) => {
        const throttled = fn as any
        throttled.cancel = vi.fn()
        return throttled
    }),
}))

// Mock getBoundingClientRect
const mockGetBoundingClientRect = vi.fn()

describe('useScrollbarSuppression', () => {
    beforeEach(() => {
        // Reset mocks
        vi.clearAllMocks()
        mockGetBoundingClientRect.mockReturnValue({ width: 100 })

        // Mock DOM methods
        Object.defineProperty(HTMLElement.prototype, 'getBoundingClientRect', {
            value: mockGetBoundingClientRect,
            writable: true,
        })

        // Mock window methods
        Object.defineProperty(window, 'addEventListener', {
            value: vi.fn(),
            writable: true,
        })
        Object.defineProperty(window, 'removeEventListener', {
            value: vi.fn(),
            writable: true,
        })
    })

    afterEach(() => {
        vi.restoreAllMocks()
    })

    it('should suppress scroll when total width is less than container width', async () => {
        const containerRef = ref({
            getBoundingClientRect: () => ({ width: 500 }),
        } as any)
        const elementRefs = [
            ref({ getBoundingClientRect: () => ({ width: 100 }) } as any),
            ref({ getBoundingClientRect: () => ({ width: 150 }) } as any),
            ref({ getBoundingClientRect: () => ({ width: 200 }) } as any),
        ]

        const { suppressScrollX, measureDimensions } = useScrollbarSuppression(
            containerRef,
            elementRefs,
            { buffer: 5 }
        )

        await measureDimensions()
        await nextTick()

        // Total width: 450, Container width: 500, Buffer: 5
        // 450 <= 505, so should suppress
        expect(suppressScrollX.value).toBe(true)
    })

    it('should not suppress scroll when total width exceeds container width plus buffer', async () => {
        const containerRef = ref({
            getBoundingClientRect: () => ({ width: 400 }),
        } as any)
        const elementRefs = [
            ref({ getBoundingClientRect: () => ({ width: 200 }) } as any),
            ref({ getBoundingClientRect: () => ({ width: 150 }) } as any),
            ref({ getBoundingClientRect: () => ({ width: 100 }) } as any),
        ]

        const { suppressScrollX, measureDimensions } = useScrollbarSuppression(
            containerRef,
            elementRefs,
            { buffer: 5 }
        )

        await measureDimensions()
        await nextTick()

        // Total width: 450, Container width: 400, Buffer: 5
        // 450 > 405, so should not suppress
        expect(suppressScrollX.value).toBe(false)
    })

    it('should handle null refs gracefully', async () => {
        const containerRef = ref(null)
        const elementRefs = [ref(null), ref(null)]

        const { suppressScrollX, measureDimensions } = useScrollbarSuppression(
            containerRef,
            elementRefs
        )

        await measureDimensions()
        await nextTick()

        // Should default to suppressing when no valid measurements
        expect(suppressScrollX.value).toBe(true)
    })
})
