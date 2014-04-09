<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionSequence>" %>


<div id="rows"></div>
<div class="form-group">
    <div class="xxs-stack col-xs-offset-2 col-xs-10 ButtonContainer">
        <button class="btn" id="addRow">Zeile hinzufügen</button>
    </div>
</div>

<script type="text/javascript">
    var addingRowId = 1;
    $("#addRow").click(function () {
        $("#rows").append("<div class='form-group'><div class='xxs-stack col-xs-offset-2 col-xs-10'><input type='text' class='form-control sequence-key' name='key-" + addingRowId + "' />: <input type='text' class='form-control sequence-value' name='value-" + addingRowId + "' /></div></div>");

        addingRowId++;
        return false;
    });
<% if(Model != null) foreach (var row in Model.Rows) { %>
    $("#addRow").click();
    $(".sequence-key").last().val('<%:row.Key %>');
    $(".sequence-value").last().val('<%:row.Value %>');
<% } %>
</script>
