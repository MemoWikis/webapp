<div id="AddCommentComponent">
    <div v-if="isLoggedIn" class="panel panel-default commentAddContainer">
        <div class="panel-body">
            <div class="commentAnswerAddTopBorder">
                <div class="panel-body commentAnswerAddTopSpace">
                    <div class="col-xs-2">
                        <img class="commentUserImg" :src="currentUserImageUrl">
                    </div>
                    <div class="col-xs-10">
                        <i class="fa fa-spinner fa-spin hide2" v-if="!commentsLoaded" id="saveCommentSpinner"></i>
                        <textarea class="commentAnswerAddTextArea form-control" v-model="commentText" placeholder="Neue Diskussion starten. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                    </div>

                    <div class="col-xs-12" style="padding-top: 18px;">
                        <a class="btn btn-secondary memo-button pull-right commentAnswerAddSaveBtn" @click="saveComment()">Speichern</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-else class="row commentLoginContainer">
        <div class="col-xs-12 commentLoginText">
            Um zu kommentieren, musst du eingeloggt sein. <a href="#" data-btn-login="true" @click="closeModal()">Jetzt anmelden</a>
        </div>
    </div>
</div>