<div class="panel-body commentRelativeContainer" id="CommentAnswerComponent">
    <div class="col-xs-2"></div>
    <div class="col-xs-1">
    </div>
    <div class="col-xs-9 commentUserDetails">
        <div>
            <span>
                <img class="commentUserImg" :src="answer.ImageUrl">
                <a :href="answer.ImageUrl">{{answer.CreatorName}}</a>
                <span class="commentAnswerCreationDate">vor {{answer.CreationDateNiceText}}</span>
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
    <div v-if="!lastAnswer" class="answerBorder"></div>
</div>
