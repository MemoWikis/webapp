function check() {
    $.ajax({
        type: 'POST',
        url: this.href,
        data: { answer: $("#txtAnswer")[0].value },
        cache: false,
        success: function (result) {
            $("#buttons-first-try").hide();
            if (result.correct) {
                $("#txtAnswer").animate({ backgroundColor: "#90EE90" }, 1000);
                $("#buttons-correct-answer").show();
                $("#buttons-wrong-answer").hide();
            } else {
                $("#txtAnswer").animate({ backgroundColor: "#FFB6C1" }, 1000);
                $("#buttons-correct-answer").hide();
                $("#buttons-wrong-answer").show();
            }
            $("#txtAnswer").keyup(function (event) {
                $(this).animate({ backgroundColor: "white" }, 200);
                $(this).unbind(event);
            });
        }
    });
    return false;
}

$(function () {
    $("#btnCheck").click(check);
    $("#btnCheckAgain").click(check);

})