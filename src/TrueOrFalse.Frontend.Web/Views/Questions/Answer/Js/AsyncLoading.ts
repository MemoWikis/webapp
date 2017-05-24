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
            } else if ($("#hddIsLearningSession").val() === "True") {
                var learningSessionId = $("#hddIsLearningSession").attr("data-learning-session-id");
                var skipStepIdx = $("#hddIsLearningSession").attr("data-skip-step-index");
                var url = "/AnswerQuestion/RenderAnswerBodyByLearningSession/?learningSessionId=" + learningSessionId + "&skipStepIdx=" + skipStepIdx;
                this.loadNewQuestion(url);

            } else if ($("#hddIsTestSession").val() === "True") {
                $("#btnNext").click((e) => {
                    e.preventDefault();
                    var testSessionId = $("#hddIsTestSession").attr("data-test-session-id");
                    var url = "/AnswerQuestion/RenderAnswerBodyByTestSession/?testSessionId=" + testSessionId;
                    this.loadNewQuestion(url);
                });
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
                        if (result.navBarData.currentHtml != null)
                            this.updateNavigationBar(result.navBarData);
                        else
                            this.updateSessionHeader(result.currentSessionHeader, result.sessionData);
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

                        this.sendGoogleAnalyticsPageView(result.offlineDevelopment);
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

    private updateSessionHeader(currentSessionHeaderText, sessionStepData) {
        $("#hddIsTestSession").attr("data-current-step-idx", sessionStepData.currentStepIdx);
        $("#hddIsTestSession").attr("data-is-last-step", sessionStepData.isLastStep);
        $(".SessionBar .QuestionCount").html(currentSessionHeaderText);
    }

    private updateUrl(url: string) {
        if(Modernizr.history)
            history.pushState({stateType: "BrowserNavigation"}, $(".QuestionText").html(), url);
    }

    private sendGoogleAnalyticsPageView(offlineDevelopment: boolean) {
        if(!offlineDevelopment)
            ga('send', 'pageview');
    }
}