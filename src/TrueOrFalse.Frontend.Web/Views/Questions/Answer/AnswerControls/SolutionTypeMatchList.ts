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
        var answerCount = $("#matchlist-pairs [id*='leftElementResponse-']").length;
        var answerRows: Pair[] = [];
        for (var i = 0; i < answerCount; i++) {
            var rightPairValue = $("#rightElementResponse-" + i).attr('name');
            var leftPairValue = $("#leftElementResponse-" + i).attr('name');
            answerRows.push(new Pair());
            answerRows[i].ElementLeft = new ElementLeft();
            answerRows[i].ElementLeft.Text = leftPairValue;
            answerRows[i].ElementRight = new ElementRight();
            answerRows[i].ElementRight.Text = rightPairValue;
        }
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