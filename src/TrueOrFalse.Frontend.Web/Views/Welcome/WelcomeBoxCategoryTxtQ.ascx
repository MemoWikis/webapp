<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxCategoryTxtQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="media panel-body">
    <div class="pull-left">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(180, false, ImageType.QuestionSet) %>
        </div>        
    </div>
    <div class="media-body">
        <h4 class="media-heading"><%: Model.CategoryName %></h4>
        <h6 style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">Kategorie mit <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.CategoryName, Model.CategoryId) %>"><%= Model.QuestionCount %> Fragen</a></h6>
        <p><%: Model.CategoryDescription %></p>
    </div>
    <div class="row" style="clear: left;">
        <% foreach (var question in Model.Questions){ %>
            <div class="col-xs-4" style="padding-top: 10px">
                <div class="caption"><%= question.Text %></div>
            </div>
        <% } %>
    </div>
    <div class="row" style="clear: left;">
        <% foreach (var question in Model.Questions){ %>
            <div class="col-xs-4">
                <p style="margin-top: 5px; text-align: center;">
                    <a href="<%= Links.AnswerQuestion(Url, question.Text, question.Id, paramElementOnPage:1, categoryFilter:Model.CategoryName) %>" class="btn btn-xs btn-primary" role="button">Beantworten</a>
                </p>
            </div>
        <% } %>


    </div>

</div>