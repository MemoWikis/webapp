import {
    ref,
    computed,
    nextTick,
    onMounted,
    onUnmounted,
    watch,
    type Ref,
} from 'vue'
import { throttle } from 'underscore'
import type { VueElement } from 'vue'

export interface UseScrollbarSuppressionOptions {
    /**
     * Buffer in pixels to account for rounding differences
     * @default 5
     */
    buffer?: number

    /**
     * Throttle delay for resize events in milliseconds
     * @default 100
     */
    throttleDelay?: number

    /**
     * Additional reactive values to watch for changes that might affect dimensions
     */
    watchSources?: Array<() => any>
}

/**
 * Composable for intelligently suppressing horizontal scrollbars based on actual content dimensions
 *
 * @param containerRef - Reference to the scrollable container element
 * @param elementRefs - Array of references to elements whose widths should be measured
 * @param options - Configuration options
 * @returns Object with suppressScrollX computed property and measurement values
 */
export function useScrollbarSuppression(
    containerRef: Ref<VueElement | null>,
    elementRefs: Array<Ref<VueElement | null>>,
    options: UseScrollbarSuppressionOptions = {}
) {
    const { buffer = 5, throttleDelay = 100, watchSources = [] } = options

    const containerWidth = ref(0)
    const totalElementsWidth = ref(0)

    const suppressScrollX = computed(() => {
        return totalElementsWidth.value <= containerWidth.value + buffer
    })

    // Function to measure actual DOM dimensions
    const measureDimensions = () => {
        nextTick(() => {
            // Measure container
            if (containerRef.value) {
                const containerRect = containerRef.value.getBoundingClientRect()
                containerWidth.value = containerRect.width
            }

            // Measure all elements
            let total = 0
            elementRefs.forEach((elementRef) => {
                if (elementRef.value) {
                    const rect = elementRef.value.getBoundingClientRect()
                    total += rect.width
                }
            })

            totalElementsWidth.value = total
        })
    }

    // Throttled version for resize events
    const throttledMeasureDimensions = throttle(
        measureDimensions,
        throttleDelay
    )

    // Watch for changes that might affect dimensions
    if (watchSources.length > 0) {
        watch(watchSources, measureDimensions, { flush: 'post' })
    }

    onMounted(() => {
        measureDimensions()
        window.addEventListener('resize', throttledMeasureDimensions)
    })

    onUnmounted(() => {
        window.removeEventListener('resize', throttledMeasureDimensions)
        throttledMeasureDimensions.cancel?.()
    })

    return {
        suppressScrollX,
        containerWidth: readonly(containerWidth),
        totalElementsWidth: readonly(totalElementsWidth),
        measureDimensions,
    }
}
