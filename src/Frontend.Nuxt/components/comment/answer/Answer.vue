<script lang="ts" setup>
import { ImageFormat } from '~~/components/image/imageFormatEnum'
import { CommentModel } from '../commentsStore'
import { useTimeElapsed } from "~~/composables/useTimeElapsed"

const { getTimeElapsedAsText } = useTimeElapsed()

interface Props {
    answer: CommentModel
    lastAnswer: boolean
    commentId: number
    highlightId?: number
}

const props = defineProps<Props>()
const { t } = useI18n()

const readMore = ref(false)
const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="panel-body commentRelativeContainer">
        <div class="answerContainer row" :class="{ 'highlight': props.answer.id === highlightId }">
            <div class="col-sm-1 hidden-xs"></div>
            <div class="col-xs-12 col-sm-11 answerUserDetails" v-bind:class="{ 'commentUserDetails': props.lastAnswer }">
                <div>
                    <NuxtLink :to="$urlHelper.getUserUrl(props.answer.creatorName, props.answer.creatorId)"
                        class="comment-header">
                        <Image class="commentUserImg" :src="props.answer.creatorImgUrl" :format="ImageFormat.Author" />
                        <span class="commentUserName">{{ props.answer.creatorName }}</span>
                    </NuxtLink>

                    <span class="commentDate">{{ getTimeElapsedAsText(props.answer.creationDate) }}</span>
                    <span v-if="props.answer.isSettled">
                        <br />
                        <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>
                            {{ t('comment.status.settled') }}</span>
                    </span>
                </div>
                <div class="answerTextContainer">
                    <span class="commentText" v-if="props.answer.text.length < 350" v-html="props.answer.text"></span>
                    <span v-else>
                        <span v-if="readMore" v-html="props.answer.text"></span>
                        <span v-else class="commentText" v-html="props.answer.text.slice(0, 350) + '...'"></span>
                        <button class="cursor-hand" @click="readMore = !readMore">
                            {{ readMore ? t('comment.readMore.less') : t('comment.readMore.more') }}
                        </button>
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.commentUserDetails {
    padding-bottom: 16px;
}

.commentRelativeContainer {
    padding: 8px 16px;

    .answerContainer {
        padding-top: 16px;
        margin-top: -8px;

        &.highlight {
            background: fade(@memo-green, 10%);
            margin-top: -16px;
        }
    }

    .answerUserDetails {
        // padding-bottom: 16px;

    }

}
</style>