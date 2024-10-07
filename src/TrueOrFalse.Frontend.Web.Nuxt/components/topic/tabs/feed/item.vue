<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { TopicFeedItem, TopicChangeType, QuestionChangeType, QuestionFeedItem, getTime, Author } from './feedHelper'
import { color } from '~/components/shared/colors'

interface Props {
    topicFeedItem?: TopicFeedItem
    questionFeedItem?: QuestionFeedItem
}
const props = defineProps<Props>()

enum FeedType {
    Topic,
    Question
}

interface Change {
    label: string
    color: string
    id: number
}

interface FeedItem {
    date: string
    changeType: Change
    feedType: FeedType
    id: number
    visibility: Visibility
    author: Author
    label: string
}

const feedItem = ref<FeedItem>()
const date = ref<string>()

const setFeedItem = (item: TopicFeedItem | QuestionFeedItem) => {
    // if item is TopicFeedItem
    if ('topicId' in item) {
        feedItem.value = {
            date: item.date,
            changeType: { label: TopicChangeType[item.type], color: getTopicChangeColor(item.type), id: item.categoryChangeId },
            feedType: FeedType.Topic,
            id: item.topicId,
            visibility: item.visibility,
            label: item.title
        } as FeedItem
    }
    else if ('questionId' in item) {
        feedItem.value = {
            date: item.date,
            changeType: { label: QuestionChangeType[item.type], color: getQuestionChangeColor(item.type), id: item.questionChangeId },
            feedType: FeedType.Question,
            id: item.questionId,
            visibility: item.visibility,
            label: item.text
        } as FeedItem
    }
}

const getTopicChangeColor = (changeType: TopicChangeType) => {
    switch (changeType) {
        case TopicChangeType.Moved:
        case TopicChangeType.Relations:
        case TopicChangeType.Create:
            return color.memoInfo
        case TopicChangeType.Text:
        case TopicChangeType.Renamed:
        case TopicChangeType.Image:
            return color.memoGreen
        case TopicChangeType.Delete:
            return color.memoWuwiRed
        default:
            return color.memoGreyLight
    }
}

const getQuestionChangeColor = (changeType: QuestionChangeType) => {
    switch (changeType) {
        case QuestionChangeType.Create:
            return color.memoInfo
        case QuestionChangeType.Update:
            return color.memoGreen
        case QuestionChangeType.Delete:
            return color.memoWuwiRed
        default:
            return color.memoGreyLight
    }
}

onBeforeMount(() => {
    if (props.topicFeedItem)
        setFeedItem(props.topicFeedItem)
    else if (props.questionFeedItem)
        setFeedItem(props.questionFeedItem)

    if (feedItem.value)
        date.value = getTime(feedItem.value.date)
})

watch(() => props.topicFeedItem, (newValue) => {
    if (newValue)
        setFeedItem(newValue)
}, { deep: true })

const emit = defineEmits(['openFeedModal'])

</script>

<template>
    <div class="feed-item" v-if="feedItem" @click="emit('openFeedModal', { type: feedItem.feedType, id: feedItem.id })">
        <div class="feed-item-date">
            {{ date }}
        </div>
        <div class="feed-item-label">
            <div class="feed-item-feed-type-label" v-if="feedItem.feedType === FeedType.Question">
                <font-awesome-icon :icon="['fas', 'circle-question']" />
            </div>
            <div class="feed-item-label-text">
                {{ feedItem.label }}
            </div>
        </div>
        <div class="feed-item-change-type" :style="`background: ${feedItem.changeType.color}`">
            {{ feedItem.changeType.label }}
        </div>
        <div class="feed-item-visibility">
            <font-awesome-icon :icon="['fas', 'lock']"
                v-if="feedItem.visibility === Visibility.Owner" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-item {
    padding: 8px 16px;
    border: 1px solid @memo-grey-lighter;
    display: flex;
    flex-direction: row;
    flex-wrap: nowrap;
    justify-content: space-between;
    align-items: center;
    min-height: 32px;
    border-radius: 4px;
    font-size: 1em;
    margin-bottom: 8px;
    cursor: pointer;
    background: white;
    &:hover{
        filter: brightness(0.95);
    }

    &:active {
        filter: brightness(0.9);
    }

    .feed-item-date {
        min-width: 50px;
        display: flex;
        justify-content: center;
        align-items: center;
        margin: 0 8px;
        margin-left: 0px;
        color: @memo-grey-dark;
    }

    .feed-item-feed-type-label {
        background: @memo-grey-light;
        padding: 0px;
        border-radius: 44px;
        color: white;
        width: 32px;
        text-align: center;
        min-width: 32px;
        height: 32px;
        display:flex;
        justify-content: center;
        align-items: center;
        margin-right: 8px;
        font-size: 2em;
    }

    .feed-item-label {
        flex-grow: 1;
        padding: 4px 8px;
        margin: 0 15px;
        max-width: 260px;
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        flex-basis: 0;
        text-overflow: ellipsis;
        overflow: hidden;
        white-space: nowrap;

        .feed-item-label-text {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
    }

    .feed-item-change-type {
        background: @memo-grey-lighter;
        padding: 4px 16px;
        border-radius: 4px;
        color: @memo-grey-darker;
        min-width: 160px;
        text-align: center;
        margin: 0 16px;
    }

    .feed-item-visibility {
        min-width: 52px;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 0 16px;
        margin-right: 0px;

    }
}
</style>