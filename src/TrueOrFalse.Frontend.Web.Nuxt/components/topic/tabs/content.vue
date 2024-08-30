<script setup lang="ts">
import { Visibility } from '~/components/shared/visibilityEnum'
import { useTopicStore } from '../topicStore'

const { isMobile } = useDevice()
const topicStore = useTopicStore()

const props = defineProps<{
    textIsHidden: boolean
}>()
</script>

<template>
    <div id="TopicContent" class="row" :class="{ 'is-mobile': isMobile, 'hideText': props.textIsHidden }">
        <template v-if="!props.textIsHidden">
            <TopicContentEditor />
            <TopicContentEditBar v-if="topicStore.visibility == Visibility.All" />
        </template>
    </div>
</template>

<style lang="less" scoped>
#TopicContent {
    &.hideText {
        position: absolute;
        width: 100%;
        padding-top: 12px;
    }
}
</style>