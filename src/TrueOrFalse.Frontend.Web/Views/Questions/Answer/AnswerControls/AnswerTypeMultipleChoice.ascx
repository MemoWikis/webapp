<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<% var random = new Random();
   foreach (var choice in Model.Choices.OrderBy(x => random.Next()))
   { %>
    <input type="radio" name="answer" value="<%: choice %>" /> <%: choice %> <br />
<% } %>

<script type="text/javascript">
    $('input:radio[name=answer]').change(function () {
        answerChanged();
    });
    function getAnswerText() {
        var selected = $('input:radio[name=answer]:checked');
        return selected.length ? selected.val() : "";
    }
    function getAnswerData() {
        return { answer: $('input:radio[name=answer]:checked').val() };
    }
    function newAnswer() {
        $('input:radio[name=answer]:checked').prop('checked', false);
    }
</script>