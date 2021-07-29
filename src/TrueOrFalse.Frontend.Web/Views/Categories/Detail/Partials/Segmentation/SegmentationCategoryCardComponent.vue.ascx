<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

 <category-card-component @select-category="selectCategory" @unselect-category="unselectCategory" inline-template :edit-mode="editMode" :ref="'card' + category.Id" :is-custom-segment="isCustomSegment" :category-id="category.Id" :selected-categories="selectedCategories" :segment-id="segmentId" hide="false" :key="index" :is-my-world="isMyWorld"  :category="category">
    
    <div class="col-xs-6 topic segmentCategoryCard" v-if="visible" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }" @contextmenu.prevent="handler($event)">
        <div class="row">
            <div class="col-xs-3">
<%--                <div class="checkBox" @click="selectCategory()" :class="{ show : showHover && isCustomSegment, selected : isSelected && isCustomSegment }">
                    <i class="fas fa-check-square" v-if="isSelected"></i>
                    <i class="far fa-square" v-else></i>
                </div>--%>
                <div class="ImageContainer" v-html="category.ImgHtml">
                </div>
            </div>
            <div class="col-xs-9">
                <a class="topic-name" :href="category.LinkToCategory">
                    <div class="topic-name">
                        <template v-html="category.CategoryTypeHtml"></template> {{category.Name}}
                        <i v-if="category.Visibility == 1" class="fas fa-lock"></i>
                    </div>
                    <div class="Button dropdown DropdownButton" :class="{ hover : showHover }">
                        <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                            <li v-if="!isCustomSegment && !isMyWorld"><a @click="thisToSegment">
                                <div class="dropdown-icon"><i class="fas fa-sitemap"></i></div>Unterthemen einblenden
                            </a></li>
                            <li><a @click="removeParent">
                                <div class="dropdown-icon"><i class="fas fa-unlink"></i></div>Verknüpfung entfernen
                            </a></li>
                        </ul>
                    </div>
                </a>

                <div class="set-question-count">
                    <span>
                        <pin-category-component :category-id="categoryId" :initial-wishknowledge-state="category.IsInWishknowledge"/>
                    </span>
                    
                    <template v-if="category.ChildCategoryCount == 1">1 Unterthema</template>
                    <template v-else-if="category.ChildCategoryCount > 1">{{category.ChildCategoryCount}} Unterthemen</template>
                    {{category.QuestionCount}} Frage<template v-if="category.QuestionCount != 1">n</template>
                </div>
                <div v-if="category.QuestionCount > 0" class="KnowledgeBarWrapper">
                    <div v-html="category.KnowledgeBarHtml"></div>
                    <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                </div>

            </div>
        </div>
    </div>

</category-card-component>

           