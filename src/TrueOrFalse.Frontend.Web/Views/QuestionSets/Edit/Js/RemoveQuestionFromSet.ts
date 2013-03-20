/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />

class RemoveQuestionFromSet 
{ 
    constructor() {           
        $(".deleteButton").click(function() {
            var parentLi = $(this).parent().parent();
            parentLi.hide(800);
            $.post("/EditQuestionSet/RemoveQuestionFromSet", {"questionInSetId" : parentLi.attr("data-id")});
            $("#revertAction").show();
        });

        $("#revertAction").click(function() {
            $("#modalRevertAction").modal();
        });
          
    }
}

$(function () { 
    new RemoveQuestionFromSet();
});