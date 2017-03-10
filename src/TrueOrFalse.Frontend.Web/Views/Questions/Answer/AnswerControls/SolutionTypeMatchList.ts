class SolutionTypeMatchList
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.MatchList;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);

        this.AnswerQuestion = new AnswerQuestion(this);
        $('input:checkbox[name=answer]').change((event) => {
            this.AnswerQuestion.OnAnswerChange();
            this.AnswerQuestion.GiveSelectedSolutionClass(event);
        });
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

    OnNewAnswer() {
        $('input:checkbox[name=answer]:checked').prop('checked', false);
    }
};