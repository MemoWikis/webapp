<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    <div class="segment">
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
                    <pin-category-component category-id="<%= Model.Category.Id %>"/>

                </div>
            </div>
            
            <div class="segmentKnowledgeBar">
                <div class="KnowledgeBarWrapper">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                </div>
            </div>
        </div>
        <div class="topicNavigation row">

           <% foreach (var category in Model.ChildCategories) {%>
               <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.Placeholder.ascx", new SegmentationCategoryCardModel(category)) %>
           <%} %>
        </div>
    </div>
