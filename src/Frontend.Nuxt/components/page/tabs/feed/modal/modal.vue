<script lang="ts" setup>
import { CommentModel, useCommentsStore } from '~/components/comment/commentsStore'
import { ContentChange, FeedItem, FeedType, getPageChangeTypeKey, QuestionChangeType, PageChangeType } from '../feedHelper'
import { useLoadingStore } from '~/components/loading/loadingStore'

interface Props {
    show: boolean,
    feedItem: FeedItem,
}

const props = defineProps<Props>()
const commentsStore = useCommentsStore()
const loadingStore = useLoadingStore()
const emit = defineEmits(['close', 'get-feed-items'])

const isPage = ref(false)
const isQuestion = ref(false)

function initModal() {
    loadingStore.stopLoading()

    isPage.value = props.feedItem.type === FeedType.Page
    isQuestion.value = props.feedItem.type === FeedType.Question
    comment.value = undefined
    contentChange.value = undefined
    if (isPage.value) {
        getContentChange()
    } else if (isQuestion.value)
        getComment()
}

onBeforeMount(() => {
    if (props.show) {
        initModal()
    }
})

watch(() => props.show, (val) => {
    loadingStore.stopLoading()

    if (val) {
        initModal()
    }
})

const contentChange = ref<ContentChange>()

const getContentChange = async () => {
    if (isPage.value && props.feedItem.pageFeedItem?.type === PageChangeType.Text) {
        loadingStore.startLoading()
        const data = {
            pageId: props.feedItem.pageFeedItem.pageId,
            changeId: props.feedItem.pageFeedItem.pageChangeId,
            oldestChangeId: props.feedItem.pageFeedItem.oldestChangeIdInGroup
        }
        const response = await $api<ContentChange>(`/apiVue/FeedModalPage/GetContentChange/`, {
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            body: data,
            onResponseError(context) {
                const { $logger } = useNuxtApp()
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
            }
        })
        contentChange.value = response
        loadingStore.stopLoading()

    }
}

const comment = ref<CommentModel>()

const getComment = async () => {
    if (isQuestion.value && props.feedItem.questionFeedItem?.type === QuestionChangeType.AddComment && props.feedItem.questionFeedItem.comment) {
        loadingStore.startLoading()
        const response: CommentModel = await commentsStore.loadComment(props.feedItem.questionFeedItem.comment.id)
        comment.value = response
        loadingStore.stopLoading()

    }
}

const addAnswer = () => {
    getComment()
    emit('get-feed-items')
}
const { t, localeProperties } = useI18n()

const niceDate = (date: string) => {
    const iso = localeProperties.value.iso as string;
    return new Date(date).toLocaleDateString(iso, { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' })
}
const { $urlHelper } = useNuxtApp()
</script>

<template>

    <Modal :show="props.show" :feed-item="props.feedItem" @close="emit('close')" :show-close-button="true" :has-header="true">
        <template #header>
            <h2 v-if="isPage && feedItem.pageFeedItem">{{ t(getPageChangeTypeKey(feedItem.pageFeedItem.type)) }}</h2>
            <h2 v-else-if="isQuestion && feedItem.questionFeedItem?.type === QuestionChangeType.AddComment">{{ t('page.feed.modal.newComment') }}</h2>
            <div class="modal-header-date">
                {{ niceDate(feedItem.date) }}
                {{ t('page.feed.modal.by') }}:
                <NuxtLink :to="$urlHelper.getUserUrl(feedItem.author.name, feedItem.author.id)">
                    {{ feedItem.author.name }}
                </NuxtLink>
            </div>
        </template>
        <template #body>
            <PageTabsFeedModalPage v-if="isPage && feedItem.pageFeedItem" :pageFeedItem="feedItem.pageFeedItem" :content-change="contentChange" />
            <PageTabsFeedModalQuestion v-else-if="isQuestion && feedItem.questionFeedItem" :question-feed-item="feedItem.questionFeedItem" :comment="comment" :highlight-id="feedItem.questionFeedItem.comment?.id" @add-answer="addAnswer" />
        </template>
    </Modal>
</template>


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

h2 {
    margin-bottom: 0px;
}

.modal-header-date {
    font-size: 14px;
    color: @memo-grey-dark;
    margin-top: 5px;
    margin-bottom: 36px;
}
</style>
