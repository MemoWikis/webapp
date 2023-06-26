﻿        <div class="cardModal">
            <div class="modal fade" id="AddToWikiModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
                <div class="modal-dialog modal-m" role="document">
                    <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                    <div class="modal-content">
                        <div class="cardModalContent">
                            <div class="modalHeader">
                                <h4 class="modal-title">Zum Wiki hinzufügen</h4>
                            </div>
                            <div class="modalBody">
                                <form v-on:submit.prevent="selectCategory">
                                    <div>

                                    </div>
                                    <div class="form-group dropdown categorySearchAutocomplete"  :class="{ 'open' : showDropdown }">
                                        <div v-if="showSelectedCategory" class="searchResultItem" @click="toggleShowSelectedCategory()" data-toggle="tooltip" data-placement="top" :title="selectedCategory.Name">
                                            <img :src="selectedCategory.ImageUrl"/>
                                            <div>
                                                <div class="searchResultLabel body-m">{{selectedCategory.Name}}</div>
                                                <div class="searchResultQuestionCount body-s">{{selectedCategory.QuestionCount}} Frage<template v-if="selectedCategory.QuestionCount != 1">n</template></div>
                                            </div>
                                        </div>
                                        <input v-show="!showSelectedCategory" ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="searchList" autocomplete="off" @click="lockDropdown = false"  aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
                                        <ul class="dropdown-menu" aria-labelledby="searchList">
                                            <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)" data-toggle="tooltip" data-placement="top" :title="c.Name" :data-original-title="c.Name">
                                                <img :src="c.ImageUrl"/>
                                                <div>
                                                    <div class="searchResultLabel body-m">{{c.Name}}</div>
                                                    <div class="searchResultQuestionCount body-s">{{c.QuestionCount}} Frage<template v-if="c.QuestionCount != 1">n</template></div>
                                                </div>
                                            </li>
                                            <li class="dropdownFooter body-m">
                                                <b>{{totalCount}}</b> Treffer. <br/>
                                                Deins ist nicht dabei? <span class="dropdownLink" @click="categoryChange = CategoryChangeType.Create">Erstelle hier dein Thema</span>
                                            </li>
                                        </ul>
                                    </div>
                                </form>
                                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{forbiddenCategoryName}}</a>
                                    {{errorMsg}}
                                </div>
                            </div>
                            <div class="modalFooter">
                                <div id="AddToWiki" class="btn btn-primary memo-button" @click="addToParent" :disabled="disableAddCategory">Thema verknüpfen</div>       
                                    <div class="btn btn-link memo-button" data-dismiss="modal" aria-label="Close">Abbrechen</div>
                            </div>   
                        </div>
                    </div>
                </div>
            </div>
        </div>