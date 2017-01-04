﻿<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>



<h4>Verwandte Inhalte</h4>
<div id="Content" class="Box">
 <% if(Model.CountReferences > 0) { %>
        <h5 class="ContentSubheading Question">Fragen mit diesem Medium als Quellenangabe (<%=Model.CountReferences %>)</h5>
        <div class="LabelList">
            <% var index = 0; foreach(var question in Model.TopQuestionsWithReferences){ index++;%>
                <div class="LabelItem LabelItem-Question">
                    <div class="EllipsWrapper">
                        <a href="<%= Links.AnswerQuestion(Url, question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                    </div>
                </div>
            <% } %>
        </div>
    <% } %>
            
    <% if(Model.TopQuestionsInSubCats.Count > 0){ %>
        <h5 class="ContentSubheading Question">Fragen in untergeordneten Kategorien</h5>
        <div class="LabelList">
        <% var index = 0; foreach(var question in Model.TopQuestionsInSubCats){ index++;%>
            <div class="LabelItem LabelItem-Question">
                <div class="EllipsWrapper">
                    <a href="<%= Links.AnswerQuestion(question) %>"><%= question.GetShortTitle(150) %></a>
                </div>
            </div>
        <% } %>
        </div>
    <% } %>
</div>
