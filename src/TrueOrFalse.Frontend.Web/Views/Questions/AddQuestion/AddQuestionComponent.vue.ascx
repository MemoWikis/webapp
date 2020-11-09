
<add-question-component inline-template>
    <div id="AddInlineQuestionContainer">

        <div id="AddQuestionHeader" class="">
            <div class="add-inline-question-label main-label">
                Frage hinzufügen 
                <span>(Karteikarte)</span>
                <a>erweiterte Optionen</a>
            </div>
            <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi">
                <div>
                    <i class="fa fa-heart" :class="" v-if="addToWuwi"></i>
                    <i class="fa fa-heart-o" :class="" v-else></i>
                </div>
                <div>
                    <span v-if="addToWuwi">Hinzugefügt</span>
                    <span v-else class="wuwi-grey">Hinzufügen</span>
                </div>
            </div>
        </div>

        <div id="AddQuestionBody">
            <div id="AddQuestionFormContainer"  class="inline-question-editor">
                <div>
                    <div class="add-inline-question-label s-label">Frage</div>
                    <editor-menu-bar :editor="questionEditor" v-slot="{ commands, isActive, focused }">
                        <div class="menubar is-hidden" :class="{ 'is-focused': focused }">
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.bold() }" @click="commands.bold">
                            <i class="fas fa-bold"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.italic() }" @click="commands.italic" >
                            <i class="fas fa-italic"></i>
                        </button>
                        
                        <button class="menubar__button":class="{ 'is-active': isActive.strike() }"@click="commands.strike">
                            <i class="fas fa-strikethrough"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.underline() }" @click="commands.underline">
                            <i class="fas fa-underline"></i>
                        </button>
                          
                        <button class="menubar__button" :class="{ 'is-active': isActive.paragraph() }" @click="commands.paragraph">
                            <i class="fas fa-paragraph"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.bullet_list() }" @click="commands.bullet_list">
                          <i class="fas fa-list-ul"></i>
                        </button>
                        
                         <button class="menubar__button" :class="{ 'is-active': isActive.ordered_list() }" @click="commands.ordered_list" >
                          <i class="fas fa-list-ol"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.blockquote() }" @click="commands.blockquote" >
                          <i class="fas fa-quote-right"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.code() }" @click="commands.code" >
                            <i class="far fa-file-code"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.code_block() }" @click="commands.code_block" >
                          <i class="fas fa-file-code"></i>
                        </button>

                        <button class="menubar__button" @click="commands.undo" >
                            <i class="fas fa-undo-alt"></i>
                        </button>
                        
                        <button class="menubar__button" @click="commands.redo" >
                            <i class="fas fa-redo-alt"></i>
                        </button>
                        
                    </div>
                    </editor-menu-bar>
                    
                    <editor-content :editor="questionEditor" />
                </div>
                <div>
                    <div class="add-inline-question-label s-label">Antwort</div>
                    <editor-menu-bar :editor="answerEditor" v-slot="{ commands, isActive, focused }">
                        <div class="menubar is-hidden" :class="{ 'is-focused': focused }">
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.bold() }" @click="commands.bold">
                            <i class="fas fa-bold"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.italic() }" @click="commands.italic" >
                            <i class="fas fa-italic"></i>
                        </button>
                        
                        <button class="menubar__button":class="{ 'is-active': isActive.strike() }"@click="commands.strike">
                            <i class="fas fa-strikethrough"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.underline() }" @click="commands.underline">
                            <i class="fas fa-underline"></i>
                        </button>
                          
                        <button class="menubar__button" :class="{ 'is-active': isActive.paragraph() }" @click="commands.paragraph">
                            <i class="fas fa-paragraph"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.bullet_list() }" @click="commands.bullet_list">
                          <i class="fas fa-list-ul"></i>
                        </button>
                        
                         <button class="menubar__button" :class="{ 'is-active': isActive.ordered_list() }" @click="commands.ordered_list" >
                          <i class="fas fa-list-ol"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.blockquote() }" @click="commands.blockquote" >
                          <i class="fas fa-quote-right"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.code() }" @click="commands.code" >
                            <i class="far fa-file-code"></i>
                        </button>
                        
                        <button class="menubar__button" :class="{ 'is-active': isActive.code_block() }" @click="commands.code_block" >
                          <i class="fas fa-file-code"></i>
                        </button>

                        <button class="menubar__button" @click="commands.undo" >
                            <i class="fas fa-undo-alt"></i>
                        </button>
                        
                        <button class="menubar__button" @click="commands.redo" >
                            <i class="fas fa-redo-alt"></i>
                        </button>
                        
                    </div>
                    </editor-menu-bar>
                    <editor-content :editor="answerEditor" />
                </div>
                <div>
                </div>
            </div>
            <div id="AddQuestionPrivacyContainer">
                <div class="add-inline-question-label s-label">                
                    Sichtbarkeit <i class="fas fa-question-circle"></i>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="radio" name="publicQuestionRadio" id="publicQuestionRadio" value="0" v-model="visibility">
                    <label class="form-check-label" for="publicQuestionRadio">
                        Öffentliche Frage
                    </label>
                </div>
                <div class="form-check">
                    <input class="form-check-input" type="radio" name="privateQuestionRadio" id="privateQuestionRadio" value="1" v-model="visibility">
                    <label class="form-check-label" for="privateQuestionRadio">
                        Private Frage <i class="fas fa-eye-slash"></i>
                    </label>
                </div>
            </div>
        </div>
    </div>

</add-question-component>
