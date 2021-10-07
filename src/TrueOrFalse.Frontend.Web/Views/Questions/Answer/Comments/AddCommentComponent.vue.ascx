<div id="AddCommentComponent">
    <div v-if="isLoggedIn" class="panel panel-default commentAddContainer">
        <div class="panel-body">
            <div class="commentAnswerAddTopBorder">
                <div class="panel-body commentAnswerAddTopSpace">
                    <div class="col-xs-1">
                        <img class="addCommentUsrImg" :src="currentUserImageUrl">
                    </div>
                    <div class="col-xs-11">
                        <i class="fa fa-spinner fa-spin hide2" v-if="!commentsLoaded" id="saveCommentSpinner"></i>
                        <div class="addCommentTitle">Neue Diskussion hinzufügen</div>
                        <div class="addCommentInputTitle">
                            <input type="text" class="form-control" v-model="commentTitle" placeholder="Gib bitte den Titel der Diskussion ein." />
                        </div>
                        <div>
                            <textarea class="commentAnswerAddTextArea form-control" v-model="commentText" placeholder="Beschreibe hier dein Anliegen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                        </div>
                    </div>

                    <div class="col-xs-12" style="padding-top: 18px;">
                        <a class="btn btn-primary memo-button pull-right " @click="saveComment()">Diskussion hinzufügen</a>
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