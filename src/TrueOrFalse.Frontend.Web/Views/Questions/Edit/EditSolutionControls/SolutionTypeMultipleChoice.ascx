<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<div id="choices">
</div>

<div class="form-group">
    <div class="noLabel columnControlsFull ButtonContainer">
        <button class="btn" id="addChoice">Antwort hinzufügen</button>
    </div>
</div>


<script type="text/javascript">
    var addingChoiceId = $("#choices .form-group").length;
    $("#addChoice").click(function () {

        var label = "";
        if (addingChoiceId == 0)
            label = "richtige Antwort";
        else
            label = "falsche Antwort " + (addingChoiceId);

        $("#choices").append(


            "<div class='form-group'>" +
            "   <div class='noLabel columnControlsFull'>" +
            "       <div class='input-group'><span class='input-group-addon'>" + label + ":</span>" +
            "           <input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' />" +
            "       </div>" +
            "   </div>" +
            "</div>");
        addingChoiceId++;
        return false;

    });
<% if (Model != null)
       foreach (var choice in Model.Choices){ %>
        $("#addChoice").click();
        $(".sequence-choice").last().val('<%: choice %>');
<% }else { %>
       $("#addChoice").click();
       $("#addChoice").click();
<% } %>
</script>
