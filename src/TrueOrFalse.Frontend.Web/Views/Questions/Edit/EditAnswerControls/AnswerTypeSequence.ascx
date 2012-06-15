<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerTypeSequenceModel>" %>

<div id="rows">
    
</div>

<button id="addRow">Zeile hinzufügen</button>

<script type="text/javascript">
    $("#addRow").click(function() {
        $("#rows").append("<div class='control-group'><input type='text' />: <input type='text' /></div>");
        return false;
    });
</script>
