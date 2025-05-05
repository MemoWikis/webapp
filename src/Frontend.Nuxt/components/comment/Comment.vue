<script lang="ts" setup>
import { Editor } from '@tiptap/vue-3'
import { useUserStore } from '../user/userStore'
import { useCommentsStore, CommentModel } from './commentsStore'
import { useTimeElapsed } from "~~/composables/useTimeElapsed"

const { getTimeElapsedAsText } = useTimeElapsed()
const userStore = useUserStore()
const commentsStore = useCommentsStore()
const { t } = useI18n()

interface Props {
    comment: CommentModel
    questionId: number
    creatorId: number
    highlightId?: number
}

const props = defineProps<Props>()

const readMore = ref(false)
const foldOut = ref(false)
const showIsSettled = ref(false)
const showCommentAnswers = ref(true)
const { $logger, $urlHelper } = useNuxtApp(
)

async function markAsSettled() {
    const result = await $api<boolean>(`/apiVue/CommentAdd/MarkCommentAsSettled/`, {
        method: 'POST',
        body: { commentId: props.comment.id },
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })

    if (result) {
        commentsStore.loadComments()
    }
}

async function markAsUnsettled() {
    const result = await $api<boolean>(`/apiVue/CommentAdd/MarkCommentAsUnsettled/`, {
        method: 'POST',
        body: { commentId: props.comment.id },
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })

    if (result)
        commentsStore.loadComments()
}

const showAnswers = computed(() =>
    foldOut.value && props.comment.answers.length > 0 ||
    !props.comment.isSettled)

const highlightEmptyAnswer = ref(false)
const answerText = ref('')
function setAnswer(e: Editor) {
    answerText.value = e.getHTML()
}
const emit = defineEmits(['addAnswer'])

watch(answerText, (e) => {
    if (e.length >= 10)
        highlightEmptyAnswer.value = false
})
async function saveAnswer() {

    if (answerText.value.length < 10) {
        highlightEmptyAnswer.value = true
        return
    }
    const data = {
        commentId: props.comment.id,
        text: answerText.value
    }
    const result = await $api<CommentModel | null>(`/apiVue/CommentAdd/SaveAnswer/`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        body: data,
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })
    if (result) {
        answerText.value = ''
        emit('addAnswer', { commentId: props.comment.id, answer: result })
    } else {
        showIsSettled.value = true
    }
}
</script>

