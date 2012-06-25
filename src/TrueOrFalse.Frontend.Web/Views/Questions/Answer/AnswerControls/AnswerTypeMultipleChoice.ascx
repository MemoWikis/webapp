<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerTypeMulitpleChoiceModel>" %>


<% var random = new Random();
   foreach (var choice in Model.Choices.OrderBy(x => random.Next()))
   { %>
    <input type="radio" name="answer" value="<%: choice %>" /> <%: choice %> <br />
<% } %>

<script type="text/javascript">
    function getAnswerText() {
        return $('input:radio[name=answer]:checked').val();
    }
    function getAnswerData() {
        return { answer: $('input:radio[name=answer]:checked').val() };
    }
</script>