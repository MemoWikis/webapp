<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SegmentModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<segment-component inline-template :edit-mode="editMode">
    <div>
        <div class="segmentSubHeader">
            <h2>
                <%= Model.Title %>
                <span class="Button Pin" data-category-id="<%= Model.Category.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 18px; height: 10px;padding-right:4px">
                        <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.Category.IsInWishknowledge(), displayAdd:false)) %>
                    </a>
                </span>
            </h2>
            <div class="segmentKnowledgeBar">

            </div>
        </div>
        <div class="topicNavigation" :key="cardsKey">
            <% foreach (var category in Model.ChildCategories) {%>
                <%: Html.Partial("~/Views/Categories/Detail/Partials/Segmentation/SegmentationCategoryCardComponent.vue.ascx", new SegmentationCategoryCardModel(category)) %>
            <%} %>
        </div>
    </div>
</segment-component>
