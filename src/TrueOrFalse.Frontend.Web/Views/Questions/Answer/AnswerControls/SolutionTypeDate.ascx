<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

Date
<textarea id="txtAnswer" class="form-control " rows="1" style=" width: 100%"></textarea>    

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

<script src="/Views/Questions/Answer/AnswerControls/SolutionTypeDate.js" type="text/javascript"></script>
