    <div class="panel panel-default commentAnswerAddContainer" id="CommentAnswerAddComponent">
        <div class="panel-body">
                <div class="col-xs-2"></div>
    <div class="col-xs-1">
        <img class="pull-right commentUserImg" :src="currentUserImageUrl">
    </div>
    <div class="col-xs-9 commentUserDetails">
        <div>
            <span>
                <a href="/">{{currentUserName}}</a>
            </span>
                    <div class="col-xs-12">
                        <textarea class="commentAnswerAddTextArea form-control" v-model="commentAnswerText" placeholder="Neuen Beitrag hinzufügen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                    </div>

                    <div class="col-xs-12 commentAnswerAddSaveSpace">
                        <a class="btn btn-secondary memo-button pull-right saveAnswer commentAnswerAddSaveBtn" @click="saveCommentAnswer(); commentAnswerText=''">Speichern</a>
                    </div>
                </div>
            </div>
        </div>
    </div>