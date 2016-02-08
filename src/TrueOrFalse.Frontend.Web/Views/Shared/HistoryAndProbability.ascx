<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HistoryAndProbabilityModel>" %>

<% if(Model.QuestionValuation.IsInWishKnowledge()) { 
       var status = Model.QuestionValuation.KnowledgeStatus;
%>
    <div class="StatsRow" style="font-size: 13px; margin-bottom: 4px;">
        <div style="background-color: <%= status.GetColor() %>; padding: 2px; padding-left: 4px; -ms-border-radius: 2px; border-radius: 3px;">
            <%= status.GetText() %>
        </div>
    </div>
<% } %>
<div class="StatsRow">
    <% Html.RenderPartial("AnswerHistory", Model.AnswerHistory); %> 
</div>
<div class="StatsRow">
    <% Html.RenderPartial("CorrectnessProbability", Model.CorrectnessProbability); %>
</div>

<% if(Model.LoadJs){ %>
    <script type="text/javascript">
        FillSparklineTotals();
        $('.show-tooltip').tooltip();
    </script>
<% } %>