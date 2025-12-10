<script lang="ts" setup>
import { CommentModel } from '~/components/comment/commentsStore'
import { QuestionFeedItem, QuestionChangeType } from './feedHelper'

interface Props {
    questionFeedItem: QuestionFeedItem
    comment?: CommentModel
    highlightId?: number
}
const props = defineProps<Props>()

const showDiff = ref(true)

const { $urlHelper } = useNuxtApp()
const emit = defineEmits(['add-answer'])
</script>

<template>
    <div class="feed-modal-question-container">

        <template v-if="questionFeedItem.type === QuestionChangeType.AddComment && comment">

            <!-- <div class="show-diff-toggle">
                <div class="show-diff-toggle-button" :class="{ 'is-active': showDiff }" @click="showDiff = true">Änderungen anzeigen</div>
                <div class="show-diff-toggle-button" :class="{ 'is-active': !showDiff }" @click="showDiff = false">Seitenvorschau</div>
            </div>

            <div class="feed-modal-question-body" v-if="questionFeedItem.contentChange.diffContent && showDiff">
                <div class="feed-modal-diff-content" v-html="questionFeedItem.contentChange.diffContent"></div>
            </div>
            <div class="feed-modal-question-body" v-else-if="questionFeedItem.contentChange.newContent && !showDiff">
                <div class="feed-modal-diff-content" v-html="questionFeedItem.contentChange.newContent"></div>
            </div> -->
            <Comment :comment="comment" :question-id="questionFeedItem.questionId" :creator-id="comment.creatorId" :highlight-id="highlightId" @add-answer="emit('add-answer')" />
        </template>

    </div>


</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-question-body {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;

    .feed-modal-content {
        width: 50%;
        padding: 16px;
    }

    .feed-modal-diff-content {
        padding: 16px;
    }
}

.show-diff-toggle {
    display: flex;
    align-items: center;
    margin-top: 20px;
    height: 56px;
    padding-bottom: 20px;
    border-bottom: solid 1px @memo-grey-lighter;

    .show-diff-toggle-button {
        padding: 8px 16px;
        cursor: pointer;
        border-radius: 24px;
        background: white;
        color: @memo-grey-darker;

        &:hover {
            filter: brightness(0.95);
        }

        &:active {
            filter: brightness(0.9);
        }

        &.is-active {
            background: @memo-info;

        }
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-question-body {
    ins {
        // background: fade(@memo-green, 10%);
        border-radius: 4px;
        color: @memo-green;

        img {
            border: solid 4px @memo-green;
        }
    }

    del {
        // background: fade(@memo-wish-knowledge-red, 10%);
        text-decoration: line-through;
        border-radius: 4px;
        color: @memo-wish-knowledge-red;

        img {
            border: solid 4px @memo-wish-knowledge-red;
        }
    }
}
</style>