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
            $("#addFilterUserId").val(ui.item.id);
            return false;
        },
        select: function (event, ui) {
            $("#txtAddUserFilter").val(ui.item.name);
            $("#addFilterUserId").val(ui.item.id);
            return false;
        }
    }).data("autocomplete")._renderItem = function (ul, item) {
        return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a><span class='usr-name'>" + item.name + "</span><br><i>" + item.numberOfQuestions + " Fragen</i></a>")
				.appendTo(ul);
    };

    function checkText() {
        var text = $("#txtAddUserFilter").val();
        var matched = $(".ui-autocomplete li .usr-name:textEquals('" + text + "')");
        //var existing = $(".added-cat:textEquals('" + text + "')");
        if (matched.size() == 0) {// || existing.size() != 0) {
            $("#addUserFilter").hide();
        } else {
            $("#addUserFilter").show();
        }
        setTimeout(checkText, 250);
    }
    checkText();

    $("#txtAddUserFilter").keydown(function (event) {
        checkText();
        //if (event.keyCode == 13 && $("#addUserFilter").is(':visible')) {
        //    addUser();
        //}
    });
});