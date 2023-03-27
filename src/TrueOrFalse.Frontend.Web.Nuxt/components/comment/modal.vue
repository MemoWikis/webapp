<script lang="ts" setup>
import { useCommentsStore } from './commentStore'

const commentsStore = useCommentsStore()

const showSettledComments = ref(false)


</script>

<template>
    <Modal :show="commentsStore.show" @close="commentsStore.show = false" :show-close-button="true"
        @keydown.esc="commentsStore.show = false">
        <template v-slot:header>Diskussion</template>
        <template v-slot:body>
            <div id="CommentsSection">
                <div class="commentSection">
                    <div v-if="commentsStore.comments">
                        <div v-for="comment in commentsStore.comments" class="comment">
                            <Comment :comment="comment" :question-id="commentsStore.questionId"
                                :creator-id="comment.creatorId" />
                        </div>
                        <div v-if="commentsStore.settledComments?.length > 0">
                            <div class="commentSettledInfo">
                                Die Frage hat {{ commentsStore.settledComments.length }} geschlossene {{
                                    commentsStore.settledComments.length == 1 ? 'Diskussion' : 'Diskussionen' }}
                                <button class="cursor-hand" @click="showSettledComments != showSettledComments">
                                    ({{ showSettledComments ? 'ausblenden' : 'einblenden' }})
                                </button>

                            </div>

                            <div v-if="showSettledComments">
                                <div v-for="settledComment in commentsStore.settledComments " class="comment">
                                    <Comment :comment="settledComment" :question-id="commentsStore.questionId"
                                        :creator-id="settledComment.creatorId" />
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