<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<TopicOfWeekModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="category-of-week-container">
    <h3 class="greyed">Thema der Woche</h3>
    <div class="separator-category"></div>

    <div class="category-of-week-container-inner">
        <h1><%= Model.TopicOfWeekTitle %></h1>

        <div class="ImageContainer category-main-image" style="clear: both;">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
        </div>

        <%= Model.TopicDescriptionHTML %>

        <div class="category-of-week-footer">
            <a class="btn btn-lg btn-primary" href="<%= Links.CategoryDetail(Model.Category) %>">Zur Themenseite <%= (Model.TopicOfWeekTitle.Length) > 15 ? "<br />" : "" %><%= Model.TopicOfWeekTitle.TruncateAtWordWithEllipsisText(40,"...") %></a>
            
            <br class="visible-xs" />
            <span style="display: inline-block; margin-top: 10px;" class="Pin float-right-sm-up" data-category-id="<%= Model.CategoryId %>">
                <%= Html.Partial("AddToWishknowledgeButton", new AddToWishknowledge(Model.IsInWishknowledge)) %>
            </span>
        </div>

        <div class="category-of-week-quiz">
            <h3>Quiz der Woche</h3>
            <script src="https://memucho.de/views/widgets/w.js" data-t="set" data-id="<%= Model.QuizOfWeekSetId %>" data-width="100%" data-hideKnowledgeBtn="true"></script>
        </div>

        <div class="category-of-week-additional-recom">
            <h3><%= Model.TopicOfWeekTitle %>: Entdecke weitere Themenbereiche</h3>
            <div class="row CardsMiniPortrait" style="padding-left: -10px; padding-right: -10px;">
                <% foreach (var categoryId in Model.AdditionalCategoriesIds) { %>
                    <div class="CardMiniColumn col-xs-6 col-sm-3 xxs-stack">
                        <% Html.RenderPartial("WelcomeCardMiniCategory", new WelcomeCardMiniCategoryModel(categoryId)); %>
                    </div>
                <% } %>
            </div>
        </div>
    </div>
    <div class="separator-category"></div>

</div>
