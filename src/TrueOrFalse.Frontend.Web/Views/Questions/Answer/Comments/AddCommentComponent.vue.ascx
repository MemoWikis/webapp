<div id="AddCommentComponent">
    <div v-if="isLoggedIn" class="commentAddContainer">
        <div class="panel-body">
            <div>
                <div class="commentAnswerAddTopSpace row">
                    <div class="col-xs-1">
                        <img class="addCommentUsrImg" :src="currentUserImageUrl">
                    </div>
                    <div >
                        <div id="AddCommentFormContainer"  class="inline-question-editor col-xs-11">
                        <div class="addCommentInputTitle">
                            <div class="overline-s no-line">Titel der Diskussion</div>
                            <input type="text" class="form-control commentEditor" v-model="commentTitle" placeholder="Gib bitte den Titel der Diskussion ein." />
                            <div v-if="highlightEmptyTitle" class="field-error">Der Diskussionstitel muss mindestens 5 Zeichen lang sein.</div>
                        </div>
                        <div class="input-container">
                            <div class="overline-s no-line">Dein Diskussionsbeitrag</div>
                            <template v-if="commentEditor">
                                <editor-menu-bar-component :editor="commentEditor"/>
                            </template>
                            <template>
                                <editor-content :editor="commentEditor" :class="{ 'is-empty': highlightEmptyComment && commentEditor.state.doc.textContent.length <= 10 }"/>
                            </template>
                            <div v-if="highlightEmptyComment" class="field-error">Der Diskussionsbeitrag muss mindestens 10 Zeichen lang sein.</div>
                        </div>
                        </div>
                    </div>
                    <div style="padding-top: 18px;">
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