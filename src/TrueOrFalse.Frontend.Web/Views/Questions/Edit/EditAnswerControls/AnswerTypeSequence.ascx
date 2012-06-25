<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionSequence>" %>

<div id="rows">
    
</div>

<button id="addRow">Zeile hinzufügen</button>

<script type="text/javascript">
    var addingRowId = 1;
    $("#addRow").click(function () {
        $("#rows").append("<div class='control-group'><input type='text' class='sequence-key' name='key-" + addingRowId + "' />: <input type='text' class='sequence-value' name='value-" + addingRowId + "' /></div>");
        addingRowId++;
        return false;
    });
<% if(Model != null) foreach (var row in Model.Rows) { %>
    $("#addRow").click();
    $(".sequence-key").last().val('<%:row.Key %>');
    $(".sequence-value").last().val('<%:row.Value %>');
<% } %>
</script>
