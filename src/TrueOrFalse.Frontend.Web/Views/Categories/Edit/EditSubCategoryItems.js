$(function () {
    
    $("#addSubCategoryItemRow").click(function () {
        $.ajax({
            url: this.href + "?name=" + $("#newItem")[0].value,
            cache: false,
            success: function (html) {
                $("#items").append(html);
            }
        });
        return false;
    });

});