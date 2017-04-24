<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice_SingleSolution>" %>


<% var localChoices = Model.Choices;
    if (!Model.isSolutionOrdered)
        localChoices = Model.Choices.OrderBy(x => new Random().Next()).ToList();

    foreach (var choice in localChoices)
    { %>
    <div class="radio">
        <label>
            <input type="radio" name="answer" value="<%: choice %>" /> <%: choice %> <br />
        </label>
    </div>
<% } %>