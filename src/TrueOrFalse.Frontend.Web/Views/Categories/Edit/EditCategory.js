function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i"));
};

$(function () {
    $("#txtNewRelatedCategory").autocomplete({
        source: '/Api/Category/ByName',
        minLength: 1
    });

    var bouncing = false;
    function checkText() {
        var text = $("#txtNewRelatedCategory").val();
        var matched = $(".ui-autocomplete li:textEquals('" + text + "')");
        var existing = $(".added-cat:textEquals('" + text + "')");
        if (matched.size() == 0 || existing.size() != 0) {
            $("#addRelatedCategory").hide();
        } else {
            $("#addRelatedCategory").show();
            $("#txtNewRelatedCategory").val(matched.text());
        }
        if (!bouncing && existing.size() != 0) {
            bouncing = true;
            existing.effect('bounce', null, 'fast', function () { bouncing = false; });
        }
        setTimeout(checkText, 0.25);
    }
    checkText();

    var catId = 1;
    function addCat() {
        var catText = $("#txtNewRelatedCategory").val();
        $("#txtNewRelatedCategory").before(
            "<div class='added-cat' id='span-cat-" + catId + "'>" + catText +
                "<input type='hidden' value='" + catText + "' name='cat-" + catId + "'/>" +
                    "</div> ");
        $("#txtNewRelatedCategory").val('');
        catId++;
    }

    $("#addRelatedCategory").click(addCat);
    $("#txtNewRelatedCategory").keydown(function (event) {
        if (event.keyCode == 13 && $("#addRelatedCategory").is(':visible')) {
            addCat();
        }
    });
});