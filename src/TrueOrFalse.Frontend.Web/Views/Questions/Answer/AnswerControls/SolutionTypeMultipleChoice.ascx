<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<% var random = new Random();
   foreach (var choice in Model.Choices.OrderBy(x => random.Next()))
   { %>
    <input type="radio" name="answer" value="<%: choice %>" /> <%: choice %> <br />
<% } %>

<script src="/Views/Questions/Answer/AnswerControls/SolutionTypeMultipleChoice.js" type="text/javascript"></script> 