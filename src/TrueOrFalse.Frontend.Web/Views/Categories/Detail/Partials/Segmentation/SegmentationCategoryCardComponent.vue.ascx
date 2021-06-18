<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

 <category-card-component @select-category="selectCategory" @unselect-category="unselectCategory" inline-template :edit-mode="editMode" :ref="'card' + id" :is-custom-segment="isCustomSegment" :category-id="id" :selected-categories="selectedCategories" :segment-id="segmentId" hide="false">
    
    <div class="col-xs-6 topic segmentCategoryCard" v-if="visible" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
        <div class="row">
            <div class="col-xs-3">
                <div class="checkBox" @click="selectCategory()" :class="{ show : showHover, selected : isSelected }">
                    <i class="fas fa-check-square" v-if="isSelected"></i>
                    <i class="far fa-square" v-else></i>
                </div>
                <div class="ImageContainer" v-html="imgHtml">
                </div>
            </div>
            <div class="col-xs-9">
                <a class="topic-name" :href="linkToCategory">
                    <div class="topic-name">
                        <template v-html="categoryTypeHtml"></template> {{categoryName}}
                        <i v-if="visibility == 1" class="fas fa-lock"></i>
                    </div>
                    <div class="Button dropdown DropdownButton" :class="{ hover : showHover }">
                        <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                            <li v-if="!isCustomSegment"><a @click="thisToSegment">
                                <div class="dropdown-icon"><i class="fa fa-code-fork"></i></div>Unterthemen einblenden
                            </a></li>
                            <li><a @click="removeParent">
                                <div class="dropdown-icon"><i class="fas fa-unlink"></i></div>Verknüpfung entfernen
                            </a></li>
                        </ul>
                    </div>
                </a>

                <div class="set-question-count">
                    <span>
                        <pin-category-component :category-id="categoryId"/>
                    </span>
                    
                    <template v-if="childCategoryCount == 1">1 Unterthema</template>
                    <template v-else-if="childCategoryCount > 1">{{childCategoryCount}} Unterthemen</template>
                    {{questionCount}} Frage<template v-if="questionCount != 1">n</template>
                </div>
                <div v-if="questionCount > 0" class="KnowledgeBarWrapper">
                    <div v-html="knowledgeBarHtml"></div>
                    <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                </div>

            </div>
        </div>
    </div>

</category-card-component>

           