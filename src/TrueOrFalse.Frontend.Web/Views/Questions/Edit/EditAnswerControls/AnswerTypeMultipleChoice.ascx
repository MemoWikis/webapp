<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>

<div id="choices">
    
</div>

<button id="addChoice">Antwort hinzufügen</button>

<script type="text/javascript">
    var addingChoiceId = 1;
    $("#addChoice").click(function () {
        $("#choices").append("<div class='control-group'><input type='text' class='sequence-choice' name='choice-" + addingChoiceId + "' /></div>");
        addingChoiceId++;
        return false;
    });
<% if(Model != null) foreach (var choice in Model.Choices) { %>
    $("#addChoice").click();
    $(".sequence-choice").last().val('<%:choice %>');
<% } %>
</script>
