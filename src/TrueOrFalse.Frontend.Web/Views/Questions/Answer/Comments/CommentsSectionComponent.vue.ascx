<div id="CommentsSection">
    <div class="commentSection">
        <div v-for="comment in comments" class="comment">
            <comment-component :comment="comment" :question-id="questionId" :currentUserImageUrl="currentUserImageUrl" :currentUserId="currentUserId" />
        </div>
        <div v-if="settledComments.Count > 0">
            <div v-if="showSettledComments" class="commentSettledInfo">
                Diese Frage hat {{settledComments.Count}}
                <span v-if="settledComments.Count != 1"> weitere als erledigt markiert Kommentare
                </span>
                <span v-if="settledComments.Count == 1"> weiteren als erledigt markiert Kommentar
                </span>
                (<a class="cursor-hand" @click="showSettledComments = false" data-question-id="questionId">alle verstecken</a>).
            </div>
            <div v-else class="commentSettledInfo">
                Diese Frage hat {{settledComments.Count}}
                <span v-if="settledComments.Count != 1"> weitere als erledigt markierte Kommentare
                </span>
                <span v-if="settledComments.Count == 1"> weiteren als erledigt markiert Kommentar
                </span>
                (<a class="cursor-hand" @click="showSettledComments = true" data-question-id="questionId">alle anzeigen</a>).
            </div>

            <div v-if="showSettledComments">
                <div v-for="settledComment in settledComments " class="comment">
                    <comment-component :comment="settledComment" :id="questionId"/>
                </div>
            </div>
        </div>
        <div class="addCommentComponent">
            <add-comment-component :currentUserImageUrl="currentUserImageUrl" :questionId="questionId"/>
        </div>
    </div>
</div>