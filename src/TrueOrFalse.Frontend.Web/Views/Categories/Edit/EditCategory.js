$(function () {
    
    $("#addClassificationRow").click(function () {
        $.ajax({
            url: this.href,
            cache: false,
            success: function (html) {
                $("#classifications").append(html);
            }
        });
        return false;
    });

    $("a.deleteRow").live("click", function () {
        $(this).parents("div.editorRow:first").remove();
        return false;
    });

});