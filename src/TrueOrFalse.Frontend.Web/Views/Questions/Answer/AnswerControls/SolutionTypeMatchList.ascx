<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div class="col-sm-12">
    <div class="row">
        <div class="col-sm-12">
            <div id="matchlist-pairs" class="row"></div>
        </div>
        <div class="col-sm-12">
            <div id="matchlist-rightelements" class="row"></div>
        </div>
    </div>
</div>

<script type="text/javascript">
    <% var random = new Random();
    foreach (var pair in Model.Pairs.OrderBy(x => random.Next()))
    { %>
    var rightDropElement = $("<div class='matchlist-droppable col-sm-5' name = '<%= pair.ElementLeft.Text %>'>").droppable({
        accept: '.matchlist-rightelement',
        hoverClass: 'matchlist-hovered',
        drop: handleElementDrop
    });
    $("div#matchlist-pairs").append($("<div class='col-sm-12'>").append($("<div class = 'row matchlist-pairrow'>")
        .append($("<span class= 'col-sm-5'><%= pair.ElementLeft.Text %></span>"))
        .append($("<i class=' matchlist-arrow fa fa-arrow-right fa-1x col-sm-2'>"))
        .append(rightDropElement)));
    <% }

    foreach (var elementRight in Model.RightElements.OrderBy(x => random.Next()))
    { %>
    var rightDragElement = $("<span class='matchlist-rightelement' name='<%= elementRight.Text %>'>").html("<%= elementRight.Text %>").draggable({
        containment: '#AnswerBody',
        stack: '.matchlist-rightelement',
        cursor: 'move',
        helper: 'clone',
        revert: 'true'
    });
    $("#matchlist-rightelements").append(rightDragElement);
    <% } %>

    var answerCount = 0;
    function handleElementDrop(event, ui) {
        ui.helper.data('dropped', true);
        if ($(this).attr('id') !== undefined)
            if ($('#rightElementResponse-' + $(this).attr('id').split("-")[1]).attr("id") !== $(ui.draggable).attr("id"))
                $('#rightElementResponse-' + $(this).attr('id').split("-")[1]).remove();
        if (ui.draggable.hasClass('helper-clone')) {
            $(this).append(ui.draggable);
            ui.draggable.position({ of: $(this), my: 'center', at: 'center' });
            var oldIdElementLeft = 'leftElementResponse-' + ui.draggable.attr('id').split("-")[1];
            $('#' + oldIdElementLeft).removeAttr('id');
            $(this).attr('id', oldIdElementLeft);
        } else {
            var helperClone = ui.helper.clone();
            helperClone.draggable({
                containment: '#AnswerBody',
                stack: '.matchlist-rightelemen',
                cursor: 'move',
                start: function(event, ui) {
                    ui.helper.data('dropped', false);
                },
                stop: function (event, ui) {
                    if (!ui.helper.data('dropped')) {
                        $('#leftElementResponse-' + $(this).attr('id').split("-")[1]).removeAttr('id');
                        $(this).remove();
                    }
                }
            });
            helperClone.addClass('helper-clone');
            $(this).append(helperClone);
            helperClone.css({height: "inherit", width: "105%" });
            helperClone.position({ of: $(this), my: 'center', at: 'center' });
            if ($(this).attr('id') !== undefined) {
                helperClone.attr('id', 'rightElementResponse-' + $(this).attr('id').split("-")[1]);
            } else {
                helperClone.attr('id', 'rightElementResponse-' + answerCount);
                $(this).attr('id', 'leftElementResponse-' + answerCount);
            }
            answerCount++;
        }
    }
</script>
