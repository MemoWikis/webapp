<div class="panel-body commentRelativeContainer row" id="CommentAnswerComponent">
    <div class="col-xs-2"></div>
    <div class="col-xs-10 answerUserDetails" v-bind:class="{ commentUserDetails: lastAnswer }">
        <div>
            <a class="pointer" :href="answer.CreatorUrl">
                <img class="commentUserImg" :src="answer.ImageUrl">
                <a class="commentUserName" >{{answer.CreatorName}}</a>
            </a>
            <span class="commentDate">vor {{answer.CreationDateNiceText}}</span>
            <span v-if="answer.isSettled">
                <br />
                <span class="commentSettledInfo"><i class="fa fa-check">&nbsp;</i>Dieser Kommentar wurde als erledigt markiert.</span>
            </span>
        </div>
        <div class="answerTextContainer">
            <span class="commentText" v-if="answer.Text.length < 350" v-html="answer.Text"></span>
            <span v-else>
                <span v-if="readMore"><span v-html="answer.Text"></span>
                    <a class="cursor-hand" @click="readMore=false">
                        Weniger
                    </a>
                </span>
                <span v-else>
                    <span class="commentText" v-html="answer.Text.slice(0,350) + '...'">

                    </span>
                    <a class="cursor-hand" @click="readMore=true">Mehr
                    </a>
                </span>
            </span>        </div>
    </div>
</div>
