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

onMounted(() => {
    window.addEventListener('scroll', throttledGetCurrentHeadingId)
})
onBeforeUnmount(() => {
    window.removeEventListener('scroll', throttledGetCurrentHeadingId)
})
</script>

<template>
    <div id="Outline">
        <div v-for="heading in outlineStore.headings" :key="heading.id" class="outline-headings"
            :class="`level-${heading.level - 1}`">
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
    .outline-headings {

        &.level-1 {}

        &.level-2 {
            padding-left: 8px;
        }

        &.level-3 {
            padding-left: 16px;
        }

        &.level-1,
        &.level-2,
        &.level-3 {
            margin-bottom: 4px;
        }

        .outline-link {
            color: @memo-grey-dark;
            display: block;

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
                font-weight: 600;
                color: @memo-blue;
            }

        }
    }
}
</style>