<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>

<div class="col-sm-12" id="matchlist">
    <div id="stems"></div>
    <div id="matchlist-button" class="row">
        <button type="button" class="btn col-sm-6" id="addStem">Paar hinzufügen</button>
        <button type="button" class="btn col-sm-4" id="responseModalButton" data-toggle="modal" data-target="#responseModal">Rechte Elemente hinzufügen</button>
    </div>
    <div id="responseModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Rechte Elemente</h4>
                </div>
                <div class="modal-body">
                    <div id="responseModalContent"></div>
                    <button type="button" id="addResponse">Rechtes Element hinzufügen</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Schließen</button>
                </div>
            </div>
        </div>
    </div>
</div>


<script type="text/javascript">
    var addingElementRightId = $("#responseModal .form-control").length;
    var removeButton = $("");
    var defaultRightElement = $("");

    function getRemoveButton() {
        var removeButton = $("<a href='#' class='removeButton btn'><i class='fa fa-times'></i></a>");
        removeButton.click(function () {
            $(this).closest(".form-group").hide(500, function () { $(this).remove(); });
        });
        return removeButton;
    }

    $("#addStem").click(function() {
        //if (addingStemId != 0) {
        //    dont enable remove button for first element
        //}
        var addingPairElementId = $("#stems .form-control").length;
        if (addingPairElementId != 0)
            removeButton = getRemoveButton();
        else
            defaultRightElement = $('<option selected="selected">').html("default");

        $("#stems").append($("<div class = 'form-inline form-group'>")
            .append($("<input type='text' name = 'LeftElement-" + addingPairElementId + "' class='matchlist-leftelement form-control'>"))
                .append($("<i class='matchlist-arrow fa fa-arrow-right fa-1x'></i>"))
                    .append($("<select class='matchlist-rightpairelement form-control' name='RightPairElement-" + addingPairElementId + "'>")
                        .append(defaultRightElement, $(".matchlist-rightpairelement").last().children().clone()))
                            .append(removeButton));
     });
    $("#addResponse").click(function () {
        var matchlistResponseInput = $("<input type='text' name = 'RightElement-" + addingElementRightId + "' class='matchlist-response form-control'>");
        $("#responseModalContent").append($("<div class='form-group form-inline'>")
            .append(matchlistResponseInput)
            .append(getRemoveButton()));
        matchlistResponseInput.change(function () {
            var responseValue = $(this).val();
            $(".matchlist-dropdown").each(function(index, element) {
                $(element).append($('<option>').val(addingElementRightId).html(/*something in here*/));
            });
        });
        addingElementRightId++;
    });

<%--    <% if (Model != null)
        foreach (var choice in Model.bla)
        { %>
    $("#addChoice").click();
    $(".sequence-choice").last().val('<%= choice.Text %>');
    $(".sequence-answertype").last().val('<%= choice.IsCorrect ? "Richtige Antwort" : "Falsche Antwort" %>');
    <% }
    else
    { %>
    $("#addResponse").click();
    $("#addStem").click();
    <% } %>--%>
</script>