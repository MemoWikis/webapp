<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice_v2>" %>


<% var random = new Random();
   foreach (var choice in Model.Choices.OrderBy(x => random.Next()))
   { %>
    <div class="radio">
        <label>
            <input type="checkbox" name="answer" value="<%: choice.Text %>" /> <%: choice.Text %> <br />
        </label>
    </div>
<% } %>