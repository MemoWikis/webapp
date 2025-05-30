<script lang="ts" setup>
import { Editor } from '@tiptap/vue-3'
import { useUserStore } from '../user/userStore'
import { useCommentsStore } from './commentsStore'

const commentsStore = useCommentsStore()
const userStore = useUserStore()
const { t } = useI18n()

const highlightEmptyTitle = ref(false)
const commentTitle = ref<string>('')
function setTitle(editor: Editor) {
    commentTitle.value = editor.getHTML()
}
const highlightEmptyComment = ref(false)
const commentText = ref<string>('')
function setText(editor: Editor) {
    commentText.value = editor.getHTML()
}

watch([commentText, commentTitle], ([text, title]) => {
    if (title.length >= 10)
        highlightEmptyTitle.value = false

    if (text.length >= 10)
        highlightEmptyComment.value = false
})
const { $logger } = useNuxtApp()

async function saveComment() {
    if (commentTitle.value.length < 10 || commentText.value.length < 10) {
        if (commentTitle.value.length < 10)
            highlightEmptyTitle.value = true

        if (commentText.value.length < 10)
            highlightEmptyComment.value = true

        return
    }

    const data = {
        id: commentsStore.questionId,
        title: commentTitle.value,
        text: commentText.value
    }
    const result = await $api<boolean>(`/apiVue/CommentAdd/SaveComment/`, {
        mode: 'cors',
        method: 'POST',
        body: data,
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    }
    )

    if (result) {
        commentTitle.value = ''
        commentText.value = ''
        commentsStore.loadComments()
    }
}
</script>

<template>
    <div id="AddCommentComponent">
        <div v-if="userStore.isLoggedIn" class="commentAddContainer">
            <div class="panel-body" style="padding-right: 10px;">
                <div class="row">
                    <div class="col-xs-1 addCommentUsrImgContainer">
                        <img class="addCommentUsrImg" :src="userStore.imgUrl">
                    </div>
                    <div>
                        <div class="inline-question-editor col-xs-11 noPadding">

                            <div class="input-container" id="AddCommentTitleFormContainer">
                                <div class="overline-s no-line">{{ t('comment.form.title') }}</div>
                                <CommentTitleEditor :highlight-empty-fields="highlightEmptyTitle"
                                    :content="commentTitle" @set-title="setTitle" />
                            </div>
                            <div class="input-container" id="AddCommentTextFormContainer">
                                <div class="overline-s no-line">{{ t('comment.form.text') }}</div>

                                <CommentTextEditor :highlight-empty-fields="highlightEmptyComment"
                                    :content="commentText" @set-text="setText" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-1"></div>
                    <div class="col-xs-11 noPadding">
                        <button class="btn btn-primary memo-button pull-right" @click="saveComment()">
                            {{ t('comment.buttons.addDiscussion') }}
                        </button>
                        <button class="btn btn-lg btn-link memo-button pull-right "
                            @click="commentsStore.show = false">
                            {{ t('comment.buttons.cancel') }}
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div v-else class="row commentLoginContainer">
            <div class="col-xs-12 commentLoginText">
                {{ t('comment.login.required') }} &nbsp;
                <NuxtLink :to="`/${t('url.register')}`" @click="commentsStore.show = false" class="pointer">
                    {{ t('comment.login.register') }}
                </NuxtLink>
            </div>
        </div>
    </div>
</template>