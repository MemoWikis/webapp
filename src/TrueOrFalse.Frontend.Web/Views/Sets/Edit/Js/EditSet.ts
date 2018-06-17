class EditSet {
    constructor() {
        var self = this;

        $("div.QuestionText a[data-action=open-details]").click(function(e) {
            self.ExpandSetRow(e, $(this));
        });
        $("div.QuestionText a[data-action=close-details]").click(function(e) {
            self.CollapseSetRow(e, $(this));
        });

        $("#ulQuestions input[data-input=video-timecode]").change(function() {
            self.SaveTimeCode($(this));
        });
    }

    ExpandSetRow(e : JQueryEventObject, elem : JQuery) {
        e.preventDefault();

        elem.hide();
        elem.parent().css('max-height', 'none');
        elem.parent().find("a[data-action=close-details]").show();
    }

    CollapseSetRow(e: JQueryEventObject, elem: JQuery) {
        e.preventDefault();

        elem.hide();
        elem.parent().css('max-height', '32px');
        elem.parent().find("a[data-action=open-details]").show();
    }

    SaveTimeCode(elem: JQuery) {
        var timeCode = elem.val();
        var questionInSetId = elem.attr("data-in-set-id");

        $.post("/SetVideo/SaveTimeCode/", { timeCode: timeCode, questionInSetId: questionInSetId } );
    }

    static GetSetId() : number { 
        return parseInt($("#questionSetId").attr("data-id"));
    }
}

$(() => {
    new EditSet();
})