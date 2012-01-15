$(function () {

    $("#btnCheck").click(function () {
        $.ajax({
            type: 'POST',
            url: this.href,
            data: { answer: $("#txtAnswer")[0].value },
            cache: false,
            success: function (result) {
                alert(result.correct);
            }
        });
        return false;
    });
})