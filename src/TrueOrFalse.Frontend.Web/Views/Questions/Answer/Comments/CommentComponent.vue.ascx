    <div v-bind:class="{commentPanelSpace: !comment.IsSettled}" class="commentPanel" id="CommentComponent">
        <div class="panel-body">
            <div class="col-xs-12">
                <%--UnsettledComment--%>
                <div v-if="!comment.IsSettled">
                <span class="commentTitle" v-if="comment.Title.length > 0" v-html="comment.Title + '&nbsp &nbsp'"></span>
                    <span v-else>
                        <span class="commentTitle" v-if="comment.Text.length > 25" v-html="comment.Text.slice(0,25) + '...' + '&nbsp &nbsp'"></span>
                        <span class="commentTitle" v-else v-html="comment.Text + '&nbsp &nbsp'"></span>
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
                    <div class="commentTitle" v-if="comment.Title.length > 0" v-html="comment.Title">
                        <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                            <i class="fa fa-comments commentAnswersCount" aria-hidden="true"></i>
                            <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                            <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                        </span>
                    </div>
                    <div class="commentTitle" v-else-if="comment.Text.length > 25" v-html="comment.Text.slice(0,25) + '...'">
                                <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                                    <i class="fa fa-comments commentAnswersCount" aria-hidden="true"></i>
                                    <span class="commentSpeechBubbleText" v-if="comment.Answers.length == 1">&nbsp {{comment.Answers.length}} Beitrag</span>
                                    <span class="commentSpeechBubbleText" v-else>&nbsp {{comment.Answers.length}} Beiträge</span>
                                </span>
                    </div>
                    <div class="commentTitle" v-else v-html="comment.Text">
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
                    <span v-if="readMore">
                        <span ref="readLess" v-html="comment.Text + readLessHtml"></span>
                    </span>
                    <span v-else>
                            <span class="commentText" ref="readMore" v-html="comment.Text.slice(0,360) + '...' + readMoreHtml">

                        </span>

                    </span>
                </span>
                </div>
                <div class="commentUserDetails">
                    <a class="pointer" :href="comment.CreatorUrl">
                    <img class="commentUserImg" :src="comment.ImageUrl">
                    <a class="commentUserName">{{comment.CreatorName}}</a>
                    </a>
                    <span class="greyed commentDate">
                        vor <span class="show-tooltip" >{{comment.CreationDateNiceText}}</span>
                    </span>
                </div>
                </div>
               
            </div>
        </div>

        <div class="commentAnswersContainer" v-if="foldOut && isInstallationAdmin|| foldOut && comment.Answers.length > 0 || !comment.IsSettled  && isInstallationAdmin|| !comment.IsSettled && comment.Answers.length > 0 || !comment.IsSettled && isLoggedIn">
            <div v-if="showCommentAnswers" class="" v-for="(answer, index) in comment.Answers">
                <comment-answer-component :answer="answer" :comment-id="comment.Id" :last-answer="comment.Answers.length -1 == index"/>
            </div>
            <div v-if="isLoggedIn && !comment.IsSettled" >
                <comment-answer-add-component :currentUserImageUrl="currentUserImageUrl" :parentCommentId="comment.Id" :currentUserName="currentUserName"/>
            </div>
            
            <div class="commentButtonsContainer" style="display: flex; flex-direction: row-reverse; justify-content: end;" v-if="isLoggedIn">
                <div v-if="!comment.IsSettled" >
                    <a @click="emitSaveAnswer()" class="btn btn-primary memo-button pull-right">Antworten</a>
                </div>
                <div>
                    <a v-if="isInstallationAdmin && !comment.IsSettled || isOwner && !comment.IsSettled" @click="markAsSettled(comment.Id)" href="#" class="btn btn-lg btn-link memo-button pull-right" data-comment-id="comment.Id">
                        Diskussion schliessen
                    </a>
                    <a v-if="isInstallationAdmin && comment.IsSettled" @click.stop="markAsUnsettled(comment.Id)" href="#" class="btn btn-lg btn-link memo-button pull-right" data-comment-id="comment.Id">
                        Diskussion wieder eröffnen
                    </a>
                </div>
            </div>
        </div>

    </div>
