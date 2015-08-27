<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeSetBoxModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="ThumbnailColumn">
    <div class="thumbnail">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(200, true, ImageType.Question) %>
        </div>

        <div>
            <!-- % Html.RenderPartial("Category", Model.Question); % -->
        </div>

        <div class="caption">
            <h4><%: Model.ContextCategoryName %></h4>
            <p><%: Model.Title %></p>
            <a href="<%= Links.AnswerQuestion(Url, Model.Question, paramElementOnPage:1, categoryFilter:Model.ContextCategoryName) %>" class="btn btn-primary" role="button">beantworten</a>
        </div>
    </div>
</div>
