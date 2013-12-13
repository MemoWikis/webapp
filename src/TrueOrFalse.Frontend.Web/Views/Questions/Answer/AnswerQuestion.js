var answerResult;

var errMsgs = ["Wer einen Fehler gemacht hat und ihn nicht korrigiert, begeht einen zweiten. (Konfuzius)",
               "Es ist ein großer Vorteil im Leben, die Fehler, aus denen man lernen kann, möglichst früh zu begehen. (Churchill)",
               "Weiter, weiter nicht aufgeben.",
               "Übung macht den Meister, Du bist auf dem richtigen Weg.",
               "Ein ausgeglichener Mensch ist einer, der denselben Fehler zweimal machen kann, ohne nervös zu werden." //Nur Zeigen, wenn der Fehler tatsächlich wiederholt wurde.
               ];

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
    $(".selectorShowAnswer").click(function () { showCorrectAnswer(); return false; });
    $("#buttons-edit-answer").click(function () {
        newAnswer();
        animateNeutral();
    });
    InitFeedbackSliders();

    var spanSparkLineTotals = $("#sparklineTrueOrFalseTotals");
    spanSparkLineTotals.sparkline([parseInt(spanSparkLineTotals.attr("data-answersTrue")), parseInt(spanSparkLineTotals.attr("data-answersFalse"))], {
        type: 'pie',
        sliceColors: ['#3e7700', '#B13A48']
    });

    var spanSparkLineUser = $("#sparklineTrueOrFalseUser");
    spanSparkLineUser.sparkline([parseInt(spanSparkLineUser.attr("data-answersTrue")), parseInt(spanSparkLineUser.attr("data-answersFalse"))], {
        type: 'pie',
        sliceColors: ['#3e7700', '#B13A48']
    });

    function foo(d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/de_DE/all.js#xfbml=1&appId=128827270569993";
        fjs.parentNode.insertBefore(js, fjs);
    }foo(document, 'script', 'facebook-jssdk');

});

function InitFeedbackSliders() {
    //Quality
    InitFeedbackSlider("Quality");
    InitFeedbackSlider("RelevancePersonal");
    InitFeedbackSlider("RelevanceForAll");
}

function InitFeedbackSlider(sliderName) {

    $("#remove" + sliderName + "Value").click(function () {
        $("#div" + sliderName + "Slider").hide();
        $("#div" + sliderName + "Add").show();
        SendSilderValue(sliderName, -1);
    });

    $("#select" + sliderName + "Value").click(function () {
        $("#div" + sliderName + "Slider").show();
        $("#div" + sliderName + "Add").hide();
    });

    var sliderValue = $("#slider" + sliderName + "Value").text();
    SetUiSliderSpan(sliderName, sliderValue);
    
    $("#slider" + sliderName).slider({
        range: "min",
        max: 100,
        value: sliderValue,
        slide: function (event, ui) { SetUiSliderSpan(sliderName, ui.value); },
        change: function (event, ui) { SendSilderValue(sliderName, ui.value); }
    });
}

function SetUiSliderSpan(sliderName, uiValue) {
    var text = uiValue != -1 ? uiValue / 10 : "";
    $("#slider" + sliderName + "Value").text(text);
}

function SendSilderValue(sliderName, value) {
    $.ajax({
        type: 'POST',
        url: "/Questions/Save" + sliderName + "/" + window.questionId + "/" + value,
        cache: false,
        success: function (result) {
            $("#span" + sliderName + "Count").text(result.totalValuations.toString());
            $("#span" + sliderName + "Average").text(result.totalAverage.toString());
        }
    });
}


function validateAnswer() {

    var answerText = getAnswerText();

    amountOfTries++;
    answerHistory.push(answerText);

    if(answerText.trim().length == 0) {
        showMsgError("Du könntest es es ja wenigstens probieren! Tzzzz... ", true); return false;
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
                showMsgSuccess();
            } else {
                showMsgErrorWithRandomText();
                animateWrongAnswer();
            };
        }
    });
    return false;
}

function updateAmountOfTries() {}

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
    $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
}

function animateNeutral() {
    $("#txtAnswer").animate({ backgroundColor: "white" }, 200);
}

function answerChanged() {
    if ($("#buttons-edit-answer").is(":visible")) {
        $("#buttons-edit-answer").hide();
        $("#buttons-answer-again").show();
        animateNeutral();
    }
}

function showMsgErrorWithRandomText() {
    showMsgError(errMsgs[randomXToY(0, errMsgs.length - 1)]);
}

function showMsgSuccess() {
    $("#buttons-next-answer").show();
    $("#buttons-edit-answer").hide();
    $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
    $("#divWrongAnswer").hide();

    $("#divAnsweredCorrect").show();
    $("#wellDoneMsg").html("" + successMsgs[randomXToY(0, successMsgs.length - 1)]).show();
}

function showMsgError(text, forceShow) {

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
        case 7 :
            errorTryText = amountOfTriesText[amountOfTries - 1]; break;
        default:
            errorTryText = amountOfTriesText[7];
    }
    $("#errorTryCount").html("(" + errorTryText + ")");

    $('#ulAnswerHistory').html("");
    $.each(answerHistory, function(index, val) {
        $('#ulAnswerHistory').append(
            $('<li>' + val + '</li>'));
    });
    
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
    
    ajaxGetAnswer(function (result) {
        $("#spanCorrectAnswer").html(result.correctAnswer);
        $("#spanAnswerDescription").html(result.correctAnswerDesc);
        
    });
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

