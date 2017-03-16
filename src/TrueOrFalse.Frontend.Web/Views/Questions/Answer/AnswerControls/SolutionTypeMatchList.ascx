<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div class="col-sm-12 row">
    <div id="matchlist-pairs" class="col-sm-12 row"></div>
    <div id="matchlist-rightelements" class="col-sm-12 row"></div>
</div>

<script type="text/javascript">
    <% var random = new Random();
    foreach (var pair in Model.Pairs.OrderBy(x => random.Next()))
    { %>
    var rightDropElement = $("<div class='matchlist-dropable col-sm-5' name = '<%= pair.ElementLeft.Text %>'>").droppable({
        accept: '#matchlist-rightelements span',
        hoverClass: 'matchlist-hovered',
        drop: handleElementDrop
    });
    $("div#matchlist-pairs").append($("<div class = 'col-sm-12 row pair-row'>")
        .append($("<span class= 'col-sm-5'><%= pair.ElementLeft.Text %></span>"))
        .append($("<i class=' matchlist-arrow fa fa-arrow-right fa-1x col-sm-2'>"))
        .append(rightDropElement));
    <% }

    foreach (var elementRight in Model.RightElements.OrderBy(x => random.Next()))
    { %>
    var rightDragElement = $("<span class='matchlist-rightelement' name='<%= elementRight.Text %>'>").html("<%= elementRight.Text %>").draggable({
        containment: '#AnswerInputSection',
        stack: '#matchlist-rightelements span',
        cursor: 'move',
        helper: 'clone',
        revert: 'true'
    });
    $("#matchlist-rightelements").append(rightDragElement);
    <% } %>

    var answerCount = 0;
    function handleElementDrop(event, ui) {
        if ($(this).attr('id') !== undefined) {
            $('#rightElementResponse-' + $(this).attr('id').split("-")[1]).remove();
        }
        if (ui.draggable.hasClass('helper-clone')) {
            ui.draggable.position({ of: $(this), my: 'center', at: 'center' });
            var oldIdElementLeft = 'leftElementResponse-' + ui.draggable.attr('id').split("-")[1];
            $('#' + oldIdElementLeft).removeAttr('id');
            $(this).attr('id', oldIdElementLeft);
        } else {
            var helperClone = ui.helper.clone();
            helperClone.draggable({
                containment: '#AnswerInputSection',
                stack: '#matchlist-rightelements span',
                cursor: 'move',
                stop: function (event, ui) {
                    //TODO Continue here
                    alert($(this).data('dropped', false));
                    //if (ui.draggable.data('dropped', false)) {
                    //    alert('false');
                    //}
                }
            });
            helperClone.addClass('helper-clone');
            ui.helper.before(helperClone);
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
