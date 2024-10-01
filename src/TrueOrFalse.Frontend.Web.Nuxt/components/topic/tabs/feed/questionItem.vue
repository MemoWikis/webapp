<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { getTime, QuestionFeedItem, TopicChangeType } from './feedHelper'

interface Props {
    feedItem: QuestionFeedItem
}
const props = defineProps<Props>()

const questionFeedItem = ref<QuestionFeedItem>(props.feedItem!)
const emit = defineEmits(['openFeedModal'])
const date = ref<string>()

onBeforeMount(() => {
    date.value = getTime(questionFeedItem.value.date)
})
</script>

<template>

    <div>
        <font-awesome-icon :icon="['fas', 'circle-question']" />
        {{ date }} - {{ TopicChangeType[questionFeedItem.type] }} - {{ questionFeedItem.questionId }} - {{ questionFeedItem.author.name }}
        <font-awesome-icon :icon="['fas', 'lock']" v-if="questionFeedItem.visibility === Visibility.Owner" />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

</style>