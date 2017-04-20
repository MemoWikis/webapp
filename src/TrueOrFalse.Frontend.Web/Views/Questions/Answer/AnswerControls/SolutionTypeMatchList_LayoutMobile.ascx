<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>
<div id="matchlist-mobilepairs" class="col-sm-12"></div>   

<style>
#AnswerAndSolutionCol {
    padding-left: 0px;
}
#AnswerAndSolutionCol #AnswerInputSection {
    padding-left: 0px;
}
</style>

<script type="text/javascript">
    <% var random = new Random();
   int i = 0;
    foreach (var pair in Model.Pairs.OrderBy(x => random.Next()))
    { %>
    $('#matchlist-mobilepairs').append($('<div class="matchlist-mobilepairrow col-sm-12 row form-group" id="matchlist-mobilepairrow-<%= i %>">')
        .append($('<div class="col-sm-12 control-label matchlist-elementlabel" id="matchlist-elementlabel-<%= i %>"><%= pair.ElementLeft.Text %></div>'))
        .append($('<div class="col-sm-12">').append($('<select class="form-control matchlist-mobileselect" id="matchlist-select-<%= i %>">').append($('<option>Keine Zuordnung</option>'))))
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