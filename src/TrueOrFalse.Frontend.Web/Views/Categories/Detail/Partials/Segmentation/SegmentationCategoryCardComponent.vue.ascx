 <%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentationCategoryCardModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

 <% if(Model.Category.CachedData.ChildrenIds.Count > 0 || Model.TotalQuestionCount > 0 || Model.IsInstallationAdmin || Model.Category.Creator.Id == Model.UserId) { %>
 <category-card-component @select-category="selectCategory" @unselect-category="unselectCategory" inline-template :edit-mode="editMode" ref="card<%= Model.Category.Id %>" :is-custom-segment="isCustomSegment" category-id="<%= Model.Category.Id %>" :selected-categories="selectedCategories" :segment-id="id" hide="false" :control-wishknowledge="controlWishknowledge">
    
    <div class="col-xs-6 topic segmentCategoryCard" v-if="visible" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover : showHover }">
        <div class="row">
            <div class="col-xs-3">
                <div v-show="showHover || isSelected" class="checkBox" v-if="editMode"  @click="selectCategory()">
                    <i class="fas fa-check-square" v-if="isSelected"></i>
                    <i class="far fa-square" v-else></i>
                </div>
                <div class="ImageContainer" v-show="editMode">
                    <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category) %> 
                </div>
                <div class="ImageContainer" v-show="!editMode">
                    <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category.Name, Model.Category.Id)) %>
                </div>
            </div>
            <div class="col-xs-9">
                <a class="topic-name" href="<%= Links.CategoryDetail(Model.Category) %>">
                    <div class="topic-name">
                        <%= Model.Category.Type.GetCategoryTypeIconHtml() %><%: Model.Category.Name %>
                        <% if (Model.Category.Visibility == CategoryVisibility.Owner) { %>
                            <i class="fas fa-lock"></i>
                        <% } %>
                    </div>
                    <div v-if="showHover" class="Button dropdown DropdownButton">
                        <a href="#" :id="dropdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                            <li v-if="!isCustomSegment"><a @click="thisToSegment"><i class="fa fa-code-fork"></i>&nbsp;Unterthemen einblenden</a></li>
                            <li><a @click="removeParent"><i class="fas fa-unlink"></i>&nbsp;Thema entfernen</a></li>
                        </ul>
                    </div>
                </a>

                <div class="set-question-count">
                    <span>
                        <pin-category-component :category-id="categoryId"/>
                    </span>

                        <% if (Model.Category.CachedData.ChildrenIds.Count == 1)
                       { %>1 Unterthema <% } %>
                        <% if(Model.Category.CachedData.ChildrenIds.Count > 1)
                           { %><%= Model.Category.CachedData.ChildrenIds.Count  %> Unterthemen <% } 
                           else { %><% } %><%=Model.TotalQuestionCount %> Frage<% if(Model.TotalQuestionCount != 1){ %>n<% } %>
                </div>
                    <%if(Model.TotalQuestionCount > 0) {%>
                    <div class="KnowledgeBarWrapper">
                        <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                    </div>
                <% }%>

            </div>
        </div>
    </div>

</category-card-component>

           