<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { getTime, QuestionFeedItem, TopicChangeType } from './feedHelper'

interface Props {
    feedItem: QuestionFeedItem
}
const props = defineProps<Props>()

const questionFeedItem = ref<QuestionFeedItem>(props.feedItem!)
const date = ref<string>()

onBeforeMount(() => {
    date.value = getTime(questionFeedItem.value.date)
})

const emit = defineEmits(['openFeedModal'])

</script>

<template>
    <div class="feed-item">
        <div><font-awesome-icon :icon="['fas', 'file-lines']" /> {{ date }} </div>
        <div>{{ TopicChangeType[questionFeedItem.type] }} - {{ questionFeedItem.questionId }}</div>
        <div>
            <font-awesome-icon :icon="['fas', 'lock']"
                v-if="questionFeedItem.visibility === Visibility.Owner" />
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