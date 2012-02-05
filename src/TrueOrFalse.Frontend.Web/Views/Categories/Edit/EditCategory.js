function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};

$(function () {
    $("#txtNewRelatedCategory").autocomplete({
        source: '/Api/Category/ByName',
        minLength: 1
    });

    var animating = false;
    function checkText() {
        var text = $("#txtNewRelatedCategory").val();
        var matched = $(".ui-autocomplete li:textEquals('" + text + "')");
        var existing = $(".added-cat:textEquals('" + text + "')");
        if (matched.size() == 0 || existing.size() != 0) {
            $("#addRelatedCategory").hide();
        } else {
            $("#addRelatedCategory").show();
            if ($("#txtNewRelatedCategory").val() != matched.text()) {
                $("#txtNewRelatedCategory").val(matched.text());
            }
        }
        if (!animating && existing.size() != 0) {
            animating = true;
            existing.effect('bounce', null, 'fast', function () { animating = false; });
        }
        setTimeout(checkText, 250);
    }
    checkText();

    var nextCatId = 1;
    function addCat() {
        var catId = nextCatId;
        nextCatId++;
        var catText = $("#txtNewRelatedCategory").val();
        $("#txtNewRelatedCategory").before(
            "<div class='added-cat' id='cat-" + catId + "' style='display: none;'>" + catText +
                "<input type='hidden' value='" + catText + "' name='cat-" + catId + "'/>" +
                    "<a href='#' id='delete-cat-" + catId + "'><img alt='' src='/Images/Buttons/cross.png' /></a>" +
                        "</div> ");
        $("#txtNewRelatedCategory").val('');
        $("#delete-cat-" + catId).click(function () {
            animating = true;
            $("#cat-" + catId).stop(true).animate({ opacity: 0 }, 250, function () {
                $(this).hide("blind", { direction: "horizontal" }, function () {
                    $(this).remove();
                    animating = false;
                });
            });
        });
        $("#cat-" + catId).show("blind", { direction: "horizontal" });
    }

    $("#addRelatedCategory").click(addCat);
    $("#txtNewRelatedCategory").keydown(function (event) {
        checkText();
        if (event.keyCode == 13 && $("#addRelatedCategory").is(':visible')) {
            addCat();
        }
    });
});