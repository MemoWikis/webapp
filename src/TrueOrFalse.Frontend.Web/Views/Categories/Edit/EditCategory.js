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
    }).keyup(function () {
        var matched = $(".ui-autocomplete li:textEquals('" + $(this).val() + "')");
        if (matched.size() == 0) {
            $("#addRelatedCategory").hide();
        } else {
            $("#addRelatedCategory").show();
            $(this).val(matched.text());
        }
    });

    var lastCategoryId = 1;
    $("#addRelatedCategory").click(function () {
        var catText = $("#txtNewRelatedCategory").val();
        var catId = "category-" + lastCategoryId;
        $("#txtNewRelatedCategory").before("<span id='" + catId + "'>" + catText + "</span> ");
        $("#txtNewRelatedCategory").val('');
    });
});