<script lang="ts" setup>
import { ImageFormat } from '~~/components/image/imageFormatEnum'
import { CommentModel } from '../commentsStore'

interface Props {
    answer: CommentModel
    lastAnswer: boolean
    commentId: number
}

const props = defineProps<Props>()

const readMore = ref(false)
const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="panel-body commentRelativeContainer row">
        <div class="col-sm-2 hidden-xs"></div>
        <div class="col-xs-12 col-sm-10 answerUserDetails" v-bind:class="{ commentUserDetails: props.lastAnswer }">
            <div>
                <NuxtLink :to="$urlHelper.getUserUrl(props.answer.creatorName, props.answer.creatorId)"
                    class="comment-header">
                    <Image class="commentUserImg" :src="props.answer.creatorImgUrl" :format="ImageFormat.Author" />
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
                    <span v-if="readMore" v-html="props.answer.text"></span>
                    <span v-else class="commentText" v-html="props.answer.text.slice(0, 350) + '...'"></span>
                    <button class="cursor-hand" @click="readMore = !readMore">
                        {{ readMore ? 'Weniger' : 'Mehr' }}
                    </button>
                </span>
            </div>
        </div>
    </div>
</template>