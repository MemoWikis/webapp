<script lang="ts" setup>
import { QuestionFeedItem, TopicChangeType } from './feedHelper'

interface Props {
    feedItem: QuestionFeedItem
}
const props = defineProps<Props>()

const questionFeedItem = ref<QuestionFeedItem>(props.feedItem!)

</script>

<template>

    <div>
        Frage: {{ questionFeedItem.date }} - {{ TopicChangeType[questionFeedItem.type] }} - {{ questionFeedItem.questionChangeId }} - {{ questionFeedItem.questionId }} - {{ questionFeedItem.visibility }}
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