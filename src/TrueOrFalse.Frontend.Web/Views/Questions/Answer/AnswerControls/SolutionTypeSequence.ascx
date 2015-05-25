<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionSequence>" %>

<% var i = 1;
    foreach (var row in Model.Rows)
   { %>
   <label for="row-<%:i %>"><%:row.Key %></label> <%:Html.TextBox("row-" + i, null, new {@class = "sequence-row"})  %> <br />
<% } %>