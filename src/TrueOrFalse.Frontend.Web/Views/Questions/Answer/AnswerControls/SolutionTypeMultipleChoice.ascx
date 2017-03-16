<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<% var random = new Random();
   foreach (var choice in Model.Choices.OrderBy(x => random.Next()))
   { %>
    <div class="radio">
        <label>
            <input type="checkbox" name="answer" value="<%: choice.Text %>" /> <%: choice.Text %> <br />
        </label>
    </div>
<% } %>
<br/>
<h6 class = "ItemInfo">
    Es können keine oder mehrere Antworten richtig sein!
</h6>
<script type="text/javascript">
   

</script>