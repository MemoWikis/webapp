<script lang="ts" setup>
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { useTopicStore } from '../topic/topicStore'
import { throttle } from 'underscore'

const outlineStore = useOutlineStore()
const topicStore = useTopicStore()

const { $urlHelper } = useNuxtApp()

const currentHeadingId = ref('')

function getCurrentHeadingId() {
    const headings = outlineStore.headings
    const offset = 120
    let headingId: string | null = null

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

const throttledGetCurrentHeadingId = throttle(getCurrentHeadingId, 100)

onMounted(async () => {
    window.addEventListener('scroll', throttledGetCurrentHeadingId)
    await nextTick()
    getCurrentHeadingId()
})
onBeforeUnmount(() => {
    window.removeEventListener('scroll', throttledGetCurrentHeadingId)
})

function headingClass(level: number, index: number) {
    const previousLevel = index > 0 ? outlineStore.headings[index - 1].level : null
    if (previousLevel != null && previousLevel > level)
        return `level-${level - 1} next-step`

    return `level-${level - 1}`
}
</script>

<template>
    <div id="Outline">
        <div v-for="(heading, index) in outlineStore.headings" :key="heading.id" class="outline-heading"
            :class="headingClass(heading.level, index)">
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
        transition: all 0.1s ease;

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

        .outline-link {
            color: @memo-grey-dark;
            display: block;

            transition: all 0.2s ease-out;

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