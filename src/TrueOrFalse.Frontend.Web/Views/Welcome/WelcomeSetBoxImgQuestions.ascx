<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeSetBoxImgQuestionsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="media panel-body">
    <div class="media-body">
        <h4 class="media-heading"><%: Model.SetName %> (<%: Model.QuestionCount %> Fragen im Fragesatz)</h4>
    </div>
<%--    <div class="pull-left">
        <div class="ImageContainer">
            <%= Model.QuestionImageFrontendDatas.RenderHtmlImageBasis(120, true, ImageType.QuestionSet) %>
        </div>        
    </div>--%>
    <div class="media-body">
        <% foreach (var question in Model.Questions) { %>
            <div class="media-body">
                <div class="pull-left">
                    <div class="ImageContainer">
                        <%= Model.QuestionImageFrontendDatas.First(x => x.ImageMetaData.TypeId == question.Id).RenderHtmlImageBasis(120, true, ImageType.Question) %>
                    </div>
                </div>
                <p><%= question.Text %></p>
                <a href="<%= Links.AnswerQuestion(Url, Model.Questions[0], set:Model.Set) %>" class="btn btn-primary btn-sm" role="button">Beantworten</a>
            </div>
        <% } %>
        <div class="pull-right">
            <a href="<%= Links.SetDetail(Url, Model.Set) %>" class="btn btn-link btn-sm" role="button">Fragesatz anzeigen</a>
            <a href="<%= Links.AnswerQuestion(Url, Model.Questions[0], set:Model.Set) %>" class="btn btn-link btn-sm" role="button">Alle <%: Model.QuestionCount %> Fragen beantworten</a>
        </div>
    </div>
</div>