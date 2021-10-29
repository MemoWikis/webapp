<div class="panel panel-default commentAnswerAddContainer" id="CommentAnswerAddComponent">
    <div class="panel-body">
        <div>
            <div class="col-xs-3"></div>
                <div class="col-xs-9">
                    <div id="AddCommentFormContainer" class="input-container">
                        <div class="overline-s no-line">Deine Antwort</div>
                        <template v-if="answerEditor">
                            <editor-menu-bar-component :editor="answerEditor"/>
                        </template>
                        <template>
                            <editor-content :editor="answerEditor" :class="{ 'is-empty': highlightEmptyAnswer && answerEditor.state.doc.textContent.length <= 10 }"/>
                        </template>
                        <div v-if="highlightEmptyAnswer" class="field-error">Deine Antwort muss mindestens 10 Zeichen lang sein.</div>
                    </div>
                </div>
            </div>
        </div>
    </div>