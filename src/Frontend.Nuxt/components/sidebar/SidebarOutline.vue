<script lang="ts" setup>
import { throttle } from 'underscore'

interface Section {
    id: string
    translationKey: string
}

interface Props {
    sections: Section[]
    containerId?: string
    visibleSections?: string[]
    offset?: number
}

const props = withDefaults(defineProps<Props>(), {
    containerId: 'Outline',
    visibleSections: () => [],
    offset: 180
})

const { t } = useI18n()
const currentHeadingId = ref<string | null>(null)
const cancelToken = ref<number>(0)

const getVisibleSections = computed(() => {
    if (props.visibleSections.length === 0) return props.sections
    return props.sections.filter(section => props.visibleSections.includes(section.id))
})

const getCurrentHeadingId = () => {
    const visibleSections = getVisibleSections.value
    if (visibleSections.length === 0) return

    const headings = visibleSections
    const offset = props.offset || 180
    let headingId: string | null = headings[0].id

    const startIndex = headings.findIndex(h => h.id === currentHeadingId.value)

    // Check if user has scrolled to the bottom of the page
    const isScrolledToBottom = window.innerHeight + window.scrollY >= document.body.offsetHeight - 40

    if (isScrolledToBottom) {
        // Use the last heading when scrolled to bottom
        headingId = headings[headings.length - 1].id
    } else {
        // Find the best matching section based on visibility and position
        let bestMatch: { id: string; score: number } | null = null

        for (let i = 0; i < headings.length; i++) {
            const heading = headings[i]
            const element = document.getElementById(heading.id)
            if (!element) continue

            const rect = element.getBoundingClientRect()
            const topPosition = rect.top + window.scrollY
            const elementHeight = rect.height

            // Calculate how much of the element is visible in viewport
            const viewportTop = window.scrollY + offset
            const viewportBottom = window.scrollY + window.innerHeight
            const elementTop = topPosition
            const elementBottom = topPosition + elementHeight

            // Check if element is in view
            if (elementBottom > viewportTop && elementTop < viewportBottom) {
                // Calculate visibility percentage
                const visibleTop = Math.max(viewportTop, elementTop)
                const visibleBottom = Math.min(viewportBottom, elementBottom)
                const visibleHeight = Math.max(0, visibleBottom - visibleTop)
                const visibilityPercentage = visibleHeight / Math.max(elementHeight, 50) // Minimum 50px for small sections

                // Calculate position score (closer to top of viewport = higher score)
                const distanceFromTop = Math.abs(elementTop - viewportTop)
                const positionScore = Math.max(0, 1000 - distanceFromTop)

                // Combined score: visibility + position + bonus for being the current section
                let score = visibilityPercentage * 100 + positionScore * 0.1

                // Bonus for current section to prevent jumping
                if (heading.id === currentHeadingId.value) {
                    score += 25
                }

                // For very small sections, give them a fair chance if they're prominently visible
                if (elementHeight < 100 && visibilityPercentage > 0.3) {
                    score += 30
                }

                if (!bestMatch || score > bestMatch.score) {
                    bestMatch = { id: heading.id, score }
                }
            }

            // Fallback to traditional method for sections that have passed the offset
            if (window.scrollY >= topPosition - offset) {
                if (!bestMatch || bestMatch.score < 50) {
                    bestMatch = { id: heading.id, score: 50 }
                }
            }
        }

        if (bestMatch) {
            headingId = bestMatch.id
        }
    }

    traverseIds(startIndex, headingId)
    return
}

const sleep = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms))
}

const traverseIds = async (startIndex: number, endId: string | null) => {
    const token = ++cancelToken.value
    const headings = getVisibleSections.value
    const initialEndIndex = headings.findIndex(h => h.id === endId)
    if (startIndex === initialEndIndex) return
    if (startIndex === -1) startIndex = 0
    let endIndex = initialEndIndex
    if (endIndex === -1 || endIndex == null) endIndex = 0

    if (startIndex <= endIndex) {
        for (let i = startIndex; i <= endIndex; i++) {
            if (token !== cancelToken.value) return
            currentHeadingId.value = headings[i].id
            await sleep(50)
        }
    } else {
        for (let i = startIndex; i >= endIndex; i--) {
            if (token !== cancelToken.value) return
            currentHeadingId.value = headings[i].id
            await sleep(50)
        }
    }
    if (initialEndIndex === -1 || initialEndIndex == null) currentHeadingId.value = null
}

const throttledGetCurrentHeadingId = throttle(getCurrentHeadingId, 50)

onMounted(() => {
    window.addEventListener('scroll', throttledGetCurrentHeadingId)
    nextTick(() => {
        getCurrentHeadingId()
    })
})

onBeforeUnmount(() => {
    window.removeEventListener('scroll', throttledGetCurrentHeadingId)
})

const headingClass = (index: number) => {
    return `level-1${index === 0 ? ' first-outline' : ''}`
}
</script>

<template>
    <div :id="containerId" class="sidebar-outline">
        <PerfectScrollbar :options="{ suppressScrollX: true }">
            <div class="outline-container">
                <div v-for="(heading, index) in getVisibleSections" :key="heading.id" class="outline-heading"
                    :class="headingClass(index)">
                    <NuxtLink :to="`#${heading.id}`"
                        class="outline-link" :class="{ 'current-heading': heading.id === currentHeadingId }">
                        {{ t(heading.translationKey) }}
                    </NuxtLink>
                </div>
            </div>
        </PerfectScrollbar>
    </div>
</template>

<style lang="less" scoped>
:global(html) {
    scroll-behavior: smooth;
    scroll-padding-top: v-bind('offset + "px"');
    /* Offset for fixed headers */
}
</style>

<style lang="less">
@import '~~/assets/sidebar.less';
</style>
