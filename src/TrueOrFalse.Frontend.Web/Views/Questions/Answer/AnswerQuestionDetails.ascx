<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="separationBorderTop col-xs-12" style="min-height: 20px;"></div>
</div>
<div class="row">
    <div class="col-xs-12">  
        <% if (Model.Categories.Count > 0)
           { %>
            <div class="margin-left-first float-left">
                <% Html.RenderPartial("CategoriesOfQuestion", Model.Question); %>
            </div>
        <% } %>
    
        <% if (Model.SetMinis.Count > 0)
           { %>
            <% foreach (var setMini in Model.SetMinis)
               { %>
                <a class="margin-left" href="<%= Links.SetDetail(Url, setMini) %>"><span class="label label-set"><%: setMini.Name %></span></a>
            <% } %>
    
            <% if (Model.SetCount > 5)
               { %>
                <div style="margin-top: 3px;">
                    <a class="margin-left" href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount - 5 %> weitere </a>
                </div>
            <% } %>

        <% } %>
        <div class="fa fa-chevron-right margin-left"></div>     
        <span class="float-right">
        <% if(Model.HistoryAndProbability.QuestionValuation.IsInWishKnowledge()) { 
               var status = Model.HistoryAndProbability.QuestionValuation.KnowledgeStatus; %>
            
                <span style="background-color: <%= status.GetColor() %>;  font-size: 13px;  padding: 2px 4px; -ms-border-radius: 5px; border-radius: 5px; width: 100%;">
                    <%= status.GetText() %>
                </span>
            
        <% } %>
        <span class="show-tooltip margin-left-20" title="Insgesamt <%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal%>x beantwortet"><%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal%>x </span>
        <span class="sparklineTotals" data-answersTrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredCorrect %>" data-answersFalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredWrongTotal %>"></span>
        
        <span class="show-tooltip" title="Von dir <%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser%>x beantwortet">  ich: <%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser%>x </span>
        <span class="sparklineTotalsUser" data-answersTrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserTrue  %>" data-answersFalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserWrong %>"></span>
        <span class="margin-left-20">
            <% Html.RenderPartial("~/Views/Shared/CorrectnessProbability.ascx", Model.HistoryAndProbability.CorrectnessProbability); %>             
        </span>
        
            <span class="show-tooltip margin-left-20" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                <i class="fa fa-eye"></i> <%= Model.TotalViews %>x
            </span>
            <span class="show-tooltip margin-left" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
                <i class="fa fa-heart greyed"></i> 
                <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span>
            </span> 
        </span>
    </div>
</div>
