class SolutionTypeMultipleChoice_SingleSolution
    extends AnswerEntryBase
    implements IAnswerEntry
{

    public SolutionType = SolutionType.MultipleChoice_SingleSolution;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);

    }

    GetAnswerText(): string {
        var selected = $('input:radio[name=answer]:checked');
        return selected.length ? selected.val() : "";
    }

    GetAnswerData(): {} {
        return { answer: $('input:radio[name=answer]:checked').val() };
    }
};