<script lang="ts" setup>
import { useUserStore } from '../user/userStore'
import { Comment } from './commentStore'

const userStore = useUserStore()

interface Props {
    comment: Comment
    questionId: number
    creatorId: number
}

const props = defineProps<Props>()

const readMore = ref(false)
const foldOut = ref(false)
const showCommentAnswers = ref(false)

async function saveAnswer() {

}

async function markAsSettled() {
    const result = await $fetch<boolean>(`/apiVue/Comment/MarkAsSettled/${props.comment.id}`)
}

async function markAsUnsettled() {
    const result = await $fetch<boolean>(`/apiVue/Comment/MarkAsUnsettled/${props.comment.id}`)
}

</script>

<template>
    <div :class="{ commentPanelSpace: !props.comment.isSettled }" class="commentPanel" id="CommentComponent">
        <div class="panel-body">
            <div class="col-xs-12">

                <div v-if="!props.comment.isSettled">
                    <span class="commentTitle" v-if="props.comment.title.length > 0"
                        v-html="props.comment.title + '&nbsp &nbsp'"></span>
                    <span v-else>
                        <template class="commentTitle" v-if="props.comment.text.length > 25"
                            v-html="props.comment.text.slice(0, 25) + '...' + '&nbsp &nbsp'"></template>
                        <template class="commentTitle" v-else v-html="props.comment.text + '&nbsp &nbsp'"></template>
                    </span>

                    <span class="commentSpeechBubbleIcon">
                        <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                        <template class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">
                            &nbsp {{ props.comment.answers.length }} Beitrag
                        </template>
                        <template class="commentSpeechBubbleText" v-else>
                            &nbsp {{ props.comment.answers.length }} Beiträge
                        </template>
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
                        <template v-html="props.comment.title"> </template>
                        <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">&nbsp
                                {{ props.comment.answers.length }} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp {{ props.comment.answers.length }}
                                Beiträge</span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else-if="props.comment.text.length > 25">
                        <template v-html="props.comment.text.slice(0, 25) + '...'"></template>

                        <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">&nbsp
                                {{ props.comment.answers.length }} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp {{ props.comment.answers.length }}
                                Beiträge</span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else>
                        <template v-html="props.comment.text"></template>
                        <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                            <font-awesome-icon icon="fa-solid fa-comments" class="commentAnswersCount" />
                            <span class="commentSpeechBubbleText" v-if="props.comment.answers.length == 1">&nbsp
                                {{ props.comment.answers.length }} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp {{ props.comment.answers.length }}
                                Beiträge</span>
                        </span>
                    </div>
                </div>

                <div v-if="foldOut || !props.comment.isSettled">
                    <div class="commentTextContainer">
                        <span class="commentText" v-if="props.comment.text.length < 350" v-html="props.comment.text"></span>
                        <span v-else class="commentText">
                            <template v-if="readMore" v-html="props.comment.text">
                            </template>
                            <template v-else v-html="props.comment.text.slice(0, 360) + '...'">
                            </template>
                            <button @click="readMore = !readMore">
                                {{ readMore ? 'Weniger' : 'Mehr' }}
                            </button>
                        </span>
                    </div>
                    <div class="commentUserDetails">
                        <NuxtLink class="pointer" :to="props.comment.creatorUrl">
                            <img class="commentUserImg" :src="props.comment.imageUrl">
                            <span class="commentUserName">{{ props.comment.creatorName }}</span>
                        </NuxtLink>
                        <span class="greyed commentDate">
                            vor <span class="show-tooltip">{{ props.comment.creationDateNiceText }}</span>
                        </span>
                    </div>
                </div>

            </div>
        </div>

        <div class="commentAnswersContainer"
            v-if="foldOut && userStore.isAdmin || foldOut && props.comment.answers.length > 0 || !props.comment.isSettled && userStore.isAdmin || !props.comment.isSettled && props.comment.answers.length > 0 || !props.comment.isSettled && userStore.isLoggedIn">
            <div v-if="showCommentAnswers" class="" v-for="(answer, index) in props.comment.answers">
                <CommentAnswer :answer="answer" :comment-id="props.comment.id"
                    :last-answer="props.comment.answers.length - 1 == index" />
            </div>
            <div v-if="userStore.isLoggedIn && !props.comment.isSettled">
                <CommentAnswerAdd :parentCommentId="props.comment.id" />
            </div>

            <div class="commentButtonsContainer row" style="" v-if="userStore.isLoggedIn">
                <div class="col-xs-12 col-sm-12">
                    <div v-if="!props.comment.isSettled" class="pull-right col-xs-12 col-sm-4">
                        <button @click="saveAnswer()"
                            class="btn btn-primary memo-button col-xs-12 answerBtn">Antworten</button>
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
        </div>

    </div>
</template>