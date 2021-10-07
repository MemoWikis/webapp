    <div class="commentPanel" id="CommentComponent">
        <div class="panel-heading" v-if="comment.IsSettled">
                <br/>
                <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Diese Diskussion wurde als erledigt markiert.</span>
        </div>
        <div class="panel-body">
            <div class="col-xs-12">
                <div>
                <span class="commentTitle" v-if="comment.Title.length > 0">{{comment.Title}} &nbsp</span> 
                    <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                        <i class="fa fa-comments-o commentAnswersCount" aria-hidden="true"></i> &nbsp {{comment.Answers.length}} 
                        <span v-if="comment.Answers.length == 1">Beitrag</span>
                        <span v-else>Beiträge</span>
                    </span>
                </div>
                <div class="commentTextContainer">
                    <p class="commentText" v-if="comment.Text.length < 50">{{comment.Text}}</p>
                    <span v-else>
                    <span v-if="readMore"><p class="">{{comment.Text}}</p>
                        <a class="cursor-hand" @click="readMore=false">
                            ...Weniger
                        </a>
                    </span>
                    <span v-else><p class="commentText">{{comment.Text.slice(0,50)}}...</p>
                    <a class="cursor-hand" @click="readMore=true">
                        ...Mehr
                    </a>
                    </span>
                </span>
                </div>
                <div class="commentUserDetails">
                    <img class="commentUserImg" :src="comment.ImageUrl">
                    <a href="comment.creatorUrl" class="commentUserName">{{comment.CreatorName}}</a>
                    <span class="greyed commentDate">
                        vor <span class="cursor-hand show-tooltip" >{{comment.CreationDateNiceText}}</span>
                    </span>
                </div>
            </div>
        </div>
        <div class="panel-body commentRelativeContainer">


                <div class="commentMarkAsSettledContainer" v-if="isLoggedIn">
                    <a @click="showAnsweringPanel = true; showCommentAnswers = true" class="btn btn-primary memo-button pull-right" >Antworten</a>

                    <a v-if="isInstallationAdmin && !settled || isOwner && !settled" @click="markAsSettled(comment.Id)" href="#" class="btn btn-secondary memo-button pull-right" data-comment-id="comment.Id">
                        Diskussion schliessen
                    </a>
                    <a v-if="isInstallationAdmin && settled" @click="markAsUnsettled(comment.Id)" href="#" class="btn btn-secondary memo-button pull-right" data-comment-id="comment.Id">
                        Diskussion wiedereröffnen
                    </a>
                </div>
        </div>
        <div class="commentAnswersContainer">
            <div v-if="showCommentAnswers" class="" v-for="(answer, index) in comment.Answers">
                <comment-answer-component :answer="answer" :comment-id="comment.Id" :last-answer="comment.Answers.length -1 == index"/>
            </div>
        </div>
        <div v-if="showAnsweringPanel && isLoggedIn" >
            <comment-answer-add-component :currentUserImageUrl="currentUserImageUrl" :parentCommentId="comment.Id"/>
        </div>
    </div>
