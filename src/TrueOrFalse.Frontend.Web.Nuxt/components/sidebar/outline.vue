<script lang="ts" setup>
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { usePageStore } from '../page/pageStore'
import { throttle } from 'underscore'

const outlineStore = useOutlineStore()
const pageStore = usePageStore()

const { $urlHelper } = useNuxtApp()

const currentHeadingId = ref<string | null>()
const previousIndex = ref()


function getCurrentHeadingId() {
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

function sleep(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
}
const cancelToken = ref<number>(0)
async function traverseIds(startIndex: number, endId: string | null) {
    const token = ++cancelToken.value
    const headings = outlineStore.headings
    const initialEndIndex = headings.findIndex(h => h.id === endId)
    if (startIndex == initialEndIndex) return
    if (startIndex === -1) startIndex = 0
    let endIndex = initialEndIndex
    if (endIndex === -1 || endIndex === null) endIndex = 0

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
    if (initialEndIndex === -1 || initialEndIndex === null) currentHeadingId.value = null
}

function findCurrentSectionId(): string | null {
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

function headingClass(level: number, index: number) {
    const previousLevel = index > 0 ? outlineStore.headings[index - 1].level : null
    if (previousLevel != null) {
        if (previousLevel > level)
            return `level-${level - 1} next-step`
        if (previousLevel === 2 && level === 2)
            return `level-${level - 1} preceeding-section-is-empty`
    }

    return `level-${level - 1}${index == 0 ? ' first-outline' : ''}`
}

</script>

<template>
    <div id="Outline">
        <perfect-scrollbar :suppressScrollX="true">
            <div class="outline-container">
                <div v-for="(heading, index) in outlineStore.headings" :key="heading.id" class="outline-heading"
                    :class="headingClass(heading.level, index)">
                    <NuxtLink :to="`${$urlHelper.getPageUrl(pageStore.name, pageStore.id)}#${heading.id}`"
                        class="outline-link" :class="{ 'current-heading': heading.id === currentHeadingId }">
                        <div v-for="text in heading.text">
                            {{ text }}
                        </div>
                    </NuxtLink>
                </div>
            </div>
        </perfect-scrollbar>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#Outline {
    height: 100%;

    .outline-container {
        height: 100%;
    }

    .outline-heading {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        transition: all 0.01s ease;

        &.level-2,
        &.level-3 {
            margin-top: 4px;
            margin-bottom: 4px;
        }

        &.next-step {
            margin-top: 8px;
        }

        &.level-1 {
            font-size: 16px;
            font-weight: 600;
            margin-top: 24px;

            .current-heading {
                font-weight: 700;
            }

            &.preceeding-section-is-empty {
                margin-top: 8px;
            }
        }

        &.level-2 {
            font-weight: 400;

            .current-heading {
                font-weight: 600;
            }
        }

        &.level-3 {
            font-weight: 300;

            .current-heading {
                font-weight: 600;
            }
        }

        &.first-outline {
            margin-top: 0px;
        }

        .outline-link {
            color: @memo-grey-dark;
            display: block;

            transition: all 0.1s ease-out;

            &:hover {
                color: @memo-blue-link;
            }

            &:visited,
            &:focus,
            &:active,
            &:hover {
                text-decoration: none;
            }

            &.current-heading {
                color: @memo-blue;
            }
        }
    }
}
</style>