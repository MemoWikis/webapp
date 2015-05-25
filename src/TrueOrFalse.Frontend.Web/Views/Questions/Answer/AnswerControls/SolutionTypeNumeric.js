var SolutionTypeNumeric = (function () {
    function SolutionTypeNumeric() {
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(function () {
            answerQuestion.OnAnswerChange();
        });
    }
    SolutionTypeNumeric.prototype.GetAnswerText = function () {
        return $("#txtAnswer").val();
    };

    SolutionTypeNumeric.prototype.GetAnswerData = function () {
        return { answer: $("#txtAnswer").val() };
    };

    SolutionTypeNumeric.prototype.OnNewAnswer = function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();
    };
    return SolutionTypeNumeric;
})();
//# sourceMappingURL=SolutionTypeNumeric.js.map
