var answerResult;

$(function () {
    $("#btnCheck").click(validateAnswer);
    $("#btnCheckAgain").click(validateAnswer);
    $("#errorTryCount").click(function () {
        var divAnswerHistory = $("#divAnswerHistory");
        if (!divAnswerHistory.is(":visible"))
            divAnswerHistory.show();
        else
            divAnswerHistory.hide();
    });
    $(".selectorShowAnswer").click(function () { showCorrectAnswer(); return false; });
    $("#buttons-edit-answer").click(function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    });
});


function validateAnswer() {

    var answerText = $("#txtAnswer")[0].value;

    if(answerText.trim().length == 0) {
        showMsgError("Du könntest es es ja wenigstens probieren! Tzzzz... ", true); return false;
    }

    $.ajax({
        type: 'POST',
        url: window.ajaxUrl_SendAnswer,
        data: { answer: answerText },
        cache: false,
        success: function (result) {
            answerResult = result;
            $("#buttons-first-try").hide();
            $("#buttons-answer-again").hide();
            if (result.correct) {
                showMsgSuccess();
            } else {
                showMsgErrorWithRandomText();
                animateWrongAnswer();
            };
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
            onSuccessAction(result.correctAnswer);
        }
    });
}


$.fn.setCursorPosition = function (pos) {
    this.each(function (index, elem) {
        if (elem.setSelectionRange) {
            elem.setSelectionRange(pos, pos);
        } else if (elem.createTextRange) {
            var range = elem.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    });
    return this;
};


function animateWrongAnswer() {
    $("#buttons-edit-answer").show();
    $("#txtAnswer").focus(function (event) {
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").show();
        $(this).animate({ backgroundColor: "white" }, 200);
        $(this).unbind(event);
    });
    $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
}

var errMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
               "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
               "Weiter, weiter nicht aufgeben.",
               "Übung macht den Meister, Du bist auf dem richtigen Weg.", 
               "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
               ];

var successMsgs = ["Yeah! Weiter so.", "Du bis auf einem guten Weg.", "Sauber!", "Well Done!"];

function showMsgErrorWithRandomText() {
    showMsgError(errMsgs[randomXToY(0, errMsgs.length - 1)]);
}

function showMsgSuccess() {
    $("#buttons-next-answer").show();
    /* $('#txtAnswer').attr('readonly', true); */
    $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
    $("#divWrongAnswer").hide();

    $("#divAnsweredCorrect").show();
    $("#wellDoneMsg").html("" + successMsgs[randomXToY(0, successMsgs.length - 1)]).show();
}

function showMsgError(text, forceShow) {
    $("#divWrongAnswer").show();
    $("#buttons-first-try").hide();
    $("#buttons-answer-again").hide();

    if (forceShow || randomXToY(1, 10) % 4 == 0) {
        $("#answerFeedback").html(text).show();
    }else {
        $("#answerFeedback").html(text).hide();
    }

    animateWrongAnswer();
}

function randomXToY(minVal, maxVal, floatVal) {
    var randVal = minVal + (Math.random() * (maxVal - minVal));
    return typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal);
}

function showCorrectAnswer() {

    showNextAnswer();
    $("#divWrongAnswer").hide();
    $("#divCorrectAnswer").show();

    if (answerResult != null)
        $("#spanCorrectAnswer").html(answerResult.correctAnswer);

    ajaxGetAnswer(function(correctAnswer) {
        $("#spanCorrectAnswer").html(correctAnswer);
    });
}

function showNextAnswer() {
    $("#txtAnswer").animate({ backgroundColor: "white" }, 200);
    $("#answerFeedback").hide();
    
    $("#buttons-first-try").hide();
    $("#buttons-edit-answer").hide();
    $("#buttons-answer-again").hide();
    
    $("#buttons-next-answer").show();
}

