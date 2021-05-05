<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<add-category-component inline-template>
        <div class="categoryCardModal">
            <div class="modal fade" id="AddCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
                <div class="modal-dialog modal-s" role="document">
                    <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                    <div class="modal-content">
                        <div class="addCategoryCardModal">
                            <div class="modalHeader">
                                <h4 class="modal-title">Neues Thema erstellen</h4>
                            </div>
                            <div class="modalBody" v-if="createCategory">
                                <form v-on:submit.prevent="addCategory">
                                    <div class="form-group">
                                        <input class="form-control" v-model="name" placeholder="Bitte gib den Namen des Themas ein" />
                                        <small class="form-text text-muted"></small>
                                    </div>
                                </form>
                                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{existingCategoryName}}</a>
                                    {{errorMsg}}
                                </div>
                                <div class="checkBox" :disabled="parentIsPrivate" @click="togglePrivacy()">
                                    <i class="fas fa-check-square" v-if="isPrivate" ></i>
                                    <i class="far fa-square" v-else></i>
                                    Privates Thema
                                </div>
                            </div>
                            <div class="modalBody" v-else>
                                <form v-on:submit.prevent="selectCategory">
                                    <div class="form-group dropdown categorySearchAutocomplete"  :class="{ 'open' : showDropdown}">
                                        <input class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="searchList" @click="lockDropdown = false"  aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
                                        <ul class="dropdown-menu" aria-labelledby="searchList">
                                            <li class="">Kein Treffer? Bitte weitertippen oder Thema in neuem Tab erstellen</li>
                                            <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)">
                                                <img :src="c.ImageUrl"/>
                                                <div>
                                                    <div class="searchResultLabel">{{c.Name}}</div>
                                                    <div class="searchResultQuestionCount">{{c.QuestionCount}} Frage<template v-if="c.QuestionCount != 1">n</template></div>
                                                </div>
                                            </li>
                                            <li class="underline-s">{{totalCount}}</li>
                                        </ul>
                                    </div>
                                </form>
                                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{existingCategoryName}}</a>
                                    {{errorMsg}}
                                </div>
                            </div>
                            <div class="modalFooter">
                                <div v-if="createCategory">
                                    <div class="btn btn-primary memo-button" @click="addCategory" :disabled="disableAddCategory">Thema Erstellen</div>       
                                    <div class="btn btn-link memo-button" @click="createCategory = false" >Bestehendes Thema Hinzufügen</div>
                                </div>
                                <div v-else>
                                    <div class="btn btn-primary memo-button" @click="addExistingCategory" :disabled="disableAddCategory">Thema Hinzufügen</div>       
                                    <div class="btn btn-link memo-button" @click="createCategory = true">Neues Thema Erstellen</div>
                                </div>
                            </div>   
                        </div>
                    </div>
                </div>
            </div>
        </div>
</add-category-component>
