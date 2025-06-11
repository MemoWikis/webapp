import {
    ref,
    computed,
    nextTick,
    onMounted,
    onUnmounted,
    watch,
    type Ref,
} from 'vue'
import { debounce } from 'underscore'
import type { VueElement } from 'vue'

export interface UseScrollbarSuppressionOptions {
    /**
     * Buffer in pixels to account for rounding differences
     * @default 5
     */
    buffer?: number

    /**
     * Debounce delay for resize events in milliseconds
     * @default 150
     */
    debounceDelay?: number

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
    const { buffer = 5, debounceDelay = 150, watchSources = [] } = options

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

    // Debounced version for resize events - waits for resize to stop
    const debouncedMeasureDimensions = debounce(
        measureDimensions,
        debounceDelay
    )

    // Watch for changes that might affect dimensions
    if (watchSources.length > 0) {
        watch(watchSources, measureDimensions, { flush: 'post' })
    }

    onMounted(() => {
        measureDimensions()
        window.addEventListener('resize', debouncedMeasureDimensions)
    })

    onUnmounted(() => {
        window.removeEventListener('resize', debouncedMeasureDimensions)
        debouncedMeasureDimensions.cancel?.()
    })

    return {
        suppressScrollX,
        containerWidth: readonly(containerWidth),
        totalElementsWidth: readonly(totalElementsWidth),
        measureDimensions,
    }
}
