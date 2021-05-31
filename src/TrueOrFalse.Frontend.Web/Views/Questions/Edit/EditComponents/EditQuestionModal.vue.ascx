<%@ Control Language="C#" AutoEventWireup="true"%>
<% 
    var userSession = new SessionUser();
    var user = userSession.User;
%>

<div id="EditQuestionModal" class="modal fade">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
           
            <edit-question-component inline-template>
                <div id="AddModalQuestionContainer">
            
                    <div id="" class="">
                        <div class="add-inline-question-label main-label">
                            <template v-if="edit">Frage bearbeiten</template>
                            <template v-else>Frage hinzufügen</template>
                        </div>
                        <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi" v-if="!edit">
                            <div>
                                <i class="fa fa-heart" :class="" v-if="addToWuwi"></i>
                                <i class="fa fa-heart-o" :class="" v-else></i>
                            </div>
                            <div>
                                <span v-if="addToWuwi">Hinzugefügt</span>
                                <span v-else class="wuwi-grey">Hinzufügen</span>
                            </div>
                        </div>
                        <select v-if="!edit" v-model="solutionType">
                            <option value="1">Text</option>
                            <option value="4">Numeric</option>
<%--                            <option value="5">Sequenz</option>--%>
                            <option value="6">Datum</option>
                            <option value="3">Multiplechoice Single Solution</option>
                            <option value="7">MultipleChoice</option>
                            <option value="8">Paare</option>
                            <option value="9">Karteikarte</option>
                        </select>
                    </div>

                    <div id="">
                        <div id=""  class="inline-question-editor">
                            <div>
                                <div class="add-inline-question-label s-label">Frage</div>
                                <editor-menu-bar :editor="questionEditor" v-slot="{ commands, isActive, focused }">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/BasicEditorMenubar.vue.ascx") %>
                                </editor-menu-bar>
                                <editor-content :editor="questionEditor" />
                            </div>
                            <div>
                                <template v-if="solutionType == 1">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Text/TextSolutionComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 3">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/MultipleChoice_SingleSolution/MultipleChoice_SingleSolutionComponent.vue.ascx") %>
                                </template>
                                <template v-if="solutionType == 4">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Numeric/NumericComponent.vue.ascx") %>
                                </template>
<%--                                <template v-if="solutionType == 5">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Sequence/SequenceComponent.vue.ascx") %>
                                </template>--%>
                                <template v-if="solutionType == 6">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/Date/DateComponent.vue.ascx") %>
                                </template>
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
                            <div v-if="solutionType != 9">
                                <div class="add-inline-question-label s-label">Ergänzungen</div>

                                <editor-menu-bar :editor="descriptionEditor" v-slot="{ commands, isActive, focused }">
                                    <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/BasicEditorMenubar.vue.ascx") %>
                                </editor-menu-bar>
                                <editor-content :editor="descriptionEditor" />
                            </div>
                        </div>
                        
                        <div>
                            <form class="" v-on:submit.prevent>
                                <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open' : showDropdown}">
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
                            <template v-for="(category, index) in selectedCategories">
                                <%: Html.Partial("~/Views/Shared/CategoryChip/CategoryChipComponent.vue.ascx") %>
                            </template>
                        </div>
                        <div id="">
                            <div class="s-label">                
                                Sichtbarkeit
                            </div>
                            
                            <div class="col-sm-offset-2 col-sm-10">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="visibility" true-value="1" false-value="0"> Private Frage
                                    </label>
                                </div>
                            </div>
                        </div>
                        <% if (user.IsInstallationAdmin) {%>
                            <select v-model="licenseId">
                                <option value="0">Keine Lizenz</option>
                                <option value="1">CC BY 4.0</option>
                                <option value="2">Amtliches Werk BAMF</option>
                                <option value="3">ELWIS</option>
                                <option value="4">BLAC</option>
                            </select>
                        <%} %>

                        <div @click="save()" class="btn btn-primary">Speichern</div>

                    </div>

                </div>
            
            </edit-question-component>

        </div>
    </div>
</div>