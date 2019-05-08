<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

    <% if(Model.CountReferences > 0 || Model.TopQuestionsInSubCats.Count > 0) { %>
        <h4>Verwandte Inhalte</h4>
        <div id="Content" class="Box">
             <% if(Model.CountReferences > 0) { %>
                <h5 class="ContentSubheading Question">
                    Fragen mit diesem Medium als Quellenangabe (<%=Model.CountReferences %>)
                    <a href="#relatedCountReferences" data-toggle="collapse" class="greyed noTextdecoration" style="font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Inhalte ein-/ausblenden</a>
                </h5>
                <div id="relatedCountReferences" class="collapse">
                    <div class="LabelList">
                        <% var index = 0; foreach(var question in Model.TopQuestionsWithReferences){ index++;%>
                            <div class="LabelItem LabelItem-Question">
                                <a href="<%= Links.AnswerQuestion(question, paramElementOnPage: index, categoryFilter:Model.Name) %>" rel="nofollow"><%= question.GetShortTitle(150) %></a>
                            </div>
                        <% } %>
                    </div>
                </div>
            <% } %>
                
            <% if(Model.TopQuestionsInSubCats.Count > 0){ %>
                <h5 class="ContentSubheading Question">
                    Fragen in untergeordneten Themen
                    <a href="#relatedTopQuestionsList" data-toggle="collapse" class="greyed noTextdecoration" style="font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Inhalte ein-/ausblenden</a>
                </h5>
                <div id="relatedTopQuestionsList" class="collapse">
                    <div class="LabelList">
                        <% var index = 0; foreach(var question in Model.TopQuestionsInSubCats){ index++;%>
                            <div class="LabelItem LabelItem-Question">
                                <a href="<%= Links.AnswerQuestion(question) %>"><%= question.GetShortTitle(150) %></a>
                            </div>
                        <% } %>
                    </div>
                </div>
            <% } %>
        </div>
    <% } %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>

