alert("hello");

$("#addClassificationRow").click(function () {
    $.ajax({
        url: this.href,
        cache: false,
        error: function (jqXHR, textStatus, errorThrown) {
            alert("hello");
            /*console.info('in error');
            console.log(jqXHR, textStatus, errorThrown); */
        },
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