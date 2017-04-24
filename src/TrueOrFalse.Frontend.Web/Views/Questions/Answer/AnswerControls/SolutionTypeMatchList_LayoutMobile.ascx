<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div id="matchlist-mobilepairs"></div>   

<script type="text/javascript">
    <% var localPairs = Model.Pairs;
    if (!Model.isSolutionOrdered)
        localPairs = Model.Pairs.OrderBy(x => new Random().Next()).ToList();

    int i = 0;
    foreach (var pair in localPairs)
    { %>
    $('#matchlist-mobilepairs').append($('<div class="matchlist-mobilepairrow form-group" id="matchlist-mobilepairrow-<%= i %>">')
        .append($('<div class="matchlist-elementlabel" id="matchlist-elementlabel-<%= i %>"><%= pair.ElementLeft.Text %></div>'))
        .append($('<select class="form-control matchlist-mobileselect" id="matchlist-select-<%= i %>">').append($('<option>Keine Zuordnung</option>')))
    );
    <% i++;
    }
    var localRightElements = Model.RightElements;
    if (!Model.isSolutionOrdered)
        localRightElements = Model.RightElements.OrderBy(x => new Random().Next()).ToList();

    foreach (var rightElement in localRightElements)
    { %>
    $('select[id*="matchlist-select-"]').each(function (index, element) {
        $(element).append($('<option><%= rightElement.Text %></option>'));
    });
    <% } %>
</script>