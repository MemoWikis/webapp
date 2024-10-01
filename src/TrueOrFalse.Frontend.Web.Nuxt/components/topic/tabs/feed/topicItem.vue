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
</script>

<template>
    <div>
        <font-awesome-icon :icon="['fas', 'file-lines']" /> {{ date }} - {{ TopicChangeType[topicFeedItem.type] }} - {{ topicFeedItem.topicId }} - {{ topicFeedItem.author.name }}<font-awesome-icon :icon="['fas', 'lock']"
            v-if="topicFeedItem.visibility === Visibility.Owner" />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

</style>