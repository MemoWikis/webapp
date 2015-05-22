class SolutionTypeTextEntry implements ISolutionEntry
{
    constructor() {
        var answerQuestion = new AnswerQuestion(this);
        $("#txtAnswer").keypress(() => { answerQuestion.OnAnswerChange(); });    
    }

    GetAnswerText(): string {
        return $("#txtAnswer").val();
    }

    GetAnswerData(): {} {
        return { answer: $("#txtAnswer").val() };
    }

    OnNewAnswer() {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
        $("#txtAnswer").select();
    }
};

$(function() {
    new SolutionTypeTextEntry();
});