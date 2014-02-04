$.fn.defaultText = function () {
 
    //placeholder attribute for IE
    var defaultText = "";

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
                backgroundColor: "#ffdede"
            }, 400);
        }
    });
};

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