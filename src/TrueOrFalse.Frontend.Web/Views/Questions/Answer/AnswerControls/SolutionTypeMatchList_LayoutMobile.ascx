<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div id="matchlist-mobilepairs"></div>   

<script type="text/javascript">
    <% var localChoices = Model.Pairs;

    if (!Model.isSolutionOrdered)
    {
        var random = new Random();
        localChoices = Model.Pairs.OrderBy(x => random.Next()).ToList();
    }

    int i = 0;
    foreach (var pair in localChoices)
    { %>
    $('#matchlist-mobilepairs').append($('<div class="matchlist-mobilepairrow form-group" id="matchlist-mobilepairrow-<%= i %>">')
        .append($('<div class="matchlist-elementlabel" id="matchlist-elementlabel-<%= i %>"><%= pair.ElementLeft.Text %></div>'))
        .append($('<select class="form-control matchlist-mobileselect" id="matchlist-select-<%= i %>">').append($('<option>Keine Zuordnung</option>')))
    );
    <% i++;
    }

    foreach (var rightElement in Model.RightElements)
    { %>
    $('select[id*="matchlist-select-"]').each(function (index, element) {
        $(element).append($('<option><%= rightElement.Text %></option>'));
    });
    <% } %>
</script>