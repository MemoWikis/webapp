<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>



<h4>Alle Inhalte</h4>
<div id="Content" class="Box">
    <h5 class="ContentSubheading Set">
        <%= Model.CountSets %> Lernset<%= StringUtils.PluralSuffix(Model.CountSets,"s") %> zu diesem Thema
        <a href="#aggregatedSetsList" data-toggle="collapse" class="greyed noTextdecoration" style="float: right; font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Lernsets ein-/ausblenden</a>
    </h5>
    <% if (Model.CountSets > 0) { %>    
        <div id="aggregatedSetsList" class="LabelList collapse in">
        <% foreach (var set in Model.AggregatedSets)
           { %>
            <div class="LabelItem LabelItem-Set">
                <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                <span style="font-size: 90%;">
                    (<%= set.QuestionsInSet.Count %> Frage<%= StringUtils.PluralSuffix(set.QuestionsInSet.Count, "n") %>)
                </span>                 
            </div>
        <% } %>
        </div>
        
    <% }
    else { %>
        Bisher gibt es keine Lernsets in diesem Thema.
    <% } %>

    <h5 class="ContentSubheading Question">
        <%= Model.CountAggregatedQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountAggregatedQuestions,"n") %> zu diesem Thema
        <a href="#aggregatedTopQuestionsList" data-toggle="collapse" class="greyed noTextdecoration" style="float: right; font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Fragen ein-/ausblenden</a>
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
