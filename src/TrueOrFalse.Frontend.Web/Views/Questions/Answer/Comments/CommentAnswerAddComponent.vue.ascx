    <div class="panel panel-default commentAnswerAddContainer" id="CommentAnswerAddComponent">
        <div class="panel-body">
            <div class="commentAnswerAddTopBorder">
                <div class="panel-body commentAnswerAddTopSpace">
                    <div class="col-xs-2">
                        <img class="commentUserImg" src="{{currentUserImageUrl}}">
                    </div>
                    <div class="col-xs-10">
                        <textarea class="commentAnswerAddTextArea form-control" v-model="commentAnswerText" placeholder="Neue Antwort hinzufügen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                    </div>

                    <div class="col-xs-12 commentAnswerAddSaveSpace">
                        <a class="btn btn-secondary memo-button pull-right saveAnswer commentAnswerAddSaveBtn" @click="saveCommentAnswer(); commentAnswerText=''">Speichern</a>
                    </div>
                </div>
            </div>
        </div>
    </div>