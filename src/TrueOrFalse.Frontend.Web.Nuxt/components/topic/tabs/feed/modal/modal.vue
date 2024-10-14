<script lang="ts" setup>
import { CommentModel, useCommentsStore } from '~/components/comment/commentsStore'
import { ContentChange, FeedItem, FeedType, getTopicChangeTypeName, QuestionChangeType, TopicChangeType } from '../feedHelper'

interface Props {
    show: boolean,
    feedItem: FeedItem,
}

const props = defineProps<Props>()
const commentsStore = useCommentsStore()

const emit = defineEmits(['close', 'get-feed-items'])

const isTopic = ref(false)
const isQuestion = ref(false)

function initModal() {
    isTopic.value = props.feedItem.type === FeedType.Topic
    isQuestion.value = props.feedItem.type === FeedType.Question
    if (isTopic.value) {
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
    if (val) {
        initModal()
    }
})

const contentChange = ref<ContentChange>()

const getContentChange = async () => {
    if (isTopic.value && props.feedItem.topicFeedItem?.type === TopicChangeType.Text) {
        const data = {
            topicId: props.feedItem.topicFeedItem.topicId,
            changeId: props.feedItem.topicFeedItem.categoryChangeId
        }
        const response = await $api<ContentChange>(`/apiVue/FeedModalTopic/GetContentChange/`, {
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
    }
}

const comment = ref<CommentModel>()

const getComment = async () => {
    if (isQuestion.value && props.feedItem.questionFeedItem?.type === QuestionChangeType.AddComment && props.feedItem.questionFeedItem.comment) {
        const response: CommentModel = await commentsStore.loadComment(props.feedItem.questionFeedItem.comment.id)
        comment.value = response
    }
}

const addAnswer = () => {
    getComment()
    emit('get-feed-items')
}

</script>

<template>

    <Modal :show="props.show" @close="emit('close')" :show-close-button="true" :has-header="true">
        <template #header>
            <h2 v-if="isTopic && feedItem.topicFeedItem">{{ getTopicChangeTypeName(feedItem.topicFeedItem.type) }}</h2>
            <h2 v-else-if="isQuestion && feedItem.questionFeedItem?.type === QuestionChangeType.AddComment">Neuer Kommentar</h2>
        </template>
        <template #body>
            <TopicTabsFeedModalTopic v-if="isTopic && feedItem.topicFeedItem" :topicFeedItem="feedItem.topicFeedItem" :content-change="contentChange" />
            <TopicTabsFeedModalQuestion v-else-if="isQuestion && feedItem.questionFeedItem" :question-feed-item="feedItem.questionFeedItem" :comment="comment" :highlight-id="feedItem.questionFeedItem.comment?.id" @add-answer="addAnswer" />
        </template>
    </Modal>
</template>


<style lang="less" scoped>

h2 {
    margin-bottom: 36px;
}

</style>
