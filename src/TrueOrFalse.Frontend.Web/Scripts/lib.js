$.fn.defaultText = function (defaultText) {

    if ($(this).val() == "") {
        $(this).val(defaultText);
    }

    $(this).focus(function () {

        $(this).animate({
            backgroundColor: "#ECFFE1"
        }, 400);

        if ($(this).val() == defaultText) {
            $(this).val("");
        }

    });

    $(this).blur(function () {

        if ($(this).val() == "") {
            $(this).val(defaultText);

            $(this).animate({
                backgroundColor: "#ffffff"
            }, 400);
        }
    });
};

/*** NAVI CHANGES ***/
function SetMenuWishKnowledge(newAmount) {
    $("#menuWishKnowledgeCount")
        .text(newAmount)
        .animate({ opacity: 0.25 }, 100)
        .animate({ opacity: 1.00 }, 500);
}