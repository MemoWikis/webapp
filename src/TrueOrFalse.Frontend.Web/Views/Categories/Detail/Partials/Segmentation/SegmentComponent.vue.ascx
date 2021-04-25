<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<segment-component inline-template :edit-mode="editMode" ref="segment<%= Model.Category.Id %>" title="<%= Model.Title %>" child-category-ids="<%= Model.ChildCategoryIds %>" category-id="<%= Model.Category.Id %>">
    <div v-if="visible" class="segment" :data-category-id="categoryId" :data-child-category-ids="currentChildCategoryIds" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
        <div class="segmentSubHeader">
            <div class="segmentHeader">
                <div class="segmentTitle">
                    <a href="<%= Links.CategoryDetail(Model.Category) %>">                        
                        <h2>
                            <%= Model.Title %>
                            <% if (Model.Category.Visibility == CategoryVisibility.Owner) {%>
                                <i class="fas fa-lock"></i>
                            <%}%>
                        </h2>
                    </a>
                    <pin-category-component :category-id="categoryId" @update-knowledge-bar="updateKnowledgeBar"/>

                </div>
                <div class="Button dropdown DropdownButton segmentDropdown" :class="{ hover : showHover }">
                    <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li><a @click="removeChildren">
                            <div class="dropdown-icon"><i class="fas fa-unlink"></i></div>Themen entfernen
                        </a></li>
                        <li><a @click="removeSegment">
                            <div class="dropdown-icon"><i class="fas fa-trash"></i></div>Unterthemen ausblenden
                        </a></li>
                    </ul>
                </div>
            </div>
            
            <div class="segmentKnowledgeBar">
                <div class="KnowledgeBarWrapper">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                </div>
            </div>
        </div>
        <div class="topicNavigation row" :key="cardsKey">
            <% foreach (var category in Model.ChildCategories) {%>
                <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category)) %>
            <%} %>
            <div class="col-xs-6 addCategoryCard" @click="addCategory" :id="addCategoryId">
                <div>
                     <i class="fas fa-plus"></i> Neues Thema hinzufügen
                </div>
            </div>
        </div>
    </div>
</segment-component>
