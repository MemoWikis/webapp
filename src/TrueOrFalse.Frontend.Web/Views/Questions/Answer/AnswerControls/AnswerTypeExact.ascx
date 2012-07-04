<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<textarea id="txtAnswer" class="questionBlockWidth row" style="height: 30px;"></textarea>    

<script type="text/javascript">
    $("#txtAnswer").keypress(function () {
        answerChanged();
    });
    function getAnswerText() {
        return $("#txtAnswer").val();
    }
    function getAnswerData() {
        return { answer: $("#txtAnswer").val() };
    }
    function newAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    }
</script>