<div id="AddCommentComponent">
    <div v-if="isLoggedIn" class="panel panel-default commentAddContainer">
        <div class="panel-body">
            <div class="commentAnswerAddTopBorder">
                <div class="panel-body commentAnswerAddTopSpace">
                    <div class="col-xs-1">
                        <img class="addCommentUsrImg" :src="currentUserImageUrl">
                    </div>
                    <div class="col-xs-11">
<%--                        <div class="addCommentTitle">Neue Diskussion hinzufügen</div>
                        <div class="addCommentInputTitle">
                            <input type="text" class="form-control" v-model="commentTitle" placeholder="Gib bitte den Titel der Diskussion ein." />
                        </div>
                        <div>
                            <textarea class="commentAnswerAddTextArea form-control" v-model="commentText" placeholder="Beschreibe hier dein Anliegen. Bitte höflich, freundlich und sachlich schreiben."></textarea>
                        </div>--%>
                    <div id="AddCommentFormContainer"  class="inline-question-editor">
                        <div class="addCommentInputTitle">
                            <div class="overline-s no-line">Titel</div>
                            <input type="text" class="form-control" v-model="commentTitle" placeholder="Gib bitte den Titel der Diskussion ein." />
                        </div>
                        <div>
                            <div class="overline-s no-line">Diskussion</div>
                            <template v-if="commentEditor">
                                <editor-menu-bar-component :editor="commentEditor"/>
                            </template>
                            <template>
                                <editor-content :editor="commentEditor" :class="{ 'is-empty': highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0 }"/>
                            </template>
                            <div v-if="highlightEmptyFields && commentEditor.state.doc.textContent.length <= 0" class="field-error">Bitte formuliere eine Frage.</div>
                        </div>
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