<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice_SingleSolution>" %>


<% var random = new Random();
   foreach (var choice in  Model.Choices.OrderBy(x => random.Next()))
    { %>
    <div class="radio">
        <label>
            <input type="radio" name="answer" value="<%: choice %>" /> <%: choice %> <br />
        </label>
    </div>
<% } %>