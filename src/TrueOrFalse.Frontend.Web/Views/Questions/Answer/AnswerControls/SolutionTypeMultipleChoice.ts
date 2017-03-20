class SolutionTypeMultipleChoice
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.MultipleChoice;

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
                selectedValues += "%seperate&xyz%";
        }
        return selectedValues;
    }

    GetAnswerText(): string {
        var answerText = SolutionTypeMultipleChoice.GetChosenAnswers().split("%seperate&xyz%").join("</br>");
        return answerText;
    }

    GetAnswerData(): {} {
        return { answer: SolutionTypeMultipleChoice.GetChosenAnswers()};
    }

    OnNewAnswer() {
        $('input:checkbox[name=answer]:checked').prop('checked', false);
    }
};