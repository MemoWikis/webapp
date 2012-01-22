function check() {

    var answerText = $("#txtAnswer")[0].value;

    $("#answerFeedback").hide();

    if(answerText.trim().length == 0) {
        msgNoAnswerProvided(); return "";
    }

    $.ajax({
        type: 'POST',
        url: this.href,
        data: { answer: answerText },
        cache: false,
        success: function (result) {
            $("#buttons-first-try").hide();
            $("#buttons-answer-again").hide();
            if (result.correct) {
                $("#buttons-correct-answer").show();
                $('#txtAnswer').attr('readonly', true);
                $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
            } else {
                animateWrongAnswer();
            };
        }
    });
    return false;
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

$(function() {
    $("#btnCheck").click(check);
    $("#btnCheckAgain").click(check);
    $("#buttons-edit-answer").click(function() {
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

function msgNoAnswerProvided() {
    $("#buttons-first-try").hide();
    $("#buttons-answer-again").hide();

    $("#answerFeedback").text("(Du könntest es es ja wenigstens probieren! Tzzzz...) ").show();

    animateWrongAnswer();
}