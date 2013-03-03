/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />

class QuestionSortable
{
    constructor() {
        $("#sortable").sortable({
            placeholder: "ui-state-highlight",
            cursor: "move",
            stop: function (event, ui) {
                console.log(ui.item.index());
            }
        });
    }

    SendIndicies() { 
    }
}

$(function () {

    $("#sortable").disableSelection();
});