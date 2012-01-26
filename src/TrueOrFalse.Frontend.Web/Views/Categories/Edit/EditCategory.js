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
    }).keyup(function() {
        if ($(".ui-autocomplete li:textEquals('" + $(this).val() + "')").size() == 0) {
            $("#addRelatedCategory").hide();
        } else {
            $("#addRelatedCategory").show();
        }
    });
});