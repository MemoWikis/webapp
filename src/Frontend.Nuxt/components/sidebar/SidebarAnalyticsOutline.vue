<script lang="ts" setup>
import { PAGE_ANALYTICS_SECTIONS } from '~/constants/pageAnalyticsSections'
import { throttle } from 'underscore'

const { t } = useI18n()
const currentHeadingId = ref<string | null>(null)
const cancelToken = ref<number>(0)

const getCurrentHeadingId = () => {
    if (PAGE_ANALYTICS_SECTIONS.length === 0) return

    const headings = PAGE_ANALYTICS_SECTIONS
    const offset = 120
    let headingId: string | null = null

    const startIndex = headings.findIndex(h => h.id === currentHeadingId.value)

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

const traverseIds = async (startIndex: number, endId: string | null) => {
    const token = ++cancelToken.value
    const headings = PAGE_ANALYTICS_SECTIONS
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
    <div id="AnalyticsOutline">
        <perfect-scrollbar :suppressScrollX="true">
            <div class="outline-container">
                <div v-for="(heading, index) in PAGE_ANALYTICS_SECTIONS" :key="heading.id" class="outline-heading"
                    :class="headingClass(index)">
                    <NuxtLink :to="`#${heading.id}`"
                        class="outline-link" :class="{ 'current-heading': heading.id === currentHeadingId }">
                        {{ t(heading.translationKey) }}
                    </NuxtLink>
                </div>
            </div>
        </perfect-scrollbar>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AnalyticsOutline {
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
            margin-top: 8px;

            .current-heading {
                font-weight: 700;
            }

            &.preceeding-section-is-empty {
                margin-top: 8px;
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