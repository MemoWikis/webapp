<add-category-component inline-template>
    <div class="cardModal">
        <div class="modal fade" id="AddCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
            <div class="modal-dialog modal-m" role="document">
                <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>

                <div class="modal-content">
                    <div class="cardModalContent">
                        <div class="modalHeader">
                            <h4 v-if="editCategoryRelation === editCategoryRelationType.Create" class="modal-title">Neues Thema erstellen</h4>
                            <h4 v-else-if="editCategoryRelation === editCategoryRelationType.Move" class="modal-title">Thema verschieben nach</h4>
                            <h4 v-else-if="editCategoryRelation === editCategoryRelationType.AddChild" class="modal-title">Bestehendes Thema verknüpfen</h4>
                            <h4 v-else-if="editCategoryRelation === editCategoryRelationType.AddParent" class="modal-title">Neues Oberthema verknüpfen</h4>
                            <h4 v-else-if="editCategoryRelation === editCategoryRelationType.AddToWiki" class="modal-title">Zum Wiki hinzufügen</h4>
                        </div>

                        <div class="modalBody" v-if="editCategoryRelation == editCategoryRelationType.Create">
                            <form v-on:submit.prevent="addCategory">
                                <div class="form-group">
                                    <input class="form-control" v-model="name" placeholder="Bitte gib den Namen des Themas ein"/>
                                    <small class="form-text text-muted"></small>
                                </div>
                            </form>
                            <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{forbiddenCategoryName}}</a>
                                {{errorMsg}}
                            </div>
                            <div class="categoryPrivate">
                                <p><b> Das Thema ist privat.</b> Du kannst es später im das Dreipunkt-Menü oder direkt über das Schloss-Icon veröffentlichen.</p>
                            </div>
                        </div>

                        <div class="modalBody" v-else-if="editCategoryRelation == editCategoryRelationType.AddToWiki">
                            <form v-on:submit.prevent="selectCategory">
                                <div class="categorySearchAutocomplete" v-if="personalWiki != null" @click="selectedParentInWikiId = personalWiki.Id">
                                    <div class="searchResultItem">
                                        <img :src="personalWiki.ImageUrl"/>
                                        <div>
                                            <div class="searchResultLabel body-m">{{personalWiki.Name}}</div>
                                            <div class="searchResultQuestionCount body-s">{{personalWiki.QuestionCount}} Frage<template v-if="personalWiki.QuestionCount != 1">n</template></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="categorySearchAutocomplete">
                                    <template v-for="previousCategory in addToWikiHistory">
                                        <div class="searchResultItem" @click="selectedParentInWikiId = previousCategory.Id">
                                            <img :src="previousCategory.ImageUrl"/>
                                            <div>
                                                <div class="searchResultLabel body-m">{{previousCategory.Name}}</div>
                                                <div class="searchResultQuestionCount body-s">{{previousCategory.QuestionCount}} Frage<template v-if="previousCategory.QuestionCount != 1">n</template></div>
                                            </div>
                                        </div>
                                    </template>
                                </div>
                                <div @click="hideSearch = false" v-if="hideSearch">
                                    <a> Zu anderem Thema hinzufügen</a>
                                </div>
                                <div v-else class="form-group dropdown categorySearchAutocomplete" :class="{ 'open' : showDropdown }">
                                    <div v-if="showSelectedCategory" class="searchResultItem" @click="toggleShowSelectedCategory()" data-toggle="tooltip" data-placement="top" :title="selectedCategory.Name">
                                        <img :src="selectedCategory.ImageUrl"/>
                                        <div>
                                            <div class="searchResultLabel body-m">{{selectedCategory.Name}}</div>
                                            <div class="searchResultQuestionCount body-s">{{selectedCategory.QuestionCount}} Frage<template v-if="selectedCategory.QuestionCount != 1">n</template></div>
                                        </div>
                                    </div>
                                    <input v-show="!showSelectedCategory" ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="searchInWikiList" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
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
                                            Deins ist nicht dabei? <span class="dropdownLink" @click="editCategoryRelation = editCategoryRelationType.Create">Erstelle hier dein Thema</span>
                                        </li>
                                    </ul>
                                </div>
                            </form>
                            <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{forbiddenCategoryName}}</a>
                                {{errorMsg}}
                            </div>
                        </div>

                        <div class="modalBody" v-else>
                            <form v-on:submit.prevent="selectCategory">
                                <div class="form-group dropdown categorySearchAutocomplete" :class="{ 'open' : showDropdown }">
                                    <div v-if="showSelectedCategory" class="searchResultItem" @click="toggleShowSelectedCategory()" data-toggle="tooltip" data-placement="top" :title="selectedCategory.Name">
                                        <img :src="selectedCategory.ImageUrl"/>
                                        <div>
                                            <div class="searchResultLabel body-m">{{selectedCategory.Name}}</div>
                                            <div class="searchResultQuestionCount body-s">{{selectedCategory.QuestionCount}} Frage<template v-if="selectedCategory.QuestionCount != 1">n</template></div>
                                        </div>
                                    </div>
                                    <input v-show="!showSelectedCategory" ref="searchInput" class="form-control dropdown-toggle" type="text" v-model="searchTerm" id="searchList" autocomplete="off" @click="lockDropdown = false" aria-haspopup="true" placeholder="Bitte gib den Namen des Themas ein"/>
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
                                            Deins ist nicht dabei? <span class="dropdownLink" @click="editCategoryRelation = editCategoryRelationType.Create">Erstelle hier dein Thema</span>
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
                            <div v-if="editCategoryRelation === editCategoryRelationType.Create" id="AddNewCategoryBtn" class="btn btn-primary memo-button" @click="addCategory" :disabled="disableAddCategory">Thema erstellen</div>
                            <div v-else-if="editCategoryRelation === editCategoryRelationType.Move" id="MoveCategoryToNewParentBtn" class="btn btn-primary memo-button" @click="moveCategoryToNewParent" :disabled="disableAddCategory">Thema verschieben</div>
                            <div v-else-if="editCategoryRelation === editCategoryRelationType.AddChild" id="AddExistingCategoryBtn" class="btn btn-primary memo-button" @click="addExistingCategory" :disabled="disableAddCategory">Thema verknüpfen</div>
                            <div v-else-if="editCategoryRelation === editCategoryRelationType.AddParent" id="AddNewParentBtn" class="btn btn-primary memo-button" @click="addNewParentToCategory" :disabled="disableAddCategory">Thema verknüpfen</div>
                            <div v-else-if="editCategoryRelation === editCategoryRelationType.AddToWiki" id="AddToWiki" class="btn btn-primary memo-button" @click="addNewParentToCategory" :disabled="disableAddCategory">Thema verknüpfen</div>
                            <div class="btn btn-link memo-button" data-dismiss="modal" aria-label="Close">Abbrechen</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</add-category-component>