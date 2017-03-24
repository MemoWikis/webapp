class SolutionTypeMultipleChoice
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.MultipleChoice;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);
    }

    static GetChosenAnswers(): string {
        var selected = $('input:checkbox[name=answer]:checked');
        var selectedValues = "";
        for (var i = 0; i < selected.length; i++) {
            selectedValues += (<any>selected.get(i)).value;
            if (i < (selected.length - 1))
                selectedValues += ", ";
        }
        return selectedValues;
    }

    GetAnswerText(): string {
        return SolutionTypeMultipleChoice.GetChosenAnswers();
    }

    GetAnswerData(): {} {
        return { answer: SolutionTypeMultipleChoice.GetChosenAnswers()};
    }
};