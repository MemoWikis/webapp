function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};

$(function () {
    $("#txtNewRelatedCategory").autocomplete({
        minLength: 0,
        source: '/Api/Category/ByName',
        focus: function (event, ui) {
            $("#txtNewRelatedCategory").data("category-id", ui.item.id);
            $("#txtNewRelatedCategory").val(ui.item.name);
            return false;
        },
        select: function (event, ui) {
            $("#txtNewRelatedCategory").data("category-id", ui.item.id);
            $("#txtNewRelatedCategory").val(ui.item.name);
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
		    return $("<li></li>")
				.data("ui-autocomplete-item", item)
				.append("<a><img src='" + item.imageUrl + "'/><span class='cat-name'>"
				    + item.name + "</span><br><i>" + item.numberOfQuestions + " Fragen</i></a>")
				.appendTo(ul);
		};

    var animating = false;
    function checkText() {
        var text = $("#txtNewRelatedCategory").val();
        var matched = $(".ui-autocomplete li .cat-name:textEquals('" + text + "')");
        var alreadAddedCategory = $(".added-cat:textEquals('" + text + "')");
        if (matched.size() == 0 || alreadAddedCategory.size() != 0) {
            $("#addRelatedCategory").hide();
        } else {
            $("#addRelatedCategory").show();
            if ($("#txtNewRelatedCategory").val() != matched.text()) {
                $("#txtNewRelatedCategory").val(matched.text());
            }
        }
        if (!animating && alreadAddedCategory.size() != 0) {
            animating = true;
            alreadAddedCategory.effect('bounce', null, 'fast', function () { animating = false; });
        }
        setTimeout(checkText, 250);
    }
    checkText();

    var nextCatId = 1;
    function addCat() {
        var catId = nextCatId;
        nextCatId++;
        var catText = $("#txtNewRelatedCategory").val();
        console.log($("#txtNewRelatedCategory").data("category-id"));
        $("#txtNewRelatedCategory").before(
            "<div class='added-cat' id='cat-" + catId + "' style='display: none;'>" +
                "<a href='/Kategorien/" + catText + "/" + catId + "'>" + catText + "</a>" +
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