class SolutionTypeMultipleChoice implements ISolutionEntry
{
    constructor() {
        var answerQuestion = new AnswerQuestion(this);
        $('input:radio[name=answer]').change(function () {
            answerQuestion.OnAnswerChange();
        });
    }

    GetAnswerText(): string {
        var selected = $('input:radio[name=answer]:checked');
        return selected.length ? selected.val() : "";
    }

    GetAnswerData(): {} {
        return { answer: $('input:radio[name=answer]:checked').val() };
    }

    OnNewAnswer() {
        $('input:radio[name=answer]:checked').prop('checked', false);
    }
};

$(function() {
    new SolutionTypeMultipleChoice();
});