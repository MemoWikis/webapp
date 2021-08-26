<%@ Control Language="C#" AutoEventWireup="true"%>
<%
    var userSession = new SessionUser();
    var user = userSession.User;
    var isAdmin = user != null && user.IsInstallationAdmin;
%>
    <div id="EditQuestionModal" class="modal fade">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="edit-question-modal-header overline-m overline-title">

                        <div class="main-header">
                            <div class="add-inline-question-label main-label">
                                <template v-if="edit">Frage bearbeiten</template>
                                <template v-else>Frage erstellen</template>
                                <i v-if="visibility == 1" class="fas fa-lock"></i>
                            </div>

                            <div class="solutionType-selector">
                                <select v-if="!edit" v-model="solutionType">
                                    <option value="1">Text</option>
                                    <option value="7">MultipleChoice</option>
                                    <option value="8">Zuordnung (Liste)</option>
                                    <option value="9">Karteikarte</option>
                                </select>
                            </div>
                        </div>

                        <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi" v-if="!edit">
                            <div>
                                <i class="fa fa-heart" :class="" v-if="addToWuwi"></i>
                                <i class="fa fa-heart-o" :class="" v-else></i>
                            </div>
                        </div>
                    </div>
                    <div id="" class="inline-question-editor">

                        <div class="input-container">
                            <div class="overline-s no-line">Frage</div>

                            <div v-if="questionEditor">
                                <template>
                                    <editor-menu-bar-component :editor="questionEditor"/>
                                </template>
                                <template>
                                    <editor-content :editor="questionEditor" :class="{ 'is-empty': highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0 }"/>
                                </template>
                                <div v-if="highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0" class="field-error">Bitte formuliere eine Frage.</div>
                            </div>
                        </div>

                        <div class="input-container" v-if="solutionType != 9">
                                <div class="overline-s no-line">Ergänzungen zur Frage</div>
                                <div v-if="showQuestionExtension && questionExtensionEditor">
                                    <template>
                                        <editor-menu-bar-component :editor="questionExtensionEditor"/>
                                    </template>
                                    <template>
                                        <editor-content :editor="questionExtensionEditor"/>
                                    </template>
                                </div>
                                <template v-else>
                                    <div class="d-flex">
                                        <div class="btn grey-bg form-control col-md-6" @click="showQuestionExtension = true">Ergänzungen hinzufügen</div> 
                                        <div class="col-sm-12 hidden-xs"></div>
                                    </div>
                                </template>
                        </div>

                        <textsolution-component v-if="solutionType == 1" :solution="textSolution" :highlight-empty-fields="highlightEmptyFields"/>
                        <multiplechoice-component v-if="solutionType == 7" :solution="multipleChoiceJson" :highlight-empty-fields="highlightEmptyFields"/>
                        <matchlist-component v-if="solutionType == 8" :solution="matchListJson" :highlight-empty-fields="highlightEmptyFields"/>
                        <flashcard-component v-if="solutionType == 9" :solution="flashCardJson" :highlight-empty-fields="highlightEmptyFields" />
                        
                        <div class="input-container description-container">
                            <div class="overline-s no-line">Ergänzungen zur Antwort</div>
                            <div v-if="showDescription && descriptionEditor">
                                <template>
                                    <editor-menu-bar-component :editor="descriptionEditor"/>
                                </template>
                                <template>
                                    <editor-content :editor="descriptionEditor"/>
                                </template>
                            </div>
                            <template v-else>
                                <div class="d-flex">
                                    <div class="btn grey-bg form-control col-md-6" @click="showDescription = true">Ergänzungen hinzufügen</div> 
                                    <div class="col-sm-12 hidden-xs"></div>
                                </div>
                            </template>
                        </div>
                        <div class="input-container">
                            <div class="overline-s no-line">Themenzuordnung</div>
                            <form class="" v-on:submit.prevent>
                                <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open' : showDropdown }">
                                    <div class="related-categories-container">
                                        <categorychip-component v-for="(category, index) in selectedCategories" :category="category" :index="index" v-on:remove-category-chip="removeCategory" />
                                    </div>
                                    <input ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="questionCategoriesList" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
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
                        </div>
                        <div class="input-container">
                            <div class="overline-s no-line">
                                Sichtbarkeit
                            </div>
                            <div class="privacy-selector" :class="{ 'not-selected' : !licenseIsValid && highlightEmptyFields }">
                                <%: Html.Partial("~/Views/Questions/Edit/EditComponents/EditorPartials/PrivacySelector.vue.ascx") %>
                            </div>
                            <div v-if="!licenseIsValid && highlightEmptyFields"></div>
                        </div>
                    </div>


                    <% if (isAdmin)
                       { %>
                        <div class="">
                            <select v-model="licenseId">
                                <option value="0">Keine Lizenz</option>
                                <option value="1">CC BY 4.0</option>
                                <option value="2">Amtliches Werk BAMF</option>
                                <option value="3">ELWIS</option>
                                <option value="4">BLAC</option>
                            </select>
                        </div>

                    <% } %>


                </div>

                <div class="modal-footer">
                    <div class="btn btn-primary memo-button col-xs-12" @click="save()" :disabled="lockSaveButton">Speichern</div>
                    <div class="btn btn-link memo-button" data-dismiss="modal">Abbrechen</div>

                </div>
            </div>

        </div>
    </div>
