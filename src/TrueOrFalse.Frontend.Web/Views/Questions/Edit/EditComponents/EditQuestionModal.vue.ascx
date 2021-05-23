<%@ Control Language="C#" AutoEventWireup="true"%>

<div id="EditQuestionModal" class="modal fade">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
           
            <edit-question-component inline-template>
                <div id="AddModalQuestionContainer">
            
                    <div id="AddQuestionHeader" class="">
                        <div class="add-inline-question-label main-label">
                            <template v-if="edit">Frage bearbeiten</template>
                            <template v-else>Frage hinzufügen</template>
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
                                <template v-if="solutionType == 1">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Text/TextComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 3">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/MultipleChoice_SingleSolution/MultipleChoice_SingleSolutionComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 4">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Numeric/NumericComponent.vue.ascx") %>
                                </template>
<%--                                <template v-if="solutionType == 5">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Sequence/SequenceComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 6">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Date/DateComponent.vue.ascx") %>
                                </template>--%>
                                <template v-if="solutionType == 7">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/MultipleChoice/MultipleChoiceComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 8">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/MatchList/MatchListComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 9">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/FlashCard/FlashCardComponent.vue.ascx") %>
                                </template>

                            </div>

                        </div>
                        <div id="AddQuestionPrivacyContainer">
                            <div class="add-inline-question-label s-label">                
                                Sichtbarkeit
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="publicQuestionRadio" id="publicQuestionRadio" value="0" v-model="visibility">
                                <label class="form-check-label" for="publicQuestionRadio">
                                    Öffentliche Frage
                                    <i class="fa fa-question-circle show-tooltip" title="" data-placement="<%= CssJs.TooltipPlacementLabel %>" 
                                       data-html="true"
                                       data-original-title="
                                                                <ul class='show-tooltip-ul'>
                                                                    <li>Die Frage ist für alle auffindbar.</li>
                                                                    <li>Jeder kann die Frage in sein Wunschwissen aufnehmen.</li>
                                                                    <li>Die Frage steht unter einer Creative-Commons-Lizenz und kann frei weiterverwendet werden.</li>
                                                                </ul>">
                                    </i>
                                </label>
            
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="privateQuestionRadio" id="privateQuestionRadio" value="1" v-model="visibility">
                                <label class="form-check-label" for="privateQuestionRadio">
                                    Private Frage                                             
                                    <i class="fa fa-question-circle show-tooltip tooltip-min-200" title="" data-placement="top" 
                                       data-html="true"
                                       data-original-title="
                                                                <ul class='show-tooltip-ul'>
                                                                    <li>Die Frage kann nur von dir genutzt werden.</li>
                                                                    <li>Niemand sonst kann die Frage sehen oder nutzen.</li>
                                                                </ul>">
                                    </i>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            
            </edit-question-component>

        </div>
    </div>
</div>