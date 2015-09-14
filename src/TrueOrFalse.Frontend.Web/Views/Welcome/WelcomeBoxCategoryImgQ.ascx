<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxCategoryImgQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="media panel-body">
    <div class="media-body" style="padding-bottom: 10px;">
        <h4 class="media-heading"><%: Model.CategoryName %></h4>
        <p><%: Model.CategoryDescription %></p>
    </div>

    <div class="row">
        <% foreach (var question in Model.Questions) { %>
        <div class="col-xs-4">
            <div class="ImageContainer">
                <%= Model.QuestionImageFrontendDatas.First(x => x.ImageMetaData.TypeId == question.Id).RenderHtmlImageBasis(120, true, ImageType.Question) %>
            </div>
            <div class="caption" style="padding-top: 10px">
                <p><%= question.Text %></p>
            </div>
        </div>
        <% } %>
    </div>
    <div class="pull-right">
        <%--get link to point to random question in set:--%> 
        <a href="<%= Links.AnswerQuestion(Url, Model.Questions.First().Text, Model.Questions.First().Id, paramElementOnPage:1, categoryFilter:Model.CategoryName) %>" class="btn btn-primary btn-sm" role="button">Alle <%: Model.QuestionCount %> Fragen beantworten</a>
    </div>
</div>