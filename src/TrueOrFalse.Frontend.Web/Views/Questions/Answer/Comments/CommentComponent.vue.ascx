    <div class="commentPanel" id="CommentComponent">
        <div class="panel-body">
            <div class="col-xs-12">
                <%--UnsettledComment--%>
                <div v-if="!comment.IsSettled"  @click="showCommentAnswers = !showCommentAnswers" class="pointer">
                <span class="commentTitle" v-if="comment.Title.length > 0">{{comment.Title}} &nbsp &nbsp</span>
                    <span v-else>
                        <span class="commentTitle" v-if="comment.Text.length > 25">{{comment.Text.slice(0,25)}}... &nbsp &nbsp</span>
                        <span class="commentTitle" v-else>{{comment.Text}} &nbsp &nbsp</span>
                    </span>

                    <span class="commentSpeechBubbleIcon">
                        <i class="fa fa-comments commentAnswersCount" aria-hidden="true"></i> 
                        <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                        <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                    </span>
                    <div class="pull-right">
                        <i class="fa fa-chevron-down pointer" v-if="!showCommentAnswers"></i>
                        <i class="fa fa-chevron-up pointer" v-else></i>
                    </div>
                </div>
                <%--SettledComment--%>
                <div v-else class="pointer" @click="foldOut = !foldOut">
                    <div class="commentTitle" v-if="comment.Title.length > 0">{{comment.Title}} &nbsp &nbsp
                        <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                            <i class="fa fa-comments-o commentAnswersCount" aria-hidden="true"></i>
                            <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                        </span>
                    </div>
                            <div class="commentTitle" v-else-if="comment.Text.length > 25">{{comment.Text.slice(0,25)}}... &nbsp &nbsp
                                <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                                    <i class="fa fa-comments-o commentAnswersCount" aria-hidden="true"></i>
                                    <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                                    <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                                </span>
                            </div>
                            <div class="commentTitle" v-else>{{comment.Text}} &nbsp &nbsp
                                <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                                    <i class="fa fa-comments-o commentAnswersCount" aria-hidden="true"></i>  
                                    <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                                    <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                                </span>
                            </div>

                        <div class="pull-right settledCommentTag">
                        <span class="commentSettledBox">geschlossen</span>
                        <i class="fa fa-chevron-down pointer" v-if="!foldOut"></i>
                        <i class="fa fa-chevron-up pointer" v-else></i>
                    </div>
                </div>
                <div v-if="foldOut">
                <div class="commentTextContainer">
                    <span class="commentText" v-if="comment.Text.length < 350" v-html="comment.Text"></span>
                    <span v-else>
                    <span v-if="readMore"><span v-html="comment.Text"></span>
                        <a class="cursor-hand" @click="readMore=false">
                            ...Weniger
                        </a>
                    </span>
                    <span v-else>
                            <span class="commentText" v-html="comment.Text.slice(0,350)">

                        </span>
                        <a class="cursor-hand" @click="readMore=true">
                            ...Mehr
                        </a>
                    </span>
                </span>
                </div>
                <div class="commentUserDetails col-xs-6">
                    <img class="commentUserImg" :src="comment.ImageUrl">
                    <a href="comment.creatorUrl" class="commentUserName">{{comment.CreatorName}}</a>
                    <span class="greyed commentDate">
                        vor <span class="cursor-hand show-tooltip" >{{comment.CreationDateNiceText}}</span>
                    </span>
                </div>
                </div>
                <div class="panel-body commentButtonsContainer col-xs-6" v-if="!comment.IsSettled">
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
            </div>
        </div>

        <div class="commentAnswersContainer">
            <div v-if="showCommentAnswers" class="" v-for="(answer, index) in comment.Answers">
                <comment-answer-component :answer="answer" :comment-id="comment.Id" :last-answer="comment.Answers.length -1 == index"/>
            </div>
            <div v-if="showAnsweringPanel && isLoggedIn && !comment.IsSettled" >
                <comment-answer-add-component :currentUserImageUrl="currentUserImageUrl" :parentCommentId="comment.Id" :currentUserName="currentUserName"/>
            </div>
        </div>

    </div>
