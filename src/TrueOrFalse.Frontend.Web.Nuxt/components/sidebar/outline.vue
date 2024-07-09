<script lang="ts" setup>
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { useTopicStore } from '../topic/topicStore'
import { throttle } from 'underscore'

const outlineStore = useOutlineStore()
const topicStore = useTopicStore()

const { $urlHelper } = useNuxtApp()

const currentHeadingId = ref('')
const previousIndex = ref()
function getCurrentHeadingId() {
    if (outlineStore.headings.length === 0) return

    const headings = outlineStore.headings
    const offset = 120
    let headingId: string | null = null

    if (outlineStore.editorIsFocused) {
        if (previousIndex.value === outlineStore.nodeIndex) return
        previousIndex.value = outlineStore.nodeIndex

        const currentSectionId = findCurrentSectionId()
        if (currentSectionId !== null) {
            currentHeadingId.value = currentSectionId
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

    if (headingId === null) headingId = headings[0].id

    currentHeadingId.value = headingId
    return
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

const throttledGetCurrentHeadingId = throttle(getCurrentHeadingId, 100)

onMounted(async () => {
    window.addEventListener('scroll', throttledGetCurrentHeadingId)
    await nextTick()
    getCurrentHeadingId()

    watch(() => outlineStore.editorIsFocused, () => {
        throttledGetCurrentHeadingId()
    })

    outlineStore.$onAction(({ name, after }) => {
        if (name == 'updateHeadings') {
            after(() => {
                throttledGetCurrentHeadingId()
            })
        }
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
        <div v-for="(heading, index) in outlineStore.headings" :key="heading.id" class="outline-heading"
            :class="headingClass(heading.level, index)">
            <font-awesome-icon class="current-heading-icon" :icon="['fas', 'pen']"
                v-show="outlineStore.editorIsFocused && heading.id === currentHeadingId" />
            <NuxtLink :to="`${$urlHelper.getTopicUrl(topicStore.name, topicStore.id)}#${heading.id}`"
                class="outline-link" :class="{ 'current-heading': heading.id === currentHeadingId }">
                {{ heading.text }}
            </NuxtLink>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#Outline {
    .outline-heading {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        transition: all 0.01s ease;

        .current-heading-icon {
            font-size: 1rem;
            margin-right: 8px;
            transition: all 0.1 ease-in;
            color: @memo-blue;
        }

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