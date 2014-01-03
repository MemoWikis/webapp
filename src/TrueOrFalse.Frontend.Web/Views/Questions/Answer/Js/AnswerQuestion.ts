/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/mm.utils.ts" />

var answerResult;

var answerHistory = [];
var amountOfTries = 0;

class  AnswerQuestion
{
    private _getAnswerText: () => string;
    private _getAnswerData: () => {};
    private _onNewAnswer : () => void;

    constructor(
        fnGetAnswerText: () => string,
        fnGetAnswerData: () => {},
        fnOnNewAnswer : () => void
    )
    {
        this._getAnswerText = fnGetAnswerText;
        this._getAnswerData = fnGetAnswerData;
        this._onNewAnswer = fnOnNewAnswer;

        var self = this;

        $("#txtAnswer").keypress(function (e) {
            if (e.keyCode == 13) {
                if (self.isAnswerPossible()) {
                    self.validateAnswer();
                    return false;
                }
            }
            return true;
        });

        $("#btnCheck").click(
            e => {
                e.preventDefault();
                this.validateAnswer();
            });

        $("#btnCheckAgain").click(
            e=> {
                e.preventDefault();
                this.validateAnswer();
        });

        $("#errorTryCount").click(function () {
            var divAnswerHistory = $("#divAnswerHistory");
            if (!divAnswerHistory.is(":visible"))
                divAnswerHistory.show();
            else
                divAnswerHistory.hide();
        });

        $(".selectorShowAnswer").click( ()=> {
            InputFeedback.ShowCorrectAnswer(); return false;
        });
        $("#buttons-edit-answer").click((e) => {
            e.preventDefault();
            this._onNewAnswer();
            InputFeedback.AnimateNeutral();
        });
    }

    private validateAnswer() {
        var answerText = this._getAnswerText();

        amountOfTries++;
        answerHistory.push(answerText);

        if (answerText.trim().length == 0) {
            InputFeedback.ShowError("Du könntest es es ja wenigstens probieren! Tzzzz... ", true); return false;
        }

        $.ajax({
            type: 'POST',
            url: window.ajaxUrl_SendAnswer,
            data: this._getAnswerData(),
            cache: false,
            success: function (result) {
                answerResult = result;
                $("#buttons-first-try").hide();
                $("#buttons-answer-again").hide();
                if (result.correct) {
                    InputFeedback.ShowSuccess();
                } else {
                    InputFeedback.ShowError();
                };
            }
        });
        return false;
    }

    private isAnswerPossible() {

        if ($("#buttons-first-try").is(":visible"))
            return true;

        if ($("#buttons-edit-answer").is(":visible"))
            return true;

        if ($("#buttons-answer-again").is(":visible"))
            return true;

        return false;
    }   

    public OnAnswerChange() {
        if ($("#buttons-edit-answer").is(":visible")) {
            $("#buttons-edit-answer").hide();
            $("#buttons-answer-again").show();
            InputFeedback.AnimateNeutral();
        }
    }
}

class InputFeedback {

    private static ErrMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter nicht aufgeben.",
        "Übung macht den Meister, Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
    ];

    private static SuccessMsgs = ["Yeah! Weiter so.", "Du bis auf einem guten Weg.", "Sauber!", "Well Done!"];

    public static ShowError(text = "", forceShow: boolean = false)
    {
        if (text == "") {
            text = InputFeedback.ErrMsgs[Utils.Random(0, InputFeedback.ErrMsgs.length - 1)];
        }

        var errorTryText;
        var amountOfTriesText = ["ein Versuch", "zwei", "drei", "vier", "fünf", "sehr hartnäckig", "Respekt!"];

        switch (amountOfTries) {
            case 1:
                errorTryText = amountOfTriesText[amountOfTries - 1]; break;
            case 2:
            case 3:
            case 4:
            case 5:
                errorTryText = amountOfTriesText[amountOfTries - 1] + " Versuche"; break;
            case 6:
            case 7:
                errorTryText = amountOfTriesText[amountOfTries - 1]; break;
            default:
                errorTryText = amountOfTriesText[6];
        }
        $("#errorTryCount").html("(" + errorTryText + ")");

        $('#ulAnswerHistory').html("");
        $.each(answerHistory, function (index, val) {
            $('#ulAnswerHistory').append(
                $('<li>' + val + '</li>'));
        });

        $("#divWrongAnswer").show();
        $("#buttons-first-try").hide();
        $("#buttons-answer-again").hide();

        if (forceShow || Utils.Random(1, 10) % 4 == 0) {
            $("#answerFeedback").html(text).show();
        } else {
            $("#answerFeedback").html(text).hide();
        }

        InputFeedback.AnimateWrongAnswer();
     }

    static AnimateWrongAnswer() {
        $("#buttons-edit-answer").show();
        $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
    }

    static AnimateNeutral() {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);
    }

    static ShowSuccess() {
        $("#buttons-next-answer").show();
        $("#buttons-edit-answer").hide();
        $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
        $("#divWrongAnswer").hide();

        $("#divAnsweredCorrect").show();
        $("#wellDoneMsg").html("" + InputFeedback.SuccessMsgs[Utils.Random(0, InputFeedback.SuccessMsgs.length - 1)]).show();
    }

    static ShowCorrectAnswer() {

        InputFeedback.ShowNextAnswer();
        $("#divWrongAnswer").hide();
        $("#divCorrectAnswer").show();

        ajaxGetAnswer(function (result) {
            $("#spanCorrectAnswer").html(result.correctAnswer);
            $("#spanAnswerDescription").html(result.correctAnswerDesc);

        });
    }

    private static ShowNextAnswer() {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);

        $("#buttons-next-answer").show();

        $("#answerFeedback").hide();

        $("#buttons-first-try").hide();
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").hide();
    }

}

function ajaxGetAnswer(onSuccessAction) {
    $.ajax({
        type: 'POST',
        url: window.ajaxUrl_GetAnswer,
        cache: false,
        success: function (result) {
            onSuccessAction(result);
        }
    });
}