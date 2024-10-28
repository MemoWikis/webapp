<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { TopicFeedItem, TopicChangeType, QuestionChangeType, QuestionFeedItem, getTime, Author, getTopicChangeTypeName, getQuestionChangeTypeName } from './feedHelper'
import { color } from '~/components/shared/colors'
import { messages } from '~/components/alert/messages'
import { useTopicStore } from '../../topicStore'

interface Props {
    index?: number
    topicFeedItem?: TopicFeedItem
    questionFeedItem?: QuestionFeedItem
}
const props = defineProps<Props>()
const topicStore = useTopicStore()

enum FeedType {
    Topic,
    Question
}

interface Params {
    label: string
    color: string
    id: number
    type: TopicChangeType | QuestionChangeType
}

interface FeedItem {
    date: string
    params: Params
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
    if ('questionId' in item) {
        const questionItem = item as QuestionFeedItem
        feedItem.value = {
            date: questionItem.date,
            params: { label: getQuestionChangeTypeName(questionItem.type), color: getQuestionChangeColor(questionItem.type), id: questionItem.questionChangeId, type: questionItem.type },
            feedType: FeedType.Question,
            id: questionItem.questionId,
            visibility: questionItem.visibility,
            label: questionItem.text
        } as FeedItem

        if (item.type === QuestionChangeType.AddComment) {
            canOpen.value = true
        }

    }
    else if ('topicId' in item) {
        const topicItem = item as TopicFeedItem
        feedItem.value = {
            date: topicItem.date,
            params: { label: getTopicChangeTypeName(topicItem.type), color: getTopicChangeColor(topicItem.type), id: topicItem.categoryChangeId, type: topicItem.type },
            feedType: FeedType.Topic,
            id: topicItem.topicId,
            visibility: topicItem.visibility,
            label: topicItem.title
        } as FeedItem

        if (item.type === TopicChangeType.Renamed && item.nameChange) {
            oldName.value = item.nameChange.oldName
            newName.value = item.nameChange.newName
        }

        if (item.type === TopicChangeType.ChildTopicDeleted && topicItem.deleteData) {
            feedItem.value.label = topicItem.deleteData.deletedName
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
            default:
                canOpen.value = false
        }
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
        case TopicChangeType.ChildTopicDeleted:
        case TopicChangeType.QuestionDeleted:
            return color.memoSalmon
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

watch(() => props.topicFeedItem, (newTopicItem) => {
    if (newTopicItem)
        setFeedItem(newTopicItem)
}, { deep: true })

watch(() => props.questionFeedItem, (newQuestionItem) => {
    if (newQuestionItem)
        setFeedItem(newQuestionItem)
}, { deep: true })

const emit = defineEmits(['openFeedModal'])

const handleClick = () => {
    if (feedItem.value && canOpen.value)
        emit('openFeedModal', { type: feedItem.value.feedType, id: feedItem.value.id, index: props.index })
}

const { isDesktop } = useDevice()

</script>

<template>
    <div class="feed-item" v-if="feedItem" @click="handleClick" :class="{ 'no-modal': !canOpen, 'mobile': !isDesktop }">
        <div class="feed-item-info">
            <div class="feed-item-change-type" :style="`background: ${feedItem.params.color}`">
                {{ feedItem.params.label }}
            </div>
            <div class="feed-item-date">
                {{ date }}
            </div>

            <div v-if="!isDesktop" class="feed-item-info-visibility" @click.stop>
                <font-awesome-icon :icon="['fas', 'lock']" v-if="feedItem.visibility === Visibility.Owner" v-tooltip="messages.info.feed.private" class="feed-item-visibility-icon" />
            </div>
        </div>
        <div class="feed-item-label">
            <div class="feed-item-feed-type-icon" v-if="feedItem.feedType === FeedType.Question">
                <font-awesome-icon :icon="['fas', 'circle-question']" />
            </div>
            <div class="feed-item-label-body">
                <div class="feed-item-label-text">
                    <template v-if="feedItem.feedType === FeedType.Topic">
                        <span v-if="feedItem.params.type === TopicChangeType.ChildTopicDeleted || feedItem.params.type === TopicChangeType.QuestionDeleted">
                            {{ feedItem.label }}
                        </span>
                        <NuxtLink v-else :to="$urlHelper.getTopicUrl(feedItem.label, feedItem.id)" @click.stop>
                            {{ feedItem.label }}
                        </NuxtLink>
                    </template>

                    <NuxtLink v-else-if="feedItem.feedType === FeedType.Question" :to="$urlHelper.getTopicUrlWithQuestionId(topicStore.name, topicStore.id, feedItem.id)" @click.stop>
                        {{ feedItem.label }}
                    </NuxtLink>
                </div>

                <template v-if="feedItem.feedType === FeedType.Topic">
                    <div class="feed-item-label-renamed" v-if="feedItem.params.type === TopicChangeType.Renamed">
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

                    <TopicTabsFeedRelations v-else-if="feedItem.params.type === TopicChangeType.Relations && props.topicFeedItem?.relationChanges" :relation-changes="props.topicFeedItem.relationChanges" />

                    <div class="feed-item-label-deleted" v-else-if="feedItem.params.type === TopicChangeType.ChildTopicDeleted">
                        Unterthema gelöscht
                    </div>
                </template>

                <template v-if="feedItem.feedType === FeedType.Question && feedItem.params.type === QuestionChangeType.AddComment && props.questionFeedItem">
                    <div class="feed-item-label-commentadd" v-html="props.questionFeedItem.comment?.title"> </div>
                </template>
            </div>
        </div>

        <div class="feed-item-visibility" v-if="isDesktop">
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

    &:hover {
        filter: brightness(0.95);
    }

    &:active {
        filter: brightness(0.9);
    }

    &.no-modal {
        cursor: default;

        &:hover,
        &:active {
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
        display: flex;
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
                overflow: hidden;
                text-overflow: ellipsis;

                color: @memo-grey-dark;
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

            .feed-item-label-deleted {
                color: @memo-grey-dark;
                white-space: nowrap;
                text-overflow: ellipsis;
            }

            .feed-item-label-commentadd {
                color: @memo-grey-dark;
                white-space: nowrap;
                text-overflow: ellipsis;
            }
        }
    }

    .feed-item-change-type {
        background: @memo-grey-lighter;
        padding: 4px 16px;
        border-radius: 4px;
        color: @memo-grey-darker;
        min-width: 140px;
        text-align: center;
        margin: 0 16px;
        font-weight: 600;
    }

    .feed-item-visibility {
        display: flex;
        justify-content: center;
        color: @memo-grey-dark;
        font-size: 18px;
        position: relative;
        right: 0;
        top: 0;
        opacity: 0.5;

        svg.feed-item-visibility-icon {
            cursor: help;
            position: absolute;
        }
    }

    &.mobile {
        flex-direction: column;

        .feed-item-info {
            flex-direction: row;

            .feed-item-change-type {
                margin-left: 0;
            }
        }

        .feed-item-info-visibility {
            height: 28px;
            display: flex;
            justify-content: center;
            align-items: center;
            color: @memo-grey-dark;
            opacity: 0.5;
        }

        .feed-item-label {
            margin-top: 16px;
            margin-left: 0;
            padding-left: 0;
            overflow: visible;

            .feed-item-label-text {
                max-height: 50px;
                text-wrap: wrap;
            }
        }
    }
}
</style>