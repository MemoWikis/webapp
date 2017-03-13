<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div id="matchListDropDown">
    <div id="pairs"></div>
    <div id="rightElements"></div>
</div>

<script type="text/javascript">
    <% var random = new Random();
   foreach (var pair in Model.Pairs.OrderBy(x => random.Next()))
   { %>
    var rightDropElement = $("<div id = 'rightDrop-<%= pair.ElementRight.Text %>'>").droppable( {
        accept: '#rightElements div',
        hoverClass: 'hovered',
        drop: function () {} //handleCardDrop
    } );
    $("div#pairs")
        .append($("<h3><%= pair.ElementLeft.Text %>"))
        .append($("<i class='matchlist-arrow fa fa-arrow-right fa-1x'>"))
        .append(rightDropElement);
    <% }

    foreach (var elementRight in Model.RightElements.OrderBy(x => random.Next()))
    { %>
    var rightDragElement = $("<div id = '<%= elementRight.Text %>'>").html("<%= elementRight.Text %>").draggable({
        containment: '#AnswerInputSection',
        stack: '#rightElements div',
        cursor: 'move',
        revert: true
    });
    $("#rightElements").append(rightDragElement);
    <% } %>
</script>