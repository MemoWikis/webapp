<div class="commentAnswerAddContainer" id="CommentAnswerAddComponent">
    <div class="panel-body">
        <div>
            <div class="col-xs-2"></div>
                <div class="col-xs-10">
                    <div id="AddAnswerTextFormContainer"  class="inline-question-editor col-xs-11">
                        <div class="input-container">
                            <div class="overline-s no-line">Dein Antwortsbeitrag</div>
                            <template v-if="answerEditor">
                                <editor-menu-bar-component :editor="answerEditor"/>
                            </template>
                            <template>
                                <editor-content :editor="answerEditor" :class="{ 'is-empty': highlightEmptyAnswer && answerEditor.state.doc.textContent.length <= 10 }"/>
                            </template>
                            <div v-if="highlightEmptyAnswer" class="field-error">Bitte formuliere einen Beitrag</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>