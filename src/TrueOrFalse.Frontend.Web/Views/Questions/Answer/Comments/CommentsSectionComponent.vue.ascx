﻿<div id="CommentsSection">
    <div class="commentSection">
        <div v-if="allLoaded">
            <div v-for="comment in comments" class="comment">
                <comment-component :comment="comment" :question-id="questionId" :current-user-image-url="currentUserImageUrl" :current-user-id="currentUserId" :current-user-name="currentUserName" :is-admin-string="isAdmin"/>
            </div>
            <div v-if="settledComments.length > 0">
                <div  class="commentSettledInfo">
                    Die Frage hat {{settledComments.length}}
                    <span v-if="settledComments.length == 1">
                        geschlossene Diskussion
                    </span>
                    <span v-else>
                        geschlossene Diskussionen
                    </span>

                    <a v-if="showSettledComments" class="cursor-hand" @click="showSettledComments = false" data-question-id="questionId">(ausblenden)</a>
                    <a v-else class="cursor-hand" @click="showSettledComments = true" data-question-id="questionId">(einblenden)</a>

                </div>

                <div v-if="showSettledComments">
                    <div v-for="settledComment in settledComments " class="comment">
                        <comment-component :comment="settledComment" :question-id="questionId" :current-user-image-url="currentUserImageUrl" :current-user-id="currentUserId" :current-user-name="currentUserName" :is-admin-string="isAdmin"/>
                    </div>
                </div>
            </div>
            <div class="addCommentComponent">
                <add-comment-component :currentUserImageUrl="currentUserImageUrl" :questionId="questionId" :comments-loaded="commentsLoaded" />
            </div>
        </div>
    </div>
</div>