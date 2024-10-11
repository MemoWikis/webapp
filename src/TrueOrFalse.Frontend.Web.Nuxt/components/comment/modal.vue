<script lang="ts" setup>
import { useCommentsStore, CommentModel } from './commentsStore'

const commentsStore = useCommentsStore()
function addAnswerToUnsettledComments(e: { commentId: number, answer: CommentModel }) {
    commentsStore.unsettledComments.find(c => c.id == e.commentId)?.answers.push(e.answer)
}

const showSettledComments = ref(false)
function addAnswerToSettledComments(e: { commentId: number, answer: CommentModel }) {
    commentsStore.settledComments.find(c => c.id == e.commentId)?.answers.push(e.answer)
}
</script>

<template>
    <Modal :show="commentsStore.show" @close="commentsStore.show = false" :show-close-button="false"
        @keydown.esc="commentsStore.show = false">
        <template v-slot:header>
            <h2>Diskussion</h2>
        </template>
        <template v-slot:body>
            <div id="CommentsSection">
                <div class="commentSection">
                    <div>
                        <div v-for="comment in commentsStore.unsettledComments" :key="comment.id" class="comment">
                            <Comment :comment="comment" :question-id="commentsStore.questionId"
                                :creator-id="comment.creatorId" @add-answer="addAnswerToUnsettledComments" />
                        </div>
                        <div v-if="commentsStore.settledComments?.length > 0">
                            <div class="commentSettledInfo">
                                Die Frage hat {{ commentsStore.settledComments.length }} geschlossene {{
                                    commentsStore.settledComments.length == 1 ? 'Diskussion' : 'Diskussionen' }}
                                <button class="cursor-hand btn-link" @click="showSettledComments = !showSettledComments">
                                    ({{ showSettledComments ? 'ausblenden' : 'einblenden' }})
                                </button>

                            </div>

                            <div v-if="showSettledComments">
                                <div v-for="settledComment in commentsStore.settledComments" :key="settledComment.id" class="comment">
                                    <Comment :comment="settledComment" :question-id="commentsStore.questionId"
                                        :creator-id="settledComment.creatorId" @add-answer="addAnswerToSettledComments" />
                                </div>
                            </div>
                        </div>
                        <div class="addCommentComponent">
                            <CommentAdd />
                        </div>
                    </div>
                </div>
            </div>
        </template>
    </Modal>
</template>

<style lang="less" scoped>

h2 {
    margin-bottom: 36px;
}

</style>