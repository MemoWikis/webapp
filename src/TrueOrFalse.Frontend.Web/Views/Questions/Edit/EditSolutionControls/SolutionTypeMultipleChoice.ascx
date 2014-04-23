<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<div id="choices">
    <div class='form-group'>
        <label class="columnLabel control-label">Antwortvorgaben</label>
        <div class='columnControlsFull'>
            <div class='input-group'>
                <span class='input-group-addon'>richtige Antwort:</span>
                <input type='text' class='sequence-choice form-control' name='choice-1' />
            </div>
        </div>
    </div>
    <div class='form-group'>
        <div class='noLabel columnControlsFull'>
            <div class='input-group'>
                <span class='input-group-addon'>falsche Antwort 1:</span>
                <input type='text' class='sequence-choice form-control' name='choice-2' />
            </div>
        </div>
    </div>
</div>
<div class="form-group">
    <div class="noLabel columnControlsFull ButtonContainer">
        <button class="btn" id="addChoice">Antwort hinzufügen</button>
    </div>
</div>


<script type="text/javascript">
    var addingChoiceId = 3;
    $("#addChoice").click(function () {
        //$("#choices").append("<div class='form-group'><div class='noLabel columnControlsFull'><input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' /></div></div>");

        $("#choices").append("<div class='form-group'><div class='noLabel columnControlsFull'><div class='input-group'><span class='input-group-addon'>falsche Antwort " + (addingChoiceId - 1) + ":</span><input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' /></div></div></div>");
        addingChoiceId++;
        return false;
    });
<% if(Model != null) foreach (var choice in Model.Choices) { %>
    $("#addChoice").click();
    $(".sequence-choice").last().val('<%:choice %>');
<% } %>
</script>
