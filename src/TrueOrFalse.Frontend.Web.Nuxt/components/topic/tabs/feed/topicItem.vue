<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { TopicFeedItem, TopicChangeType, getTime } from './feedHelper'

interface Props {
    feedItem: TopicFeedItem
}
const props = defineProps<Props>()

const topicFeedItem = ref<TopicFeedItem>(props.feedItem!)
const date = ref<string>()

onBeforeMount(() => {
    date.value = getTime(topicFeedItem.value.date)
})

const emit = defineEmits(['openFeedModal'])

</script>

<template>
    <div class="feed-item">
        <div><font-awesome-icon :icon="['fas', 'file-lines']" /> {{ date }} </div>
        <div>{{ TopicChangeType[topicFeedItem.type] }} - {{ topicFeedItem.topicId }}</div>
        <div>
            <font-awesome-icon :icon="['fas', 'lock']"
                v-if="topicFeedItem.visibility === Visibility.Owner" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-item {
    padding: 8px;
    border: 1px solid @memo-grey-lighter;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    justify-content: space-between;
    align-items: center;
    min-height: 32px;
}
</style>