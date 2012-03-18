function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};

$(function () {
    $("#txtAddUserFilter").autocomplete({
        minLength: 0,
        source: '/Api/User/ByName',
        focus: function (event, ui) {
            $("#txtAddUserFilter").val(ui.item.name);
            return false;
        },
        select: function (event, ui) {
            $("#txtAddUserFilter").val(ui.item.name);
            return false;
        }
    }).data("autocomplete")._renderItem = function (ul, item) {
        return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a><span class='usr-name'>" + item.name + "</span><br><i>" + item.numberOfQuestions + " Fragen</i></a>")
				.appendTo(ul);
    };
});