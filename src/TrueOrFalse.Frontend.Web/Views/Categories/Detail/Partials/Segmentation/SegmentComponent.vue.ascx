<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<segment-component inline-template :edit-mode="editMode" :ref="'segment'+ s.CategoryId" :title="s.Title" :child-category-ids="s.ChildCategoryIds" :category-id="s.CategoryId">
    <div class="segment" :data-category-id="categoryId" :data-child-category-ids="currentChildCategoryIdsString" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
        <div class="segmentSubHeader">
            <div class="segmentHeader">
                <div class="segmentTitle">
                    <a :href="linkToCategory">                        
                        <h2>
                            {{segmentTitle}}
                            <i v-if="visibility == 1" class="fas fa-lock"></i>
                        </h2>
                    </a>
                    <pin-category-component :category-id="categoryId"/>

                </div>
                <div class="Button dropdown DropdownButton segmentDropdown" :class="{ hover : showHover }">
                    <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li @click="removeSegment()"><a>
                            <div class="dropdown-icon"><img class="fas" src="/Images/Icons/sitemap-disable.svg"/></div>Unterthema ausblenden
                        </a></li>
<%--                        <li @click="removeChildren()" :disabled="disabled"><a>
                            <div class="dropdown-icon"><i class="fas fa-unlink"></i></div>Verknüpfungen entfernen
                        </a></li>--%>
                    </ul>
                </div>
            </div>
            
            <div class="segmentKnowledgeBar">
                <div class="KnowledgeBarWrapper" v-html="knowledgeBarHtml">
                </div>
            </div>
        </div>
        <div class="topicNavigation row" :key="cardsKey">
            <template v-for="(category, index) in categories">
                <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx")%>
            </template>

            <div class="col-xs-6 addCategoryCard memo-button" @click="addCategory" :id="addCategoryId">
                <div>
                     <i class="fas fa-plus"></i> Neues Thema
                </div>
            </div>
        </div>
    </div>
</segment-component>
