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
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/BasicEditorMenubar.vue.ascx") %>
                                </editor-menu-bar>
                                <editor-content :editor="questionEditor" />
                            </div>
                            <div>
                                <%--                                <template v-if="solutionType == 1">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Text/TextComponent.vue.ascx") %>
                                </template>--%>
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
                            <div>
                                <editor-menu-bar :editor="descriptionEditor" v-slot="{ commands, isActive, focused }">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/BasicEditorMenubar.vue.ascx") %>
                                </editor-menu-bar>
                                <editor-content :editor="descriptionEditor" />
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
                    
                    <form v-on:submit.prevent>
                        <div class="form-group dropdown categorySearchAutocomplete"  :class="{ 'open' : showDropdown}">
                            <input ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="questionCategoriesList" autocomplete="off" @click="lockDropdown = false"  aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
                            <ul class="dropdown-menu" aria-labelledby="questionCategoriesList">
                                <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)" data-toggle="tooltip" data-placement="top" :title="c.Name">
                                    <img :src="c.ImageUrl"/>
                                    <div>
                                        <div class="searchResultLabel body-m">{{c.Name}}</div>
                                        <div class="searchResultQuestionCount body-s">{{c.QuestionCount}} Frage<template v-if="c.QuestionCount != 1">n</template></div>
                                    </div>
                                </li>
                                <li class="dropdownFooter body-m">
                                    <b>{{totalCount}}</b> Treffer. <br/>
                                    Deins ist nicht dabei? <span class="dropdownLink" @click="createCategory = true">Erstelle hier dein Thema</span>
                                </li>
                            </ul>
                        </div>
                    </form>
                    <template v-for="category in selectedCategories">
                        <%: Html.Partial("~/Views/Shared/CategoryChip/CategoryChipComponent.vue.ascx") %>
                    </template>

                    <div @click="save()" class="btn btn-primary">Speichern</div>

                </div>
            
            </edit-question-component>

        </div>
    </div>
</div>