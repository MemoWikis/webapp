$.fn.defaultText = function (defaultText) {

    $(this).val(defaultText);

    $(this).focus(function () {

        $(this).animate({
            backgroundColor: "#ECFFE1"
        }, 400);

        if ($(this).val() == defaultText) {
            $(this).val("");
        }

    });

    $(this).blur(function () {

        if ($(this).val().trim() == "") {
            $(this).val(defaultText);

            $(this).animate({
                backgroundColor: "#ffffff"
            }, 400);
        }
    });
};