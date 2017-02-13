class SetVideo {
    constructor() {

        $("a[data-video-question-id]").click(function (e) {
            SetVideo.LoadQuestionView(e, $(this));
        });

        SetVideo.InitAnswerBody();
    }

    static LoadQuestionView(e: JQueryEventObject, menuItem: JQuery) {
        e.preventDefault();

        $("#video-pager")
            .find("[data-video-question-id]")
            .removeClass("btn-info")
            .removeClass("current");

        menuItem
            .addClass("btn-info")
            .addClass("current");

        var questionId = menuItem.attr("data-video-question-id");

        $.get("/SetVideo/RenderAnswerBody/?questionId=" + questionId,
            htmlResult => {
                AnswerQuestion.LogTimeForQuestionView();
                this.ChangeAnswerBody(htmlResult);
            });        
    }

    static ClickItem(questionId : number) {
        $("#video-pager a[data-video-question-id=" + questionId + "]").trigger("click");
    }

    static ChangeAnswerBody(html: string) {
        $("#divBodyAnswer")
            .empty()
            .animate({ opacity: 0.00 }, 0)
            .append(html)
            .animate({ opacity: 1.00 }, 400);

        $(".show-tooltip").tooltip();
        this.InitAnswerBody();
    }

    static InitAnswerBody() {

        var answerEntry = new AnswerEntry();
        answerEntry.Init();

        answerEntry.OnCorrectAnswer(() => { this.HandleCorrectAnswer(); });
        answerEntry.OnWrongAnswer(() => { this.HandleWrongAnswer(); });

        $('#hddTimeRecords').attr('data-time-on-load', $.now());

        var pinQuestion = new PinQuestion();
        pinQuestion.Init();

        Images.Init();
    }

    static HandleCorrectAnswer() {
        this.GetCurrentMenuItem()
            .removeClass("wrongAnswer")
            .addClass("correctAnswer");
    }

    static HandleWrongAnswer() {
        this.GetCurrentMenuItem()
            .removeClass("correctAnswer")
            .addClass("wrongAnswer");
    }

    static GetCurrentMenuItem(): JQuery {
        return $("#video-pager").find("a.current").first();
    }
}