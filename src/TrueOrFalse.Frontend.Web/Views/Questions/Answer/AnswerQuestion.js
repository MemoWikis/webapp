$(function () {

    $("#btnCheck").click(function () {
        $.ajax({
            url: this.href + "?answer=" + $("#txtAnswer")[0].value,
            cache: false,
            success: function (html) {
            }
        });
        return false;
    });
})