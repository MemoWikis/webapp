<script lang="ts" setup>
import { ImageStyle } from '~~/components/image/imageStyleEnum';
import { CommentModel } from '../commentsStore'

interface Props {
    answer: CommentModel
    lastAnswer: boolean
    commentId: number
}

const props = defineProps<Props>()

const readMore = ref(false)
</script>

<template>
    <div class="panel-body commentRelativeContainer row">
        <div class="col-sm-2 hidden-xs"></div>
        <div class="col-xs-12 col-sm-10 answerUserDetails" v-bind:class="{ commentUserDetails: props.lastAnswer }">
            <div>
                <NuxtLink :to="`/Nutzer/${props.answer.creatorEncodedName}/${props.answer.creatorId}`">
                    <Image class="commentUserImg" :url="props.answer.imageUrl" :style="ImageStyle.Author" />
                    <span class="commentUserName">{{ props.answer.creatorName }}</span>
                </NuxtLink>

                <span class="commentDate">vor {{ props.answer.creationDateNiceText }}</span>
                <span v-if="props.answer.isSettled">
                    <br />
                    <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>
                        Dieser Kommentar wurde als erledigt markiert.</span>
                </span>
            </div>
            <div class="answerTextContainer">
                <span class="commentText" v-if="props.answer.text.length < 350" v-html="props.answer.text"></span>
                <span v-else>
                    <template v-if="readMore" v-html="props.answer.text"></template>
                    <template v-else class="commentText" v-html="props.answer.text.slice(0, 350) + '...'"></template>
                    <button class="cursor-hand" @click="readMore = !readMore">
                        {{ readMore ? 'Weniger' : 'Mehr' }}
                    </button>
                </span>
            </div>
        </div>
    </div>
</template>