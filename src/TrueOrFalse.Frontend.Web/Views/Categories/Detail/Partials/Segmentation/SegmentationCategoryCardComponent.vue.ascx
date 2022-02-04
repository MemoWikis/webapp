<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<category-card-component @select-category="selectCategory" @unselect-category="unselectCategory" inline-template :ref="'card' + category.Id" :is-custom-segment="isCustomSegment" :category-id="category.Id" :selected-categories="selectedCategories" :segment-id="segmentId" hide="false" :key="index" :is-my-world="isMyWorld" :category="category" :is-historic="isHistoric">
    <div class="col-xs-6 topic segmentCategoryCard" v-if="visible" @mouseover="mouseOver" @mouseleave="mouseLeave" :class="{ hover : showHover }">
        <div class="row" v-on:click.self="goToCategory()">
            <div class="col-xs-3">
                <a :href="category.LinkToCategory">
                    <div class="ImageContainer" v-html="category.ImgHtml">
                    </div>
                </a>
            </div>
            <div class="col-xs-9">
                <div class="topic-name">
                    <a :href="category.LinkToCategory">
                        <template v-html="category.CategoryTypeHtml"></template> {{category.Name}}
                    </a>
                    <div v-if="visibility == 1" class="segmentCardLock" @click="openPublishModal" data-toggle="tooltip" title="Thema ist privat. Zum Veröffentlichen klicken.">
                        <i class="fas fa-lock"></i>
                        <i class="fas fa-unlock"></i>
                    </div>
                </div>
                <div v-if="!isHistoric && !isMyWorld" class="Button dropdown DropdownButton" :class="{ hover : showHover && !isHistoric && !isMyWorld}">
                    <a href="#" :id="dropdownId" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li v-if="!isCustomSegment && !isMyWorld">
                            <a @click="thisToSegment">
                                <div class="dropdown-icon">
                                    <i class="fas fa-sitemap"></i>
                                </div>Unterthemen einblenden
                            </a>
                        </li>
                        <li>
                            <a @click="removeParent">
                                <div class="dropdown-icon">
                                    <i class="fas fa-unlink"></i>
                                </div>Verknüpfung entfernen
                            </a>
                        </li>
                        <li v-if="visibility == 1">
                            <a @click="openPublishModal">
                                <div class="dropdown-icon">
                                    <i class="fas fa-unlock"></i>
                                </div>Thema veröffentlichen
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="set-question-count">
                    <span v-if="!category.IsPersonalHomepage">
                        <pin-category-component :category-id="categoryId" :initial-wishknowledge-state="category.IsInWishknowledge"/>
                    </span>
                    <a :href="category.LinkToCategory" class="sub-label">
                        <template v-if="category.ChildCategoryCount == 1">1 Unterthema</template>
                        <template v-else-if="category.ChildCategoryCount > 1">{{category.ChildCategoryCount}} Unterthemen</template>
                        <span v-if="category.QuestionCount > 0">{{category.QuestionCount}} Frage<template v-if="category.QuestionCount != 1">n</template></span>
                    </a>
                </div>
                <a :href="category.LinkToCategory">
                    <div v-if="category.QuestionCount > 0" class="KnowledgeBarWrapper">
                        <div v-html="category.KnowledgeBarHtml"></div>
                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                    </div>
                </a>
            </div>
        </div>
    </div>
</category-card-component>