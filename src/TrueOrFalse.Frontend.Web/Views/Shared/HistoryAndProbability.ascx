<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HistoryAndProbabilityModel>" %>

<div style="padding-bottom: 2px;">
    <% Html.RenderPartial("AnswerHistory", Model.AnswerHistory); %> 
</div>
<% Html.RenderPartial("CorrectnessProbability", Model.CorrectnessProbability); %>

<% if(Model.LoadJs){ %>
    <script type="text/javascript">
        FillSparklineTotals();
        $('.show-tooltip').tooltip();
    </script>
<% } %>