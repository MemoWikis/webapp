<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HistoryAndProbabilityModel>" %>

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