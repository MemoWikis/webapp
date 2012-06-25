<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<textarea id="txtAnswer" class="questionBlockWidth row" style="height: 30px;"></textarea>    

<script type="text/javascript">
    function getAnswerText() {
        return $("#txtAnswer").val();
    }
    function getAnswerData() {
        return { answer: $("#txtAnswer").val() };
    }
    function clearAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    }
</script>