/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />

class QuestionSortable
{
    _ulQuestions: JQuery;
    _questionSetId: number;

    constructor() {

        this._ulQuestions = $("#ulQuestions");
        this._questionSetId = parseInt($("#questionSetContainer").attr("data-id"));

        this._ulQuestions.sortable({
            placeholder: "ui-state-highlight",
            cursor: "move",
            stop: (event, ui) => {
                this.UpdateIndicies();
            }
        });
    }

    UpdateIndicies() { 
        var lisItems = $("#ulQuestions li");
        var cmdItems = [];
        for (var i = 0; i < lisItems.length; i++) {
            var questionId = $(lisItems[i]).attr("data-id");
            cmdItems.push({ "QuestionId": questionId, "NewIndex": i})
        }

        $.post("/QuestionSet/UpdateQuestionsOrder/", 
            { "questionSetId":this._questionSetId, 
              "newIndicies": JSON.stringify(cmdItems) });
    }
}

$(function () {

    var questionSortable = new QuestionSortable();
    $("#ulQuestions").disableSelection();
});