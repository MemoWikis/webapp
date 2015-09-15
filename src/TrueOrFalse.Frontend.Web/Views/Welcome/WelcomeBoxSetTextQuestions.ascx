<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSetTextQuestionsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="media panel-body">
    <div class="pull-left">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(180, false, ImageType.QuestionSet) %>
        </div>        
    </div>
    <div class="media-body">
        <h4 class="media-heading"><%: Model.SetName %></h4>
        <p><%: Model.SetDescription %></p>
    </div>
    <div class="row">
        <% foreach (var question in Model.Questions){ %>
            <div class="col-xs-4" style="padding-top: 10px">
                <div class="caption"><%= question.Text %></div>
                    
            </div>
        <% } %>
    </div>
<%--        <ul>
        <% foreach (var question in Model.Questions){ %>
            <li><%= question.Text %></li>
        <% } %>
        <li>...</li>
    </ul>--%>
    <div class="row" style="margin-top: 10px;">
        <div class="pull-right">
            <a href="<%= Links.SetDetail(Url, Model.Set) %>" class="btn btn-link btn-sm" role="button">Fragesatz anzeigen</a>
            <a href="<%= Links.AnswerQuestion(Url, Model.Questions[0], set:Model.Set) %>" class="btn btn-primary btn-sm" role="button">Alle <%: Model.QuestionCount %> Fragen beantworten</a>
        </div>
    </div>
</div>