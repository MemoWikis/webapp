<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HistoryAndProbabilityModel>" %>

<% Html.RenderPartial("AnswerHistory", Model.AnswerHistory); %> <br/>
<% Html.RenderPartial("CorrectnessProbability", Model.CorrectnessProbability); %>

<% if(Model.LoadJs){ %>
    <script type="text/javascript">
        FillSparklineTotals();
        $('.show-tooltip').tooltip();
    </script>
<% } %>