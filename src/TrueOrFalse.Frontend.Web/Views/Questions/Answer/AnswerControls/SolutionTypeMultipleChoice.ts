class SolutionTypeMultipleChoice
    extends AnswerEntryBase
    implements IAnswerEntry
{
    AnswerQuestion: AnswerQuestion;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);

        $('input:radio[name=answer]').change(() => {
            this.AnswerQuestion.OnAnswerChange();
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