<template>
    <div :class="{ commentPanelSpace: !props.comment.isSettled }" class="commentPanel" id="CommentComponent">
        <div class="panel-body discussion" :class="{ 'highlight': props.comment.id === highlightId }">
            <div class="col-xs-12">

                <div v-if="!props.comment.isSettled" class="commentHeader">
                    <div class="commentTitle" v-if="props.comment.title?.length > 0" v-html="props.comment.title">
                    </div>
                    <template v-else>
                        <div class="commentTitle" v-if="props.comment.text.length > 25"
                            v-html="props.comment.text.slice(0, 25) + '...'"></div>
                        <div class="commentTitle" v-else v-html="props.comment.text"></div>
                    </template>
                </div>

                <div v-else class="pointer" @click="foldOut = !foldOut">
                    <div class="pull-right">
                        <span class="commentSettledBox">{{ t('comment.status.closed') }}</span>
                        <span>
                            <font-awesome-icon icon="fa-sharp fa-solid fa-chevron-up" v-if="foldOut" class="pointer" />
                            <font-awesome-icon icon="fa-sharp fa-solid fa-chevron-down" v-else class="pointer" />
                        </span>
                    </div>
                    <div class="commentTitle" v-if="props.comment.title.length > 0">
                        <span v-html="props.comment.title"></span>
                        <span class="commentSpeechBubbleIcon">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText">
                                &nbsp;
                                {{ t('comment.posts', props.comment.answers.length) }}
                            </span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else-if="props.comment.text.length > 25">
                        <span v-html="props.comment.text.slice(0, 25) + '...'"></span>

                        <span class="commentSpeechBubbleIcon">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText">
                                &nbsp;
                                {{ t('comment.posts', props.comment.answers.length) }}
                            </span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else>
                        <span v-html="props.comment.text"></span>
                        <span class="commentSpeechBubbleIcon">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText">
                                &nbsp;
                                {{ t('comment.posts', props.comment.answers.length) }}
                            </span>
                        </span>
                    </div>
                </div>

                <div v-if="foldOut || !props.comment.isSettled">
                    <div class="commentTextContainer">
                        <span class="commentText" v-if="props.comment.text.length < 350" v-html="props.comment.text"></span>
                        <span v-else class="commentText">
                            <span v-if="readMore" v-html="props.comment.text">
                            </span>
                            <span v-else v-html="props.comment.text.slice(0, 360) + '...'">
                            </span>
                            <button @click="readMore = !readMore" class="btn-link">
                                {{ readMore ? t('comment.readMore.less') : t('comment.readMore.more') }}
                            </button>
                        </span>
                    </div>
                    <div class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers" :class="{ 'clickable': props.comment.answers.length > 0 }">
                        <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                        <div class="commentSpeechBubbleText">
                            {{ t('comment.posts', props.comment.answers.length) }}
                            <template v-if="props.comment.answers.length > 0 && !showCommentAnswers">
                                {{ t('comment.answers.show') }}
                            </template>
                            <template v-else-if="props.comment.answers.length > 0 && showCommentAnswers">
                                {{ t('comment.answers.hide') }}
                            </template>
                        </div>
                    </div>

                    <div class="commentUserDetails">

                        <span class="greyed commentDate">
                            {{ t('comment.time.ago') }} <span class="show-tooltip">{{ getTimeElapsedAsText(props.comment.creationDate) }}</span> {{ t('comment.time.by') }}:
                        </span>
                        <NuxtLink class="pointer comment-header" v-if="props.comment.creatorId > 0" :to="$urlHelper.getUserUrl(props.comment.creatorName, props.comment.creatorId)">
                            <img class="commentUserImg" :src="props.comment.creatorImgUrl">
                            <span class="commentUserName">{{ props.comment.creatorName }}</span>
                        </NuxtLink>
                    </div>
                </div>

            </div>
        </div>

        <div class="commentAnswersContainer" v-if="showAnswers">

            <CommentAnswer v-if="showCommentAnswers" v-for="(answer, index) in props.comment.answers" :answer="answer" :comment-id="props.comment.id" :last-answer="props.comment.answers.length - 1 === index"
                :highlight-id="props.highlightId" />

            <CommentAnswerAdd v-if="userStore.isLoggedIn && !props.comment.isSettled" :parentCommentId="props.comment.id" :highlight-empty-fields="highlightEmptyAnswer" @set-answer="setAnswer" :content="answerText" />

            <div class="commentButtonsContainer row" style="" v-if="userStore.isLoggedIn && !props.comment.isSettled || userStore.isAdmin">
                <div class="col-xs-12 col-sm-12">
                    <div v-if="!props.comment.isSettled" class="pull-right col-xs-12 col-sm-4">
                        <button @click="saveAnswer()" class="btn btn-primary memo-button col-xs-12 answerBtn">
                            {{ t('comment.buttons.reply') }}
                        </button>
                    </div>
                    <div class="pull-right col-xs-12 col-sm-5">
                        <button v-if="userStore.isAdmin && !props.comment.isSettled || props.creatorId === userStore.id && !props.comment.isSettled" @click="markAsSettled()" class="btn btn-lg btn-link memo-button col-xs-12">
                            {{ t('comment.buttons.closeDiscussion') }}
                        </button>
                        <button v-if="userStore.isAdmin && props.comment.isSettled" @click.stop="markAsUnsettled()" class="btn btn-lg btn-link memo-button col-xs-12">
                            {{ t('comment.buttons.reopenDiscussion') }}
                        </button>
                    </div>
                </div>
            </div>
            <div v-if="showIsSettled" class="discurs-is-settled col-xs-12 col-sm-12">
                {{ t('comment.messages.discussionClosed') }}
            </div>
        </div>
    </div>
</template>
<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.discurs-is-settled {
    background-color: @memo-yellow;
    font-size: 14px;
    font-weight: bold;
    margin-top: 10px;
    padding: 15px;
    display: flex;
    justify-content: center;
}

.commentAnswersCount {
    font-size: 0.8em;
}

.commentSpeechBubbleText {
    padding-left: 8px;
    user-select: none;
}

.commentSpeechBubbleIcon {
    cursor: default;
    margin-bottom: 16px;
    padding-bottom: 8px;
    border-bottom: 1px solid @memo-grey-light;
    display: flex;
    justify-content: end;

    &.clickable {
        cursor: pointer;
        color: @memo-blue-link;
    }
}

.commentUserDetails {
    display: flex;
    justify-content: flex-end;
    align-items: center;

    .commentDate {
        padding-right: 8px;
    }
}

.commentAnswersContainer {
    padding-top: 16px;
}

.discussion {
    &.highlight {
        background-color: fade(@memo-green, 10%)
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.commentTitle {
    p {
        margin-bottom: 0;
        color: @memo-blue;
    }
}
</style>