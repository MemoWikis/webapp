<script lang="ts" setup>
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { throttle } from 'underscore'

const outlineStore = useOutlineStore()

const currentHeadingId = ref<string | null>()
const previousIndex = ref()

const getCurrentHeadingId = () => {
    if (outlineStore.headings.length === 0) return

    const headings = outlineStore.headings
    const offset = 120
    let headingId: string | null = null

    const startIndex = headings.findIndex(h => h.id === currentHeadingId.value)
    if (outlineStore.editorIsFocused) {
        if (previousIndex.value === outlineStore.nodeIndex) return
        previousIndex.value = outlineStore.nodeIndex

        const currentSectionId = findCurrentSectionId()
        if (currentSectionId !== null) {
            traverseIds(startIndex, currentSectionId)
            return
        }
    }

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
    traverseIds(startIndex, headingId)
    return
}

const sleep = (ms: number): Promise<void> => {
    return new Promise(resolve => setTimeout(resolve, ms))
}
const cancelToken = ref<number>(0)
const traverseIds = async (startIndex: number, endId: string | null) => {
    const token = ++cancelToken.value
    const headings = outlineStore.headings
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

const findCurrentSectionId = (): string | null => {
    if (!outlineStore.nodeIndex)
        return null

    const headings = outlineStore.headings

    for (let i = 0; i < headings.length; i++) {
        const num = headings[i].index
        if (outlineStore.nodeIndex === num ||
            outlineStore.nodeIndex > num &&
            (i + 1 >= headings.length || headings[i + 1].index > outlineStore.nodeIndex)) {
            return headings[i].id
        }
    }
    return null
}

const throttledGetCurrentHeadingId = throttle(getCurrentHeadingId, 50)

onMounted(async () => {
    window.addEventListener('scroll', throttledGetCurrentHeadingId)
    await nextTick()
    getCurrentHeadingId()

    watch(() => outlineStore.nodeIndex, () => {
        if (previousIndex.value !== outlineStore.nodeIndex)
            getCurrentHeadingId()
    })

    watch(() => outlineStore.editorIsFocused, () => {
        throttledGetCurrentHeadingId()
    })
})

onBeforeUnmount(() => {
    window.removeEventListener('scroll', throttledGetCurrentHeadingId)
})

const headingClass = (level: number, index: number) => {
    const previousLevel = index > 0 ? outlineStore.headings[index - 1].level : null
    if (previousLevel != null) {
        if (previousLevel > level)
            return `level-${level - 1} next-step`
        if (previousLevel === 2 && level === 2)
            return `level-${level - 1} preceeding-section-is-empty`
    }

    return `level-${level - 1}${index === 0 ? ' first-outline' : ''}`
}

</script>

<template>
    <div id="Outline">
        <PerfectScrollbar :options="{ suppressScrollX: true }">
            <div class="outline-container">
                <div v-for="(heading, index) in outlineStore.headings" :key="heading.id" class="outline-heading"
                    :class="headingClass(heading.level, index)">
                    <NuxtLink :to="`#${heading.id}`"
                        class="outline-link" :class="{ 'current-heading': heading.id === currentHeadingId }">
                        <div v-for="text in heading.text">
                            {{ text }}
                        </div>
                    </NuxtLink>
                </div>
            </div>
        </PerfectScrollbar>

    </div>
</template>

<style lang="less" scoped>
@import '~~/assets/sidebar.less';
</style>