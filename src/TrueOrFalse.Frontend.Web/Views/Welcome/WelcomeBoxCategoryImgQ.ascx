<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxCategoryImgQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var primaryActionUrl = Links.AnswerQuestion(Model.Questions.First().Text, Model.Questions.First().Id, paramElementOnPage: 1, categoryFilter: Model.CategoryName); %>

<div class="media panel-body">
    <div class="media-body" style="padding-bottom: 10px;">
        <h4 class="media-heading"><%: Model.CategoryName %></h4>
        <h6 style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">Thema mit <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.CategoryName, Model.CategoryId) %>"><%= Model.QuestionCount %> Fragen</a></h6>
        <p><%: Model.CategoryDescription %></p>
    </div>

    <div class="row">
        <% foreach (var question in Model.Questions) { %>
        <div class="col-xs-4">
            <div class="ImageContainer">
                <%= Model.QuestionImageFrontendDatas.First(x => x.Item1 == question.Id).Item2.RenderHtmlImageBasis(120, true, ImageType.Question, linkToItem: primaryActionUrl) %>
            </div>
            <div class="caption" style="padding-top: 10px">
                <p><%= question.Text %></p>
            </div>
        </div>
        <% } %>
    </div>
    <div class="pull-right">
        <a href="<%= primaryActionUrl %>" class="btn btn-primary btn-sm" role="button">Alle <%: Model.QuestionCount %> Fragen beantworten</a>
    </div>
</div>