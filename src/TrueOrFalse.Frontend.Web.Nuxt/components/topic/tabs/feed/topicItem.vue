<script lang="ts" setup>
import { TopicFeedItem, TopicChangeType } from './feedHelper'

interface Props {
    feedItem: TopicFeedItem
}
const props = defineProps<Props>()

const topicFeedItem = ref<TopicFeedItem>(props.feedItem!)

</script>

<template>

    <div>
        Thema: {{ topicFeedItem.date }} - {{ TopicChangeType[topicFeedItem.type] }} - {{ topicFeedItem.categoryChangeId }} - {{ topicFeedItem.topicId }} - {{ topicFeedItem.visibility }}
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-item {

    margin: 8px;
    border: solid 1px @memo-grey-lighter;
    padding: 8px;

}

</style>