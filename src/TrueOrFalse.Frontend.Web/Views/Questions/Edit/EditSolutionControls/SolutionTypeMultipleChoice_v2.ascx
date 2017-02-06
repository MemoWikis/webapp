<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>


<%--Impelemtation of the bootstrap-toggle plugin
<link href="/Style/vendor/bootstrap2-toggle.css" rel="stylesheet">
<script type="text/javascript" src="/Scripts/vendor/bootstrap2-toggle.js"></script>
<input id="toggle-one" checked type="checkbox">
<script type="text/javascript">
  $(function() {
    $('#toggle-one').bootstrapToggle();
  })
</script>--%>



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
                .append($("<div class='noLabel columnControlsFull input-group'>")
                    .append($("<div class=''>")
                                .append($("<select class='form-control'><option>Richtige Antwort</option><option selected='selected'>Falsche Antwort</option></select>")),
                            $("<input type='text' class='sequence-choice form-control' name='choice-" + addingChoiceId + "' />"),
                            actionButton
                    )
                )
            );

        addingChoiceId++;
        return false;

    });
<% if (Model != null)
       foreach (var choice in Model.Choices){ %>
        $("#addChoice").click();
        $(".sequence-choice").last().val('<%= choice %>');
<% }else { %>
       $("#addChoice").click();
       $("#addChoice").click();
<% } %>
</script>