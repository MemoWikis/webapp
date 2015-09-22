<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSingleSetModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="ThumbnailColumn">
    <div class="thumbnail">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(200, true, ImageType.QuestionSet) %>
        </div>

        <div>
            <!-- % Html.RenderPartial("Category", Model.Question); % -->
        </div>

        <div class="caption">
            <h6 style="margin-bottom: 5px; color: #a3a3a3;">Fragesatz mit <a href="<%= Links.SetDetail(Url,Model.SetName,Model.SetId) %>"><%= Model.QCount %> Fragen</a></h6>
            <h4 style="margin-top: 5px;"><%: Model.SetName %></h4>
            <p><%: Model.SetText %></p>
            <p style="text-align: center;">
                <a href="<%= Links.AnswerQuestion(Url, Model.FirstQText, Model.FirstQId, Model.SetId) %>" class="btn btn-primary btn-sm" role="button">Alle beantworten</a>
                <%--<a href="<%= Links.StartSetLearningSession(Model.SetId) %>" class="btn btn-link btn-sm" role="button">Jetzt üben (10 Fragen)</a>--%>
            </p>
        </div>
    </div>
</div>
