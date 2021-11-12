<div id="AddCommentComponent">
    <div v-if="isLoggedIn" class="commentAddContainer">
        <div class="panel-body" style="padding-right: 10px;">
            <div>
                <div class="commentAnswerAddTopSpace row">
                    <div class="col-xs-1 addCommentUsrImgContainer">
                        <img class="addCommentUsrImg" :src="currentUserImageUrl">
                    </div>
                    <div >
                        <div class="inline-question-editor col-xs-11 noPadding">
                        
                            <div class="input-container" id="AddCommentTitleFormContainer">
                                <div class="overline-s no-line">Titel der Diskussion</div>
                                <template >
                                    <editor-content :editor="titleEditor"  :class="{ 'is-empty': highlightEmptyComment && commentEditor.state.doc.textContent.length <= 10 }"/>
                                </template>
                                <div v-if="highlightEmptyTitle" class="field-error">Bitte formuliere einen Titel</div>
                            </div>
                            <div class="input-container" id="AddCommentTextFormContainer">
                            <div class="overline-s no-line">Dein Diskussionsbeitrag</div>
                            <template v-if="commentEditor">
                                <editor-menu-bar-component :editor="commentEditor"/>
                            </template>
                            <template>
                                <editor-content :editor="commentEditor" :class="{ 'is-empty': highlightEmptyComment && commentEditor.state.doc.textContent.length <= 10 }"/>
                            </template>
                            <div v-if="highlightEmptyComment" class="field-error">Bitte formuliere einen Diskussionsbeitrag</div>
                        </div>
                        </div>
                    </div>
                    <div class="col-xs-1"></div>
                    <div class="col-xs-11 noPadding">
                        <a class="btn btn-primary memo-button pull-right" @click="saveComment()">Diskussion hinzufügen</a>
                        <a class="btn btn-lg btn-link memo-button pull-right " @click="closeModal()">Abbrechen</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-else class="row commentLoginContainer">
        <div class="col-xs-12 commentLoginText">
            Um zu kommentieren, musst du eingeloggt sein. &nbsp<a href="/Registrieren" @click="closeModal()" class="pointer">Jetzt registrieren</a>
        </div>
    </div>
</div>