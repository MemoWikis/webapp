    <div class="commentPanel" id="CommentComponent">
        <div class="panel-heading" v-if="comment.IsSettled">
                <br/>
                <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
        </div>
        <div class="panel-body">
            <div class="col-xs-2">
                <img class="commentUserImg" :src="comment.ImageUrl">
            </div>
            <div class="col-xs-10">
                <div class="commentUserDetails">
                    <a href="comment.creatorUrl" class="commentUserName">{{comment.CreatorName}}</a>
                    <span class="greyed commentDate">
                        vor <span class="cursor-hand show-tooltip" >{{comment.CreationDateNiceText}}</span>
                    </span>
                </div>

                    <div class='ReasonList' v-if="comment.shouldBeImproved">
                        Ich bitte darum, dass diese Frage verbessert wird, weil:
                        <ul class="fa-ul commentModalImproveText" v-for="shouldReason in comment.ShouldReasons">

                                <li>
                                    <i class="fa-li fa fa-repeat commentShouldReasonImprove"></i>{{ShouldReason}}
                                </li>
                        </ul>
                    </div>

                    <div class="ReasonList" v-if="false">
                        Ich bitte darum, dass diese Frage gelöscht wird, weil:
                        <ul class="fa-ul commentShouldReasonDelete" v-for="shouldReason in comment.ShouldReasons">
                            <li >
                                    <i class="fa-li fa fa-fire commentImproveFire"></i>{{ShouldReason}}
                                </li>
                        </ul>
                    </div>
                    <h3>{{comment.Title}}</h3>
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
        </div>

            <div class="panel-body commentSettledInfo settledCommentsDescription" v-if="comment.showSettledAnswers && comment.answersSettledCount > 0">
                Dieser Kommentar hat {{comment.AnswersSettledCount}}
                <span v-if="comment.settledAnswersCount != 1"> weitere als erledigt markierte Antworten
                </span>
                <span v-if="comment.settledAnswersCount == 1"> weiteren als erledigt markiert Kommentar
                </span>
            </div>
        <div class="panel-body commentRelativeContainer">
            <div class="commentIconsContainer">
                <a v-if="false" class="commentThumbsIcon">
                    <i class="fa fa-thumbs-up" aria-hidden="true"></i> &nbsp 0
                </a>
                <span class="commentSpeechBubbleIcon" @click="showCommentAnswers = !showCommentAnswers">
                <i class="fa fa-comments-o commentAnswersCount" aria-hidden="true"></i> &nbsp {{comment.Answers.length}}
                </span>
            </div>

                <div class="commentMarkAsSettledContainer" v-if="isLoggedIn">

                    <a v-if="isInstallationAdmin && !settled || isOwner && !settled" @click="markAsSettled(comment.Id)" href="#" class="btnAnswerComment btn btn-link commentMarkAsSettled commentFooterText" data-comment-id="comment.Id">
                        <i class="fa fa-check" aria-hidden="true"></i>
                        Als Erledigt Markieren
                    </a>
                    <a v-if="isInstallationAdmin && settled || isOwner && settled" @click="markAsUnsettled(comment.Id)" href="#" class="btnAnswerComment btn btn-link commentMarkAsSettled commentFooterText" data-comment-id="comment.Id">
                        <i class="fa fa-check" aria-hidden="true"></i>
                        Als nicht Erledigt Markieren
                    </a>
                    <a @click="showAnsweringPanel = true; showCommentAnswers = true" class="btnAnswerComment btn btn-link commentFooterText" >Antworten</a>
                </div>
        </div>
        <div v-if="showCommentAnswers" class="commentAnswers" v-for="answer in comment.Answers">

            <comment-answer-component :answer="answer" :commentId="comment.id"/>

            <div v-for="answer in addedAnswers">
                <div v-html="answer"></div>
            </div>
        </div>

        <div v-if="showAnsweringPanel && isLoggedIn" >
            <comment-answer-add-component :currentUserImageUrl="currentUserImageUrl" :parentCommentId="comment.Id"/>
        </div>
    </div>
