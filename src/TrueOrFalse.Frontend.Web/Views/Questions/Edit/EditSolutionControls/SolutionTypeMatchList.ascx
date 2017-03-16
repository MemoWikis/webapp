<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>

<div class="col-sm-12" id="matchlist">
    <div id="pairs"></div>
    <div id="matchlist-button" class="row">
        <button type="button" class="btn col-sm-6" id="addPair">Paar hinzufügen</button>
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
                    <button type="button" id="addRightPairElement">Weiteres Element hinzufügen</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Speichern</button>
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

    $("#addPair").click(function() {
        var addingPairElementId = $("#pairs .matchlist-leftelement").length;
        if (addingPairElementId !== 0)
            removeButton = $(getRemoveButton());

        $("#pairs").append($("<div class = 'form-inline form-group'>")
            .append($("<input type='text' name = 'LeftElement-" + addingPairElementId + "' class='matchlist-leftelement form-control'>"))
                .append($("<i class='matchlist-arrow fa fa-arrow-right fa-1x'></i>"))
                    .append($("<select class='matchlist-rightpairelement form-control' name='RightPairElement-" + addingPairElementId + "'>")
                        .append($(".matchlist-rightpairelement").last().children().clone()))
                            .append(removeButton));
        if (addingPairElementId === 0)
            $("[name='RightPairElement-0']").append($('<option selected="selected">').html("keine Zuordnung"));
    });
    $("#addRightPairElement").click(function () {
        var matchlistRightElementInput = $("<input type='text' id='pairElementRight-" + addingElementRightId + "'name = 'RightElement-" + addingElementRightId + "' class='matchlist-rightelement form-control'>");
        var rightElementRemoveButton = getRemoveButton();
        $("#responseModalContent").append($("<div class='form-group form-inline'>")
            .append(matchlistRightElementInput)
            .append(rightElementRemoveButton));
        matchlistRightElementInput.change(function () {
            var rightElementValue = $(this).val();
            var rightElementId = $(this).attr('id');
            $(".matchlist-rightpairelement").each(function (selectElementIndex, selectElement) {
                $(selectElement).children().each(function(optionElementIndex, optionElement) {
                    if ($(optionElement).attr('name') === rightElementId)
                        $(optionElement).remove();
                });
                $(selectElement).append($('<option>').attr('name', rightElementId).html(rightElementValue));
            });
        rightElementRemoveButton.click(function() {
        $(".matchlist-rightpairelement").each(function (index, selectElement) {
            $(selectElement).children().each(function (optionElementIndex, optionElement) {
                if ($(optionElement).attr('name') === rightElementId)
                    $(optionElement).remove();
            });
        });
        });
        });
        addingElementRightId++;
    });

    <% if (Model != null)
       {
           foreach (var pair in Model.RightElements)
           { %>
            $("#addRightPairElement").click();
            $(".matchlist-rightelement").last().val('<%= pair.Text %>');
        <% }

           for (int i = 0; i < Model.Pairs.Count; i++)
           {
          %>$("#addPair").click();
            <% if (i == 0)
               {
                   foreach (var rightElement in Model.RightElements)
                   { %>
                        $(".matchlist-rightpairelement").last().append($('<option>').html('<%= rightElement.Text %>'));
                <% }
               } %>
            $(".matchlist-leftelement").last().val('<%= Model.Pairs[i].ElementLeft.Text %>');
            $(".matchlist-rightpairelement").last().val('<%= Model.Pairs[i].ElementRight.Text %>');
        <% }
       }
       else
       { %>
    $("#addPair").click();
    $("#addRightPairElement").click();
    <% } %>
</script>