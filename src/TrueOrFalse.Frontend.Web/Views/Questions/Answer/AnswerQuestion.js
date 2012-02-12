var answerResult;

function validateAnswer() {

    var answerText = $("#txtAnswer")[0].value;

    $("#answerFeedback").hide();

    if(answerText.trim().length == 0) {
        showMsgError("Du könntest es es ja wenigstens probieren! Tzzzz... "); return false;
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

$(function () {
    $("#btnCheck").click(validateAnswer);
    $("#btnCheckAgain").click(validateAnswer);
    $(".selectorShowAnswer").click(function () { showCorrectAnswer(); return false; });
    $("#buttons-edit-answer").click(function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    });
});

function animateWrongAnswer() {
    $("#buttons-edit-answer").show();
    $("#txtAnswer").focus(function (event) {
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").show();
        $(this).animate({ backgroundColor: "white" }, 200);
        $(this).unbind(event);
        $("#answerFeedback").hide();
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
    $("#buttons-correct-answer").show();
    /* $('#txtAnswer').attr('readonly', true); */
    $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);

    $("#answerFeedback").html("<font color=green>Richtig!</font>  " + successMsgs[randomXToY(0, successMsgs.length - 1)]).show();
}

function showMsgError(text) {
    $("#buttons-first-try").hide();
    $("#buttons-answer-again").hide();

    $("#answerFeedback").html("<font color=red>Falsche Antwort!</font></br>(" + text + ")").show();

    animateWrongAnswer();
}

function randomXToY(minVal, maxVal, floatVal) {
    var randVal = minVal + (Math.random() * (maxVal - minVal));
    return typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal);
}

function showCorrectAnswer() {
    $("#divCorrectAnswer").show();
    
    if (answerResult != null)
        $("#spanCorrectAnswer").html(answerResult.correctAnswer);

    ajaxGetAnswer(function(correctAnswer) {
        $("#spanCorrectAnswer").html(correctAnswer);
    });
}

