<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>
                
    <h4>Alle Inhalte</h4>
    
    <% if (Model.Category.IsHistoric) { %>
        <div class="alert alert-info" role="alert">
            Aus technischen Gründen können <b>keine Archivdaten für <i>Fragen</i></b> angezeigt werden. 
            Es werden die aktuellen Fragen dargestellt.
        </div>
    <% } %>
    
    <div id="Content" class="Box">
        <h5 class="ContentSubheading Question">
            <%= Model.CountAggregatedQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountAggregatedQuestions,"n") %> zu diesem Thema
            <a href="#aggregatedTopQuestionsList" data-toggle="collapse" class="greyed noTextdecoration" style="font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Fragen ein-/ausblenden</a>
        </h5>
        
        <% if (Model.CountAggregatedQuestions > 0){ %>
            <div id="aggregatedTopQuestionsList" class="collapse">
                <div class="LabelList">
                    <% var index = 0;
    
                       foreach (var question in Model.TopQuestions){
                           index++; %>
                        <div class="LabelItem LabelItem-Question">
                            <a href="<%= Links.AnswerQuestion(question, paramElementOnPage: index, categoryFilter: Model.Name) %>">
                                <%= question.Text %>
                            </a>
                        </div>
                    <% } %>
                </div>
                <div style="margin-bottom: 15px;">
                    <% if(Model.CountAggregatedQuestions > CategoryModel.MaxCountQuestionsToDisplay){ %>
                        und weitere Fragen...
                    <% } %>
                    <%--            <a href="<%: Links.QuestionWithCategoryFilter(Url, Model.Category) %>" class="" rel="nofollow" style="font-style: italic; margin-left: 10px;">
                    <i class="fa fa-forward" style="color: #afd534;">&nbsp;</i>Alle <%: Model.CountAggregatedQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountAggregatedQuestions, "n") %> dieses Themas zeigen
                </a>--%>
                </div>
            </div>
        <% } else{ %> 
            Bisher gibt es keine Fragen zu diesem Thema.        
        <% } %>
                
    </div>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %> 


