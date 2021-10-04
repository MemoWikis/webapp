<div class="panel-body commentRelativeContainer" id="CommentAnswerComponent">
    <div class="col-xs-2">
        <img class="pull-right answerUserImage" src="answer.creatorImageUrl">
    </div>
    <div class="col-xs-10 commentUserDetails">
        <div>
            <span>
                <a href="answer.creatorUrl">{{answer.CreatorName}}</a>
                <span class="commentUserDetails">vor {{answer.CreationDateNiceText}}</span>
            </span>
            <span v-if="answer.isSettled">
                <br />
                <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
            </span>
        </div>
        <div class="answerTextContainer">
            {{answer.Text}}    
        </div>
    </div>
</div>
