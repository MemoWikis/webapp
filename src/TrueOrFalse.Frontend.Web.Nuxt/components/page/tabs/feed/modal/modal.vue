<script lang="ts" setup>
import { CommentModel, useCommentsStore } from '~/components/comment/commentsStore'
import { ContentChange, FeedItem, FeedType, getPageChangeTypeName, QuestionChangeType, PageChangeType } from '../feedHelper'
import { useSpinnerStore } from '~/components/spinner/spinnerStore'

interface Props {
    show: boolean,
    feedItem: FeedItem,
}

const props = defineProps<Props>()
const commentsStore = useCommentsStore()
const spinnerStore = useSpinnerStore()
const emit = defineEmits(['close', 'get-feed-items'])

const isPage = ref(false)
const isQuestion = ref(false)

function initModal() {
    spinnerStore.hideSpinner()

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
    spinnerStore.hideSpinner()

    if (val) {
        initModal()
    }
})

const contentChange = ref<ContentChange>()

const getContentChange = async () => {
    if (isPage.value && props.feedItem.pageFeedItem?.type === PageChangeType.Text) {
        spinnerStore.showSpinner()
        const data = {
            pageId: props.feedItem.pageFeedItem.pageId,
            changeId: props.feedItem.pageFeedItem.categoryChangeId,
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
        spinnerStore.hideSpinner()

    }
}

const comment = ref<CommentModel>()

const getComment = async () => {
    if (isQuestion.value && props.feedItem.questionFeedItem?.type === QuestionChangeType.AddComment && props.feedItem.questionFeedItem.comment) {
        spinnerStore.showSpinner()
        const response: CommentModel = await commentsStore.loadComment(props.feedItem.questionFeedItem.comment.id)
        comment.value = response
        spinnerStore.hideSpinner()

    }
}

const addAnswer = () => {
    getComment()
    emit('get-feed-items')
}

const niceDate = (date: string) => {
    return new Date(date).toLocaleDateString('de-DE', { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' })
}

</script>

<template>

    <Modal :show="props.show" @close="emit('close')" :show-close-button="true" :has-header="true">
        <template #header>
            <h2 v-if="isPage && feedItem.pageFeedItem">{{ getPageChangeTypeName(feedItem.pageFeedItem.type) }}</h2>
            <h2 v-else-if="isQuestion && feedItem.questionFeedItem?.type === QuestionChangeType.AddComment">Neuer Kommentar</h2>
            <div class="modal-header-date">
                {{ niceDate(feedItem.date) }}
                von:
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
