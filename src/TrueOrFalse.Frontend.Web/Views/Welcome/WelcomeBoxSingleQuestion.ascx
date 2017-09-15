<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSingleQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var linkAnswerQuestion = Links.AnswerQuestion(Model.QuestionText, Model.QuestionId, paramElementOnPage: 1, categoryFilter: Model.ContextCategoryName); %>

<div class="CardColumn">
    <div class="thumbnail">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(200, true, ImageType.Question, linkAnswerQuestion) %>
        </div>

        <div class="caption">
            <h6 style="margin-bottom: 5px; color: #a3a3a3;">Thema mit <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.ContextCategoryName, Model.ContextCategoryId) %>"><%= Model.QuestionCount %> Fragen</a></h6>
            <h4 style="margin-top: 5px;"><%: Model.ContextCategoryName %></h4>
            <p><%: Model.QuestionText %></p>
            <p style="text-align: center;">
                <a href="<%= linkAnswerQuestion %>" class="btn btn-primary" role="button">Beantworten</a>
            </p>
        </div>
    </div>
</div>
