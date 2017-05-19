class AsyncLoading {
    constructor() {
        $("#AnswerQuestion").ready(() => {
            if (window.location.pathname.split("/")[4] === "im-Fragesatz") {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#NextQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyBySet/?questionId=" +
                        questionId +
                        "&setId=" +
                        setId;
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#PreviousQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyBySet/?questionId=" +
                        questionId +
                        "&setId=" +
                        setId;
                    this.loadNewQuestion(primaryDataUrl);
                });
            } else if ($("#hddIslearningSession").val() === "True") {
                //do the LearningSession stuff
            } else if ($("#hddIsTestSession").val() === "True") {
                //do the TestSession stuff
            } else {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByNextQuestion/?pager=" + pager;
                    this.loadNewQuestion(primaryDataUrl);
                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var pager = $("#PreviousQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var primaryDataUrl = "/AnswerQuestion/RenderAnswerBodyByPreviousQuestion/?pager=" + pager;
                    this.loadNewQuestion(primaryDataUrl);
                });
            }

            if (Modernizr.history) {
                $(window).off("popstate");
                window.onpopstate = (event) => {
                    if (event.state.stateType === "BrowserNavigation")
                        location.reload();
                };
            }
        });
    }

    private loadNewQuestion(url: string) {
                $.ajax({
                    url: url,
                    success: result => {
                        result = JSON.parse(result);
                        $("div#LicenseQuestion").remove();
                        $("#AnswerBody")
                            .replaceWith(result.answerBodyAsHtml);
                        this.updateNavigationBar(result.navBarData);
                        document.title = $(".QuestionText").html();
                        this.updateUrl(result.url);
                        $("div#answerQuestionDetails").replaceWith(result.questionDetailsAsHtml);
                        $("div#comments").replaceWith(result.commentsAsHtml);
                        new PageInit();

                        FillSparklineTotals();
                        InitTooltips();
                        Images.Init();
                        InitClickLog("div#LicenseQuestion");
                        InitClickLog("div#AnswerBody");
                        InitClickLog("div#AnswerQuestionPager");
                        InitClickLog("div#answerQuestionDetails");
                        InitClickLog("div#comments");
                        PreventDropdonwnsFromBeingHorizontallyOffscreen("div#AnswerBody");
                    }
                });
    }

    private updateNavigationBar(navBarData: any) {
        $("#AnswerQuestionPager .Current").replaceWith($(navBarData.currentHtml).find(".Current"));

        if (navBarData.nextUrl) {
            if($("#NextQuestionLink").length === 0)
                $("#AnswerQuestionPager .Next").append($(navBarData.currentHtml).find("#NextQuestionLink"));
            else
                $("#NextQuestionLink").attr("href", navBarData.nextUrl);
        } else {
            $("#NextQuestionLink").remove();
        }

        if (navBarData.previousUrl) {
            if ($("#PreviousQuestionLink").length === 0)
                $("#AnswerQuestionPager .Previous").append($(navBarData.currentHtml).find("#PreviousQuestionLink"));
            else
                $("#PreviousQuestionLink").attr("href", navBarData.previousUrl);
        } else {
            $("#PreviousQuestionLink").remove();
        }

        $("#NextQuestionLink, #PreviousQuestionLink").unbind();
    }

    private updateUrl(url: string) {
        if(Modernizr.history)
            history.pushState({stateType: "BrowserNavigation"}, $(".QuestionText").html(), url);
    }
}