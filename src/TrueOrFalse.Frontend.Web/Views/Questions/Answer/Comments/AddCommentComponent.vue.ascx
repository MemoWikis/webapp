<div id="AddCommentComponent">
    <div v-if="isLoggedIn" class="commentAddContainer">
        <div class="panel-body">
            <div>
                <div class="commentAnswerAddTopSpace">
                    <div class="col-xs-1">
                        <img class="addCommentUsrImg" :src="currentUserImageUrl">
                    </div>
                    <div class="col-xs-11">
                        <div id="AddCommentFormContainer"  class="inline-question-editor">
                        <div class="addCommentInputTitle">
                            <div class="overline-s no-line">Titel der Diskussion</div>
                            <input type="text" class="form-control commentEditor" v-model="commentTitle" placeholder="Gib bitte den Titel der Diskussion ein." />
                        </div>
                        <div class="inline-question-editor">
                            <div class="overline-s no-line">Dein Diskussionsbeitrag</div>
                            <template v-if="commentEditor">
                                <editor-menu-bar-component :editor="commentEditor"/>
                            </template>
                            <template>
                                <editor-content :editor="commentEditor" :class="{ 'is-empty': highlightEmptyFields && commentEditor.state.doc.textContent.length <= 0 }" class="commentEditor"/>
                            </template>
                            <div v-if="highlightEmptyFields && commentEditor.state.doc.textContent.length <= 0" class="field-error">Beschreibe hier dein Anliegen. Bitte höflich, freundlich und sachlich schreiben.</div>
                        </div>
                    </div>
                    </div>
                    <div class="col-xs-12" style="padding-top: 18px;">
                        <a class="btn btn-primary memo-button pull-right " @click="saveComment()">Diskussion hinzufügen</a>
                        <a class="btn btn-secondary memo-button pull-right " @click="cancel()">Abbrechen</a>
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