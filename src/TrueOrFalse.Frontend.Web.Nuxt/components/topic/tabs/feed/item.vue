<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { TopicFeedItem, TopicChangeType, QuestionChangeType, QuestionFeedItem, getTime, Author, getTopicChangeTypeName } from './feedHelper'
import { color } from '~/components/shared/colors'
import { messages } from '~/components/alert/messages'

interface Props {
    index?: number
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
    type: TopicChangeType
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

const oldName = ref<string>()
const newName = ref<string>()

const canOpen = ref<boolean>(false)

const setFeedItem = (item: TopicFeedItem | QuestionFeedItem) => {
    canOpen.value = false

    if ('topicId' in item) {
        feedItem.value = {
            date: item.date,
            changeType: { label: getTopicChangeTypeName(item.type), color: getTopicChangeColor(item.type), id: item.categoryChangeId, type: item.type },
            feedType: FeedType.Topic,
            id: item.topicId,
            visibility: item.visibility,
            label: item.title
        } as FeedItem

        if (item.type === TopicChangeType.Renamed && item.nameChange) {
            oldName.value = item.nameChange.oldName
            newName.value = item.nameChange.newName
        }

        const hasRelations = item.relationChanges &&
            (item.relationChanges.addedParents.length > 0 ||
                item.relationChanges.addedChildren.length > 0 ||
                item.relationChanges.removedParents.length > 0 ||
                item.relationChanges.removedChildren.length > 0)

        switch (item.type) {
            case TopicChangeType.Text:
            case TopicChangeType.Renamed:
                canOpen.value = true
                break
            case TopicChangeType.Relations:
                canOpen.value = !!hasRelations
                break
        }

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
})

watch(feedItem, (newValue) => {
    if (newValue)
        date.value = getTime(newValue.date)
}, { deep: true })

watch(() => props.topicFeedItem, (newValue) => {
    if (newValue)
        setFeedItem(newValue)
}, { deep: true })

const emit = defineEmits(['openFeedModal'])

const handleClick = () => {
    if (feedItem.value && canOpen.value)
        emit('openFeedModal', { type: feedItem.value.feedType, id: feedItem.value.id, index: props.index })
}

</script>

<template>
    <div class="feed-item" v-if="feedItem" @click="handleClick" :class="{ 'no-modal': !canOpen }">
        <div class="feed-item-info">
            <div class="feed-item-change-type" :style="`background: ${feedItem.changeType.color}`">
                {{ feedItem.changeType.label }}
            </div>
            <div class="feed-item-date">
                {{ date }}
            </div>
        </div>
        <div class="feed-item-label">
            <div class="feed-item-feed-type-icon" v-if="feedItem.feedType === FeedType.Question">
                <font-awesome-icon :icon="['fas', 'circle-question']" />
            </div>
            <div class="feed-item-label-body">
                <div class="feed-item-label-text">
                    <NuxtLink :to="$urlHelper.getTopicUrl(feedItem.label, feedItem.id)" @click.stop>
                        {{ feedItem.label }}
                    </NuxtLink>
                </div>

                <template v-if="feedItem.feedType === FeedType.Topic">
                    <div class="feed-item-label-renamed" v-if="feedItem.changeType.type === TopicChangeType.Renamed">
                        <div class="feed-item-label-renamed-text">
                            {{ oldName }}
                        </div>
                        <div class="feed-item-label-renamed-icon">
                            <font-awesome-icon :icon="['fas', 'chevron-right']" />
                        </div>
                        <div class="feed-item-label-renamed-text">
                            {{ newName }}
                        </div>
                    </div>

                    <TopicTabsFeedRelations v-else-if="feedItem.changeType.type === TopicChangeType.Relations && props.topicFeedItem?.relationChanges" :relation-changes="props.topicFeedItem.relationChanges" />

                </template>


            </div>
        </div>

        <div class="feed-item-visibility">
            <font-awesome-icon :icon="['fas', 'lock']" v-if="feedItem.visibility === Visibility.Owner" v-tooltip="messages.info.feed.private" class="feed-item-visibility-icon" />
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

    &.no-modal {
        cursor: default;
        &:hover, &:active {
            filter: brightness(1);
        }
    }

    .feed-item-info {
        display: flex;
        height: 100%;
        flex-direction: column;
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

    .feed-item-feed-type-icon {
        background: @memo-grey-light;
        padding: 0px;
        border-radius: 25px;
        color: white;
        width: 25px;
        text-align: center;
        min-width: 25px;
        height: 25px;
        display:flex;
        justify-content: center;
        align-items: center;
        margin-right: 8px;
        font-size: 1.5em;
        margin-bottom: 8px;
    }

    .feed-item-label {
        flex-grow: 1;
        padding: 0 8px;
        margin: 0 15px;
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        flex-basis: 0;
        text-overflow: ellipsis;
        overflow: hidden;
        white-space: nowrap;
        height: 100%;

        .feed-item-label-body {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;

            .feed-item-label-text {
                font-size: 1.2em;
                font-weight: 600;
                margin-bottom: 8px;
            }

            .feed-item-label-renamed {
                color: @memo-grey-dark;
                white-space: nowrap;
                text-overflow: ellipsis;

                .feed-item-label-renamed-icon {
                    display: inline;
                    padding: 0 8px;
                    width: 25px;
                }

                .feed-item-label-renamed-text {
                    display: inline;
                    max-width: calc(50% - 13px);
                    overflow: hidden;
                    text-overflow: ellipsis;
                    white-space: nowrap;
                }
            }
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
        font-weight: 600;
    }

    .feed-item-visibility {
        min-width: 30px;
        display: flex;
        justify-content: center;
        padding: 0 0 0 16px;
        margin-right: -8px;
        color: @memo-grey-light;
        font-size: 18px;

        svg.feed-item-visibility-icon {
            cursor: help;
        }
    }
}
</style>