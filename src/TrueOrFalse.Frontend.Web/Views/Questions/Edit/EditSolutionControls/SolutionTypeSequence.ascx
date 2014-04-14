<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionSequence>" %>


<div id="rows"></div>
<div class="form-group">
    <div class="noLabel columnControlsFull ButtonContainer">
        <button class="btn" id="addRow">Zeile hinzufügen</button>
    </div>
</div>

<script type="text/javascript">
    var addingRowId = 1;

    var fnAddRow = function() {
        $("#rows").append(
            "<div class='form-group'>" +
            "   <div class='noLabel columnControlsFull'>" +
            "       <input type='text' class='form-control sequence-key' name='key-" + addingRowId + "' />: " +
            "       <input type='text' class='form-control sequence-value' name='value-" + addingRowId + "' />" +
            "       <a href='#' onClick=\"$(this).parent().remove()\">[x]</a>" + 
            "   </div>" +
            "</div>");
        addingRowId++;
    }
    $("#addRow").click(function () {
        fnAddRow();
        return false;
    });

    <% bool isNewQuestion = Model == null; %>

    <% if (isNewQuestion || (Model != null && Model.Rows.Count == 0)){ %>
        fnAddRow();
    <% } %> 

    <% if(!isNewQuestion) foreach (var row in Model.Rows) { %>
        fnAddRow();
        $(".sequence-key").last().val('<%:row.Key %>');
        $(".sequence-value").last().val('<%:row.Value %>');
    <% }%>
</script>
