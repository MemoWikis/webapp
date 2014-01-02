/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
var answerResult;
var getAnswerData;
var newAnswer;
var getAnswerText;

var successMsgs = ["Yeah! Weiter so.", "Du bis auf einem guten Weg.", "Sauber!", "Well Done!"];

var answerHistory = [];
var amountOfTries = 0;

$(function () {
    $("#txtAnswer").keypress(function (e) {
        if (e.keyCode == 13) {
            if (isAnswerPossible()) {
                validateAnswer();
                return false;
            }
        }
        return true;
    });

    $("#btnCheck").click(validateAnswer);
    $("#btnCheckAgain").click(validateAnswer);
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
    $("#buttons-edit-answer").click(function () {
        newAnswer();
        InputFeedback.AnimateNeutral();
    });
});

function validateAnswer() {
    var answerText = getAnswerText();

    amountOfTries++;
    answerHistory.push(answerText);

    if (answerText.trim().length == 0) {
        InputFeedback.ShowError("Du könntest es es ja wenigstens probieren! Tzzzz... ", true);
        return false;
    }

    $.ajax({
        type: 'POST',
        url: window.ajaxUrl_SendAnswer,
        data: getAnswerData(),
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

function answerChanged() {
    if ($("#buttons-edit-answer").is(":visible")) {
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").show();
        InputFeedback.AnimateNeutral();
    }
}

var InputFeedback = (function () {
    function InputFeedback() {
    }
    InputFeedback.ShowError = function (text, forceShow) {
        if (typeof text === "undefined") { text = ""; }
        if (typeof forceShow === "undefined") { forceShow = false; }
        if (text == "") {
            text = InputFeedback.ErrMsgs[randomXToY(0, InputFeedback.ErrMsgs.length - 1)];
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

        if (forceShow || randomXToY(1, 10) % 4 == 0) {
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
        $("#wellDoneMsg").html("" + successMsgs[randomXToY(0, successMsgs.length - 1)]).show();
    };

    InputFeedback.ShowCorrectAnswer = function () {
        showNextAnswer();
        $("#divWrongAnswer").hide();
        $("#divCorrectAnswer").show();

        ajaxGetAnswer(function (result) {
            $("#spanCorrectAnswer").html(result.correctAnswer);
            $("#spanAnswerDescription").html(result.correctAnswerDesc);
        });
    };
    InputFeedback.ErrMsgs = [
        "Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
        "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
        "Weiter, weiter nicht aufgeben.",
        "Übung macht den Meister, Du bist auf dem richtigen Weg.",
        "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden."
    ];
    return InputFeedback;
})();

function randomXToY(minVal, maxVal, floatVal) {
    if (typeof floatVal === "undefined") { floatVal = 'undefined'; }
    var randVal = minVal + (Math.random() * (maxVal - minVal));
    return (typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
}

function showNextAnswer() {
    $("#txtAnswer").animate({ backgroundColor: "white" }, 200);

    $("#buttons-next-answer").show();

    $("#answerFeedback").hide();

    $("#buttons-first-try").hide();
    $("#buttons-edit-answer").hide();
    $("#buttons-answer-again").hide();
}

function isAnswerPossible() {
    if ($("#buttons-first-try").is(":visible"))
        return true;

    if ($("#buttons-edit-answer").is(":visible"))
        return true;

    if ($("#buttons-answer-again").is(":visible"))
        return true;

    return false;
}
//# sourceMappingURL=AnswerQuestion.js.map
