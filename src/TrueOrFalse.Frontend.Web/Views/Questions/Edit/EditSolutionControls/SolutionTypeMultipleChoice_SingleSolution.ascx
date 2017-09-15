<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice_SingleSolution>" %>


<div id="choices">
</div>

<div class="form-group">
    <div class="noLabel columnControlsFull ButtonContainer">
        <button class="btn" id="addChoice">weitere falsche Antwort hinzufügen</button>
    </div>
</div>

<script type="text/javascript">
    var addingChoiceId = $("#choices .form-group").length;
    $("#addChoice").click(function () {

        var label = "";
        if (addingChoiceId == 0)
            label = "richtige Antwort";
        else
            label = "falsche Antwort (" + (addingChoiceId) + ")";

        var actionButton = $("");
        if(addingChoiceId != 0) {
            actionButton = $("<span class='CloseButton input-group-btn'><a href='#' class='btn'><i class='fa fa-times'></i></a></span>");
            actionButton.click(function() {
                $(this).closest(".form-group").hide(500, function () { $(this).remove(); });
                return false;
            });
        }

        $("#choices")
            .append($("<div class='form-group'>")
                .append($("<div class='noLabel columnControlsFull'>")
                    .append($("<div class='input-group'><span class='input-group-addon'>" + label + ":</span>")
                        .append($("<input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' />"), 
                            actionButton
                        )
                    )
                )
            );

        addingChoiceId++;
        return false;

    });
<% if (Model != null)
   {
       foreach (var choice in Model.Choices)
       { %>
        $("#addChoice").click();
        $(".sequence-choice").last().val('<%= choice %>');
        <% }
   }
   else
   { %>
       $("#addChoice").click();
       $("#addChoice").click();
<% } %>
</script>
