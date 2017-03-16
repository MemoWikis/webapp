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
        });
    }

    static GetChosenAnswers(): string {
        var answerRows: Pair[] = [];
        var answerCount = $("#matchlist-pairs [id*='leftElementResponse-']").each((index, elem) => {
            var rightPairValue = $("#rightElementResponse-" + $(elem).attr('id').split("-")[1]).attr('name');
            var leftPairValue = $(elem).attr('name');
            answerRows.push(new Pair());
            answerRows[index].ElementLeft = new ElementLeft();
            answerRows[index].ElementLeft.Text = leftPairValue;
            answerRows[index].ElementRight = new ElementRight();
            answerRows[index].ElementRight.Text = rightPairValue;
        });        
        return JSON.stringify(answerRows);
    }

    GetAnswerText(): string {
        return "Note to self: Cannot be empty string. Still have to fix that!";
    }

    GetAnswerData(): {} {
        return { answer: '{ "Pairs": ' + SolutionTypeMatchList.GetChosenAnswers() + '}'};
    }

    OnNewAnswer() {
        $('input:checkbox[name=answer]:checked').prop('checked', false);
    }
};

class Pair {
    ElementLeft: ElementLeft;
    ElementRight: ElementRight;
}

class ElementLeft {
    Text: string;
}

class ElementRight {
    Text: string;
}