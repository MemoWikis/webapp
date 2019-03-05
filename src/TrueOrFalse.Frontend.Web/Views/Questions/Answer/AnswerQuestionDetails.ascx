<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="row question-details">
    <div class="col-lg-6">
        <span class=" category-set">
            <span id="Category">
                <% if (Model.Categories.Count > 0)
                    { %>
                <span>Thema:&nbsp;</span> <% Html.RenderPartial("CategoriesOfQuestion", Model.Question); %>

                <% } %>

                <% if (Model.SetMinis.Count > 0)
                    { %>
                <% foreach (var setMini in Model.SetMinis)
                    { %>
                <a class="set" href="<%= Links.SetDetail(Url, setMini) %>"><span class="label label-set"><%: setMini.Name %></span></a>
                <% } %>
                <% if (Model.SetCount > 5)
                    { %>
                <a class="margin-left" href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount - 5 %> weitere </a>
                <% } %>

                <% } %>
                <span class="fa fa-chevron-right"></span>
            </span>
        </span>
    </div>
    <div class="col-lg-6 second-row">
        <% if (Model.HistoryAndProbability.QuestionValuation.IsInWishKnowledge())
            {
                var status = Model.HistoryAndProbability.QuestionValuation.KnowledgeStatus; %>
        <span class="learning-status">
            <span style="background-color: <%= status.GetColor() %>; font-size: 12px; padding: 2px 4px; -ms-border-radius: 5px; border-radius: 5px; width: 100%;">
                <%= status.GetText() %>
            </span>
        </span>
        <% } %>
        <span class="show-tooltip" title="Insgesamt <%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal%>x beantwortet"><%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredTotal%>x </span>
        <span class="sparklineTotals" data-answerstrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredCorrect %>" data-answersfalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredWrongTotal %>"></span>

        <span class="show-tooltip" title="Von dir <%=Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser%>x beantwortet">ich: <%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUser%>x </span>
        <span class="sparklineTotalsUser" data-answerstrue="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserTrue  %>" data-answersfalse="<%= Model.HistoryAndProbability.AnswerHistory.TimesAnsweredUserWrong %>"></span>
        <span class="correctness-prohability">
            <% Html.RenderPartial("~/Views/Shared/CorrectnessProbability.ascx", Model.HistoryAndProbability.CorrectnessProbability); %>             
        </span>
        <span class="show-tooltip seen" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
            <span><i class="fa fa-eye greyed"></i><%= Model.TotalViews %>x </span>
        </span>
        <span class="show-tooltip margin-left" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
            <i class="fa fa-heart greyed"></i>
            <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span>
        </span>
    </div>
</div>
