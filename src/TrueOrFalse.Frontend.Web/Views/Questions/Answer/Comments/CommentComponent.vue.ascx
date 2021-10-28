    <div v-bind:class="{commentPanelSpace: !comment.IsSettled}" class="commentPanel" id="CommentComponent">
        <div class="panel-body">
            <div class="col-xs-12">
                <%--UnsettledComment--%>
                <div v-if="!comment.IsSettled">
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
                </div>
                <%--SettledComment--%>
                <div v-else class="pointer" @click="foldOut = !foldOut">
                    <div class="pull-right">
                        <span class="commentSettledBox">geschlossen</span>
                        <span>
                            <i class="fa fa-chevron-down pointer" v-if="!foldOut"></i>
                            <i class="fa fa-chevron-up pointer" v-else></i>
                        </span>
                    </div>
                    <div class="commentTitle" v-if="comment.Title.length > 0">{{comment.Title}} &nbsp &nbsp
                        <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                            <i class="fa fa-comments commentAnswersCount" aria-hidden="true"></i>
                            <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else-if="comment.Text.length > 25">{{comment.Text.slice(0,25)}}... &nbsp &nbsp
                                <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                                    <i class="fa fa-comments commentAnswersCount" aria-hidden="true"></i>
                                    <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                                    <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                                </span>
                            </div>
                            <div class="commentTitle" v-else>{{comment.Text}} &nbsp &nbsp
                                <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                                    <i class="fa fa-comments commentAnswersCount" aria-hidden="true"></i>  
                                    <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                                    <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                                </span>
                            </div>

                </div>

                <div v-if="foldOut || !comment.IsSettled">
                <div class="commentTextContainer" >
                    <span class="commentText" v-if="comment.Text.length < 350" v-html="comment.Text"></span>
                    <span v-else>
                    <span v-if="readMore"><span v-html="comment.Text"></span>
                        <a class="cursor-hand" @click="readMore=false">
                             Weniger
                        </a>
                    </span>
                    <span v-else>
                            <span class="commentText" v-html="comment.Text.slice(0,350) + '...'">

                        </span>
                        <a class="cursor-hand" @click="readMore=true">Mehr
                        </a>
                    </span>
                </span>
                </div>
                <div class="commentUserDetails col-xs-6">
                    <a class="pointer" :href="comment.CreatorUrl">
                    <img class="commentUserImg" :src="comment.ImageUrl">
                    <a class="commentUserName">{{comment.CreatorName}}</a>
                    </a>
                    <span class="greyed commentDate">
                        vor <span class="cursor-hand show-tooltip" >{{comment.CreationDateNiceText}}</span>
                    </span>
                </div>
                </div>
               
            </div>
        </div>

        <div class="commentAnswersContainer" v-if="foldOut && comment.Answers.length > 0 || !comment.IsSettled">
            <div v-if="showCommentAnswers" class="" v-for="(answer, index) in comment.Answers">
                <comment-answer-component :answer="answer" :comment-id="comment.Id" :last-answer="comment.Answers.length -1 == index"/>
            </div>
            <div v-if="isLoggedIn && !comment.IsSettled" >
                <comment-answer-add-component :currentUserImageUrl="currentUserImageUrl" :parentCommentId="comment.Id" :currentUserName="currentUserName"/>
            </div>
            
            <div class="panel-body commentButtonsContainer" v-if="!comment.IsSettled">
                <div class="commentMarkAsSettledContainer" v-if="isLoggedIn">
                    <a v-if="showAnsweringPanel" @click="emitSaveAnswer()" class="btn btn-primary memo-button pull-right">Antworten</a>
                    <a v-else @click="showAnsweringPanel = true; showCommentAnswers = true" class="btn btn-primary memo-button pull-right" >Antworten</a>

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
