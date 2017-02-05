class SetVideo {
    constructor() {

        var self = this;

        $("a[data-video-question-id]").click(function(e) {
            self.LoadQuestionView(e, $(this).attr("data-video-question-id"));
        });

        this.InitAnswerBody();

    }

    LoadQuestionView(e : JQueryEventObject, questionId : any) {
        e.preventDefault();

        $.get("/SetVideo/RenderAnswerBody/?questionId=" + questionId,
            htmlResult => {
                AnswerQuestion.LogTimeForQuestionView();
                this.ChangeAnswerBody(htmlResult);
            });
    }

    ChangeAnswerBody(html : string) {
        $("#divBodyAnswer")
            .empty()
            .animate({ opacity: 0.00 }, 0)
            .append(html)
            .animate({ opacity: 1.00 }, 400);

        $(".show-tooltip").tooltip();
        this.InitAnswerBody();
    }

    InitAnswerBody() {

        var answerEntry = new AnswerEntry();
        answerEntry.Init();

        $('#hddTimeRecords').attr('data-time-on-load', $.now());

        var pinQuestion = new PinQuestion();
        pinQuestion.Init();
    }
}


$(() => {
    $(".pieTotals").each(function() {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#90EE90', '#FFA07A']
        });
    });

    if ($("#hhdHasVideo").val() == "True") {
        new SetVideo();
    }

    new Pin(PinRowType.SetDetail);
    new Pin(PinRowType.Question);
});