/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../scripts/utils.ts" />
var answerResult;

var answerHistory = [];
var amountOfTries = 0;

var AnswerQuestion = (function () {
    function AnswerQuestion(solutionEntry) {
        var _this = this;
        this._getAnswerText = solutionEntry.GetAnswerText;
        this._getAnswerData = solutionEntry.GetAnswerData;
        this._onNewAnswer = solutionEntry.OnNewAnswer;

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

        $("#btnCheck").click(function (e) {
            e.preventDefault();
            _this.validateAnswer();
        });

        $("#btnCheckAgain").click(function (e) {
            e.preventDefault();
            _this.validateAnswer();
        });

        $("#errorTryCount").click(function () {
            var divAnswerHistory = $("#divAnswerHistory");
            if (!divAnswerHistory.is(":visible"))
                divAnswerHistory.show();
            else
                divAnswerHistory.hide();
        });

        $(".selectorShowAnswer").click(function () {
            InputFeedback.ShowCorrectAnswer();
            return false;
        });
        $("#buttons-edit-answer").click(function (e) {
            e.preventDefault();
            _this._onNewAnswer();
            InputFeedback.AnimateNeutral();
        });
    }
    AnswerQuestion.prototype.validateAnswer = function () {
        var answerText = this._getAnswerText();

        amountOfTries++;
        answerHistory.push(answerText);

        if (answerText.trim().length == 0) {
            InputFeedback.ShowError("Du könntest es es ja wenigstens probieren! Tzzzz... ", true);
            return false;
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
                }
                ;
            }
        });
        return false;
    };

    AnswerQuestion.prototype.isAnswerPossible = function () {
        if ($("#buttons-first-try").is(":visible"))
            return true;

        if ($("#buttons-edit-answer").is(":visible"))
            return true;

        if ($("#buttons-answer-again").is(":visible"))
            return true;

        return false;
    };

    AnswerQuestion.prototype.OnAnswerChange = function () {
        this.Reenable_answer_button_if_renewed_answer();
    };

    AnswerQuestion.prototype.Reenable_answer_button_if_renewed_answer = function () {
        if ($("#buttons-edit-answer").is(":visible")) {
            $("#buttons-edit-answer").hide();
            $("#buttons-answer-again").show();
            InputFeedback.AnimateNeutral();
        }
    };
    return AnswerQuestion;
})();

var InputFeedback = (function () {
    function InputFeedback() {
    }
    InputFeedback.ShowError = function (text, forceShow) {
        if (typeof text === "undefined") { text = ""; }
        if (typeof forceShow === "undefined") { forceShow = false; }
        if (text == "") {
            text = InputFeedback.ErrMsgs[Utils.Random(0, InputFeedback.ErrMsgs.length - 1)];
        }

        var errorTryText;
        var amountOfTriesText = ["ein Versuch", "zwei", "drei", "vier", "fünf", "sehr hartnäckig", "Respekt!"];

        switch (amountOfTries) {
            case 1:
                errorTryText = amountOfTriesText[amountOfTries - 1];
                break;
            case 2:
            case 3:
            case 4:
            case 5:
                errorTryText = amountOfTriesText[amountOfTries - 1] + " Versuche";
                break;
            case 6:
            case 7:
                errorTryText = amountOfTriesText[amountOfTries - 1];
                break;
            default:
                errorTryText = amountOfTriesText[6];
        }
        $("#errorTryCount").html("(" + errorTryText + ")");

        $('#ulAnswerHistory').html("");
        $.each(answerHistory, function (index, val) {
            $('#ulAnswerHistory').append($('<li>' + val + '</li>'));
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
    };

    InputFeedback.AnimateWrongAnswer = function () {
        $("#buttons-edit-answer").show();
        $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
    };

    InputFeedback.AnimateNeutral = function () {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);
    };

    InputFeedback.ShowSuccess = function () {
        $("#buttons-next-answer").show();
        $("#buttons-edit-answer").hide();
        $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
        $("#divWrongAnswer").hide();

        $("#divAnsweredCorrect").show();
        $("#wellDoneMsg").html("" + InputFeedback.SuccessMsgs[Utils.Random(0, InputFeedback.SuccessMsgs.length - 1)]).show();
    };

    InputFeedback.ShowCorrectAnswer = function () {
        InputFeedback.ShowNextAnswer();
        $("#divWrongAnswer").hide();
        $("#divCorrectAnswer").show();

        ajaxGetAnswer(function (result) {
            $("#spanCorrectAnswer").html(result.correctAnswer);
            $("#spanAnswerDescription").html(result.correctAnswerDesc);
        });
    };

    InputFeedback.ShowNextAnswer = function () {
        $("#txtAnswer").animate({ backgroundColor: "white" }, 200);

        $("#buttons-next-answer").show();

        $("#answerFeedback").hide();

        $("#buttons-first-try").hide();
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").hide();
    };
    InputFeedback.ErrMsgs = [
        "Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter nicht aufgeben.",
        "Übung macht den Meister, Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden."
    ];

    InputFeedback.SuccessMsgs = ["Yeah! Weiter so.", "Du bis auf einem guten Weg.", "Sauber!", "Well Done!"];
    return InputFeedback;
})();

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
//# sourceMappingURL=AnswerQuestion.js.map
