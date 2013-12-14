/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
var RemoveQuestionFromSet = (function () {
    function RemoveQuestionFromSet() {
        $(".deleteButton").click(function () {
            var parentLi = $(this).parent().parent();
            parentLi.hide(800);
            $.post("/EditSet/RemoveQuestionFromSet", { "questionInSetId": parentLi.attr("data-id") });
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
//# sourceMappingURL=RemoveQuestionFromSet.js.map
