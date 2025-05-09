<script lang="ts" setup>
import { Visibility } from '~/components/shared/visibilityEnum'
import { PageFeedItem, PageChangeType, QuestionChangeType, QuestionFeedItem, getTime, Author, getPageChangeTypeKey, getQuestionChangeTypeKey } from './feedHelper'
import { color } from '~/components/shared/colors'

import { usePageStore } from '../../pageStore'

interface Props {
    index?: number
    pageFeedItem?: PageFeedItem
    questionFeedItem?: QuestionFeedItem
}
const props = defineProps<Props>()
const pageStore = usePageStore()
const { t } = useI18n()

enum FeedType {
    Page,
    Question
}

interface Params {
    label: string
    color: string
    id: number
    type: PageChangeType | QuestionChangeType
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

const setFeedItem = (item: PageFeedItem | QuestionFeedItem) => {
    canOpen.value = false
    if ('questionId' in item) {
        const questionItem = item as QuestionFeedItem
        feedItem.value = {
            date: questionItem.date,
            params: { label: t(getQuestionChangeTypeKey(questionItem.type)), color: getQuestionChangeColor(questionItem.type), id: questionItem.questionChangeId, type: questionItem.type },
            feedType: FeedType.Question,
            id: questionItem.questionId,
            visibility: questionItem.visibility,
            label: questionItem.text
        } as FeedItem

        if (item.type === QuestionChangeType.AddComment) {
            canOpen.value = true
        }

    }
    else if ('pageId' in item) {
        const pageItem = item as PageFeedItem
        feedItem.value = {
            date: pageItem.date,
            params: { label: t(getPageChangeTypeKey(pageItem.type)), color: getPageChangeColor(pageItem.type), id: pageItem.pageChangeId, type: pageItem.type },
            feedType: FeedType.Page,
            id: pageItem.pageId,
            visibility: pageItem.visibility,
            label: pageItem.title
        } as FeedItem

        if (item.type === PageChangeType.Renamed && item.nameChange) {
            oldName.value = item.nameChange.oldName
            newName.value = item.nameChange.newName
        }

        if ((item.type === PageChangeType.ChildPageDeleted || item.type === PageChangeType.QuestionDeleted) && pageItem.deleteData) {
            feedItem.value.label = pageItem.deleteData.deletedName
        }

        const hasRelations = item.relationChanges &&
            (item.relationChanges.addedParents.length > 0 ||
                item.relationChanges.addedChildren.length > 0 ||
                item.relationChanges.removedParents.length > 0 ||
                item.relationChanges.removedChildren.length > 0)

        switch (item.type) {
            case PageChangeType.Text:
            case PageChangeType.Renamed:
                canOpen.value = true
                break
            case PageChangeType.Relations:
                canOpen.value = !!hasRelations
                break
            default:
                canOpen.value = false
        }
    }
}

const getPageChangeColor = (changeType: PageChangeType) => {
    switch (changeType) {
        case PageChangeType.Moved:
        case PageChangeType.Relations:
        case PageChangeType.Create:
            return color.memoInfo
        case PageChangeType.Text:
        case PageChangeType.Renamed:
        case PageChangeType.Image:
            return color.memoGreen
        case PageChangeType.Delete:
        case PageChangeType.ChildPageDeleted:
        case PageChangeType.QuestionDeleted:
            return color.lightRed
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
            return color.lightRed
        default:
            return color.memoGreyLight
    }
}

onBeforeMount(() => {
    if (props.pageFeedItem)
        setFeedItem(props.pageFeedItem)
    else if (props.questionFeedItem)
        setFeedItem(props.questionFeedItem)
})

watch(feedItem, (newValue) => {
    if (newValue)
        date.value = getTime(newValue.date)
}, { deep: true })

watch(() => props.pageFeedItem, (newPageItem) => {
    if (newPageItem)
        setFeedItem(newPageItem)
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
const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="feed-item" v-if="feedItem" @click="handleClick" :class="{ 'no-modal': !canOpen, 'mobile': !isDesktop }">
        <div class="feed-item-info">
            <div class="feed-item-date">
                {{ date }}
            </div>

            <div v-if="!isDesktop" class="feed-item-info-visibility" @click.stop>
                <font-awesome-icon :icon="['fas', 'lock']" v-if="feedItem.visibility === Visibility.Private" v-tooltip="t('page.feed.item.private')" class="feed-item-visibility-icon" />
            </div>
        </div>
        <div class="feed-item-label">
            <div class="feed-item-feed-type-icon" v-if="feedItem.feedType === FeedType.Question || feedItem.params.type === PageChangeType.QuestionDeleted">
                <font-awesome-icon :icon="['fas', 'circle-question']" />
            </div>
            <div class="feed-item-label-body">
                <div class="feed-item-label-text">
                    <template v-if="feedItem.feedType === FeedType.Page">
                        <span v-if="feedItem.params.type === PageChangeType.ChildPageDeleted || feedItem.params.type === PageChangeType.QuestionDeleted">
                            {{ feedItem.label }}
                        </span>
                        <NuxtLink v-else :to="$urlHelper.getPageUrl(feedItem.label, feedItem.id)" @click.stop>
                            {{ feedItem.label }}
                        </NuxtLink>
                    </template>

                    <NuxtLink v-else-if="feedItem.feedType === FeedType.Question" :to="$urlHelper.getPageUrlWithQuestionId(pageStore.name, pageStore.id, feedItem.id)" @click.stop>
                        {{ feedItem.label }}
                    </NuxtLink>
                </div>

                <template v-if="feedItem.feedType === FeedType.Page">
                    <div class="feed-item-label-renamed" v-if="feedItem.params.type === PageChangeType.Renamed">
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

                    <PageTabsFeedRelations v-else-if="feedItem.params.type === PageChangeType.Relations && props.pageFeedItem?.relationChanges"
                        :relation-changes="props.pageFeedItem.relationChanges" />
                    <PageTabsFeedCreate v-else-if="feedItem.params.type === PageChangeType.Create && props.pageFeedItem?.relationChanges && props.pageFeedItem.relationChanges.addedParents.length > 0"
                        :added-parent="props.pageFeedItem.relationChanges.addedParents[0]" />

                    <div class="feed-item-label-deleted" v-else-if="feedItem.params.type === PageChangeType.ChildPageDeleted">
                        {{ t('page.feed.item.childPageDeleted') }}
                    </div>
                    <div class="feed-item-label-deleted" v-else-if="feedItem.params.type === PageChangeType.QuestionDeleted">
                        {{ t('page.feed.item.questionDeleted') }}
                    </div>
                </template>
            </div>
        </div>

        <div class="feed-item-visibility" v-if="isDesktop">
            <font-awesome-icon :icon="['fas', 'lock']" v-if="feedItem.visibility === Visibility.Private" v-tooltip="t('page.feed.item.private')" class="feed-item-visibility-icon" />
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