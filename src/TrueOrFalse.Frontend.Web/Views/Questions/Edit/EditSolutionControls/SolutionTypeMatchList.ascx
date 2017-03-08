<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>

<div class="col-sm-12" id="matchlist">
    <div id="stems"></div>
    <div id="matchlist-button" class="row">
        <button type="button" class="btn col-sm-6" id="addStem">Add Stem</button>
        <button type="button" class="btn col-sm-4" id="responseModalButton" data-toggle="modal" data-target="#responseModal">Add Response</button>
    </div>
    <div id="responseModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Add Response</h4>
                </div>
                <div class="modal-body">
                    <div id="responseModalContent"></div>
                    <button type="button" id="addResponse">Add Response</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</div>


<script type="text/javascript">
    var addingStemId = $("#stems .form-control").length;
    var addingResponseId = $("#responseModal .form-control").length;

    function getRemoveButton() {
        var removeButton = $("<a href='#' class='removeButton btn'><i class='fa fa-times'></i></a>");
        removeButton.click(function () {
            $(this).closest(".form-group").hide(500, function () { $(this).remove(); });
        });
        return removeButton;
    }

    $("#addStem").click(function () {
        if (addingStemId != 0) {
        }
        $("#stems").append($("<div class = 'form-inline form-group'>")
            .append($("<input type='text' name = 'stem-" + addingStemId + "' class='matchlist-stem form-control'>"))
                .append($("<i class='matchlist-arrow fa fa-arrow-right fa-1x'></i>"))
                    .append($("<select class='matchlist-dropdown form-control' name='dropdown-" + addingStemId + "'>")
                        .append($('<option selected="selected">').val(0).html("no response")))
                            .append(getRemoveButton())
        );
        //fill select lists with elements in last select lists
        addingStemId++;

    });
    $("#addResponse").click(function () {
        var matchlistResponseInput = $("<input type='text' name = 'stem-" + addingResponseId + "' class='matchlist-response form-control'>");
        $("#responseModalContent").append($("<div class='form-group form-inline'>")
            .append(matchlistResponseInput)
            .append(getRemoveButton()));
        matchlistResponseInput.change(function () {
            var responseValue = $(this).val();
            $(".matchlist-dropdown").each(function(index, element) {
                $(element).append($('<option>').val(addingResponseId).html(/*something in here*/));
            });
        });
        addingResponseId++;
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