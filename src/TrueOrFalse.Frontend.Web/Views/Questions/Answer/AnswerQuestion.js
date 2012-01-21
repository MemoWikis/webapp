function check() {
    $.ajax({
        type: 'POST',
        url: this.href,
        data: { answer: $("#txtAnswer")[0].value },
        cache: false,
        success: function (result) {
            $("#buttons-first-try").hide();
            $("#buttons-answer-again").hide();
            if (result.correct) {
                $("#buttons-correct-answer").show();
                $('#txtAnswer').attr('readonly', true);
                $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
            } else {
                $("#buttons-edit-answer").show();
                $("#txtAnswer").focus(function(event) {
                    $("#buttons-edit-answer").hide();
                    $("#buttons-answer-again").show();
                    $(this).animate({ backgroundColor: "white" }, 200);
                    $(this).unbind(event);
                });
                $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
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

$(function () {
    $("#btnCheck").click(check);
    $("#btnCheckAgain").click(check);
    $("#buttons-edit-answer").click(function () {
        $("#txtAnswer").focus();
        $("#txtAnswer").setCursorPosition(0);
    });
})