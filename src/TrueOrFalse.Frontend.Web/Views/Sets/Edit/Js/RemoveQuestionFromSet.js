var RemoveQuestionFromSet = (function () {
    function RemoveQuestionFromSet() {
        $(".deleteButton").click(function () {
            var parentLi = $(this).parent().parent();
            parentLi.hide(800);
            $.post("/EditSet/RemoveQuestionFromSet", {
                "questionInSetId": parentLi.attr("data-id")
            });
            $("#revertAction").show();
        });
        $("#revertAction").click(function () {
            $("#modalRevertAction").modal();
        });
    }
    return RemoveQuestionFromSet;
})();
$(function () {
    new RemoveQuestionFromSet();
});
