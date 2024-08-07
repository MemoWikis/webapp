<script lang="ts" setup>
import { Editor } from '@tiptap/vue-3'
import { useUserStore } from '../user/userStore'
import { useCommentsStore, CommentModel } from './commentsStore'

const userStore = useUserStore()
const commentsStore = useCommentsStore()
interface Props {
    comment: CommentModel
    questionId: number
    creatorId: number
}

const props = defineProps<Props>()

const readMore = ref(false)
const foldOut = ref(false)
const showIsSettled = ref(false)
const showCommentAnswers = ref(false)
const { $logger, $urlHelper } = useNuxtApp()

function handleWindowClick(e: MouseEvent) {
    showIsSettled.value = false
}

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
        console.log('result', result)
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

watch(showAnswers, (newVal) => {
    if (newVal === true)
        console.log('props ANswer', props.comment.answers)
})

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

function toggleShowCommentAnswers() {
    showCommentAnswers.value = !showCommentAnswers.value
}

</script>

<template>
    <div :class="{ commentPanelSpace: !props.comment.isSettled }" class="commentPanel" id="CommentComponent">
        <div class="panel-body">
            <div class="col-xs-12">

                <div v-if="!props.comment.isSettled">
                    <span class="commentTitle" v-if="props.comment.title.length > 0"
                        v-html="props.comment.title + '&nbsp &nbsp'">
                    </span>
                    <template v-else>
                        <span class="commentTitle" v-if="props.comment.text.length > 25"
                            v-html="props.comment.text.slice(0, 25) + '...' + '&nbsp &nbsp'"></span>
                        <span class="commentTitle" v-else v-html="props.comment.text + '&nbsp &nbsp'"></span>
                    </template>

                    <span class="commentSpeechBubbleIcon" @click="toggleShowCommentAnswers()">
                        <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                        <div class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">
                            &nbsp; {{ props.comment.answers.length }} Beitrag
                        </div>
                        <div class="commentSpeechBubbleText" v-else>
                            &nbsp; {{ props.comment.answers.length }} Beiträge
                        </div>
                    </span>
                </div>

                <div v-else class="pointer" @click="foldOut = !foldOut">
                    <div class="pull-right">
                        <span class="commentSettledBox">geschlossen</span>
                        <span>
                            <font-awesome-icon icon="fa-sharp fa-solid fa-chevron-up" v-if="foldOut" class="pointer" />
                            <font-awesome-icon icon="fa-sharp fa-solid fa-chevron-down" v-else class="pointer" />
                        </span>
                    </div>
                    <div class="commentTitle" v-if="props.comment.title.length > 0">
                        <span v-html="props.comment.title"></span>
                        <span class="commentSpeechBubbleIcon">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">&nbsp;
                                {{ props.comment.answers.length }} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp; {{ props.comment.answers.length }}
                                Beiträge</span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else-if="props.comment.text.length > 25">
                        <span v-html="props.comment.text.slice(0, 25) + '...'"></span>

                        <span class="commentSpeechBubbleIcon">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">&nbsp;
                                {{ props.comment.answers.length }} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp; {{ props.comment.answers.length }}
                                Beiträge</span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else>
                        <span v-html="props.comment.text"></span>
                        <span class="commentSpeechBubbleIcon">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">&nbsp;
                                {{ props.comment.answers.length }} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp; {{ props.comment.answers.length }}
                                Beiträge</span>
                        </span>
                    </div>
                </div>

                <div v-if="foldOut || !props.comment.isSettled">
                    <div class="commentTextContainer">
                        <span class="commentText" v-if="props.comment.text.length < 350"
                            v-html="props.comment.text"></span>
                        <span v-else class="commentText">
                            <span v-if="readMore" v-html="props.comment.text">
                            </span>
                            <span v-else v-html="props.comment.text.slice(0, 360) + '...'">
                            </span>
                            <button @click="readMore = !readMore" class="btn-link">
                                {{ readMore ? 'Weniger' : 'Mehr' }}
                            </button>
                        </span>
                    </div>
                    <div class="commentUserDetails">
                        <NuxtLink class="pointer comment-header" v-if="props.comment.creatorId > 0"
                            :to="$urlHelper.getUserUrl(props.comment.creatorName, props.comment.creatorId)">
                            <img class="commentUserImg" :src="props.comment.creatorImgUrl">
                            <span class="commentUserName">{{ props.comment.creatorName }}</span>
                        </NuxtLink>
                        <span class="greyed commentDate">
                            vor <span class="show-tooltip">{{ props.comment.creationDateNiceText }}</span>
                        </span>
                    </div>
                </div>

            </div>
        </div>

        <div class="commentAnswersContainer" v-if="showAnswers">

            <CommentAnswer v-if="showCommentAnswers" v-for="(answer, index) in props.comment.answers" :answer="answer"
                :comment-id="props.comment.id" :last-answer="props.comment.answers.length - 1 == index" />

            <CommentAnswerAdd v-if="userStore.isLoggedIn && !props.comment.isSettled"
                :parentCommentId="props.comment.id" :highlight-empty-fields="highlightEmptyAnswer"
                @set-answer="setAnswer" :content="answerText" />

            <div class="commentButtonsContainer row" style=""
                v-if="userStore.isLoggedIn && !props.comment.isSettled || userStore.isAdmin">
                <div class="col-xs-12 col-sm-12">
                    <div v-if="!props.comment.isSettled" class="pull-right col-xs-12 col-sm-4">
                        <button @click="saveAnswer()" class="btn btn-primary memo-button col-xs-12 answerBtn">
                            Antworten
                        </button>
                    </div>
                    <div class="pull-right col-xs-12 col-sm-5">
                        <button
                            v-if="userStore.isAdmin && !props.comment.isSettled || props.creatorId == userStore.id && !props.comment.isSettled"
                            @click="markAsSettled()" class="btn btn-lg btn-link memo-button col-xs-12">
                            Diskussion schliessen
                        </button>
                        <button v-if="userStore.isAdmin && props.comment.isSettled" @click.stop="markAsUnsettled()"
                            class="btn btn-lg btn-link memo-button col-xs-12">
                            Diskussion wieder eröffnen
                        </button>
                    </div>
                </div>
            </div>
            <div v-if="showIsSettled" class="discurs-is-settled col-xs-12 col-sm-12">Leider wurde die Diskussion in der
                Zwischenzeit
                geschlossen
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
</style>