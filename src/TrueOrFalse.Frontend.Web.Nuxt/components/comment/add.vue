<script lang="ts" setup>
import { useUserStore } from '../user/userStore'
import { useCommentsStore } from './commentStore'

const commentsStore = useCommentsStore()
const userStore = useUserStore()
const highlightEmptyTitle = ref(false)
const commentTitle = ref<string>('')

const highlightEmptyComment = ref(false)
const commentContent = ref<string>('')

async function saveComment() {

}
</script>

<template>
    <div id="AddCommentComponent">
        <div v-if="userStore.isLoggedIn" class="commentAddContainer">
            <div class="panel-body" style="padding-right: 10px;">
                <div>
                    <div class="commentAnswerAddTopSpace row">
                        <div class="col-xs-1 addCommentUsrImgContainer">
                            <img class="addCommentUsrImg" :src="userStore.imgUrl">
                        </div>
                        <div>
                            <div class="inline-question-editor col-xs-11 noPadding">

                                <div class="input-container" id="AddCommentTitleFormContainer">
                                    <div class="overline-s no-line">Titel der Diskussion</div>
                                    <CommentTitleEditor :highlight-empty-fields="highlightEmptyTitle"
                                        :content="commentTitle" />
                                    <div v-if="highlightEmptyTitle" class="field-error">Bitte formuliere einen Titel</div>
                                </div>
                                <div class="input-container" id="AddCommentTextFormContainer">
                                    <div class="overline-s no-line">Dein Diskussionsbeitrag</div>

                                    <CommentContentEditor :highlight-empty-fields="highlightEmptyComment"
                                        :content="commentContent" />
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-1"></div>
                        <div class="col-xs-11 noPadding">
                            <button class="btn btn-primary memo-button pull-right" @click="saveComment()">
                                Diskussion hinzufügen
                            </button>
                            <button class="btn btn-lg btn-link memo-button pull-right " @click="commentsStore.show = false">
                                Abbrechen
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div v-else class="row commentLoginContainer">
            <div class="col-xs-12 commentLoginText">
                Um zu kommentieren, musst du eingeloggt sein. &nbsp
                <NuxtLink href="/Registrieren" @click="commentsStore.show = false" class="pointer">
                    Jetzt registrieren
                </NuxtLink>
            </div>
        </div>
    </div>
</template>