<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxSetImgQModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="media panel-body">
    <div class="media-body" style="padding-bottom: 10px;">
        <h4 class="media-heading"><%: Model.SetName %></h4>
        <h6 style="margin-bottom: 5px; margin-top: 0px; color: #a3a3a3;">Fragesatz mit <a href="<%: Links.SetDetail(Url, Model.Set) %>"><%= Model.QuestionCount %> Fragen</a></h6>
        <p><%: Model.SetText %></p>
    </div>

    <div class="row">
        <% foreach (var question in Model.Questions) { %>
        <div class="col-xs-4">
            <div class="ImageContainer">
                <%= Model.QuestionImageFrontendDatas
                        .First(x => x.Item1 == question.Id).Item2.RenderHtmlImageBasis(200, true, ImageType.Question) %>
            </div>
            <div class="caption" style="padding-top: 10px">
                <p><%= question.Text %></p>
                <%--<a href="<%= Links.AnswerQuestion(Url, Model.Questions[0], set:Model.Set) %>" class="btn btn-primary btn-sm" role="button">Beantworten</a>--%>
            </div>
        </div>
        <% } %>
    </div>
    <div class="pull-right">
        <a href="<%= Links.AnswerQuestion(Url, Model.Questions[0], set:Model.Set) %>" class="btn btn-primary btn-sm" role="button">Alle <%: Model.QuestionCount %> Fragen beantworten</a>
    </div>
</div>