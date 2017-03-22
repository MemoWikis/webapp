class SolutionTypeMatchList
    extends AnswerEntryBase
    implements IAnswerEntry
{
    public SolutionType = SolutionType.MatchList;

    constructor(answerEntry: AnswerEntry) {
        super(answerEntry);
        this.AnswerQuestion = new AnswerQuestion(this);
        $('#AnswerInputSection .ui-draggable')
            .each((index, dragElement) => {
                $(dragElement)
                    .on("drag", (event, ui) => { this.AnswerQuestion.OnAnswerChange(); });
            });
    }

    static GetChosenAnswers(): string {
        if ($('#matchlist-mobilepairs').length) {
            var answerRowsMobile: Pair[] = [];
            var answerCountMobile = $('#matchlist-mobilepairs .matchlist-mobilepairrow')
                .each((index, element) => {
                    var leftPairValueMobile;
                    var rightPairValueMobile;
                    answerRowsMobile.push(new Pair());
                    answerRowsMobile[index].ElementLeft = new ElementLeft();
                    answerRowsMobile[index].ElementLeft.Text = leftPairValueMobile;
                    answerRowsMobile[index].ElementRight = new ElementRight();
                    answerRowsMobile[index].ElementRight.Text = rightPairValueMobile;
                });
            return JSON.stringify(answerRowsMobile);
        } else {
            var answerRows: Pair[] = [];
            var answerCount = $("#matchlist-pairs [id*='leftElementResponse-']")
                .each((index, elem) => {
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
    }

    GetAnswerText(): string {
        return "falsche Antwort";
    }

    GetAnswerData(): {} {
        return { answer: '{ "Pairs": ' + SolutionTypeMatchList.GetChosenAnswers() + '}'};
    }

    OnNewAnswer() {
        $('[id*="rightElementResponse-"]').each((index, element) => {
            $('#leftElementResponse-' + $(element).attr('id').split("-")[1]).removeAttr('id');
            $(element).remove();
        });
    }
}

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