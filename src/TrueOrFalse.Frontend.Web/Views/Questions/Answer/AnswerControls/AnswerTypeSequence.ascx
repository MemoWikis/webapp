<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionSequence>" %>

<% var i = 1;
    foreach (var row in Model.Rows)
   { %>
   <label for="row-<%:i %>"><%:row.Key %></label> <%:Html.TextBox("row-" + i, null, new {@class = "sequence-row"})  %> <br />
<% } %>

<script type="text/javascript">
    $('.sequence-row').keydown(function () {
        answerChanged();
    });
    function getAnswerText() {
        return $.map($('.sequence-row'), function(x) {
            return $(x).val();
        }).join(", ");
    }
    function getAnswerData() {
        return {
            answer: JSON.stringify($.map($('.sequence-row'), function(x) {
                return $(x).val();
            }))
        };
    }
    function newAnswer() {
        $('.sequence-row').val(""); ;
    }
</script>