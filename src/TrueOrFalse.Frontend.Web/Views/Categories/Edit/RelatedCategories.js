function escape_regexp(text) {
    return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
}

$.expr[':'].textEquals = function (a, i, m) {
    return $(a).text().match(new RegExp("^" + escape_regexp(m[3]) + "$", "i")) != null;
};

$(function () {

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
            
            if ($(".added-cat:textEquals('" + ui.item.name + "')").length > 0) {
                return false;
            }

            addCat();
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
        var matchesInAutomcompleteList = $(".ui-autocomplete li .cat-name:textEquals('" + text + "')");
        var alreadyAddedCategory = $(".added-cat:textEquals('" + text + "')");

        if (matchesInAutomcompleteList.size() != 0 && alreadyAddedCategory.size() == 0) {
            if ($("#txtNewRelatedCategory").val() != matchesInAutomcompleteList.text()) {
                $("#txtNewRelatedCategory").val(matchesInAutomcompleteList.text());
            }
        }

        if (!animating && alreadyAddedCategory.size() != 0) {
            animating = true;
            alreadyAddedCategory.effect('bounce', null, 'fast', function () { animating = false; });
        }
        setTimeout(checkText, 250);
    }
    checkText();


    var fnCheckTextAndAdd = function (event) {
        checkText();
        if (event.keyCode == 13 && $(".added-cat:textEquals('" + ui.item.name + "')").length == 0) {
            addCat();
        }

        if (event.keyCode == 13) {
            event.preventDefault();
        }
    }
    $("#txtNewRelatedCategory").keydown(fnCheckTextAndAdd);

    $("#txtNewRelatedCategory").bind("initCategoryFromTxt", addCat);
});