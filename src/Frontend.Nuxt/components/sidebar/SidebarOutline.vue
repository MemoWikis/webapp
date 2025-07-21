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
}

const props = withDefaults(defineProps<Props>(), {
    containerId: 'Outline',
    visibleSections: () => []
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
    const offset = 180
    let headingId: string | null = headings[0].id

    const startIndex = headings.findIndex(h => h.id === currentHeadingId.value)

    // Check if user has scrolled to the bottom of the page
    const isScrolledToBottom = window.innerHeight + window.scrollY >= document.body.offsetHeight - 40

    if (isScrolledToBottom) {
        // Use the last heading when scrolled to bottom
        headingId = headings[headings.length - 1].id
    } else {
        for (const heading of headings) {
            const element = document.getElementById(heading.id)
            if (!element) continue

            const rect = element.getBoundingClientRect()
            const topPosition = rect.top + window.scrollY

            if (window.scrollY >= topPosition - offset) {
                headingId = heading.id
            } else {
                break
            }
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
    <div :id="containerId">
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
@import '~~/assets/sidebar.less';
</style>
