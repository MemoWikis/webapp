class AsyncLoading {
    constructor() {
        $("#AnswerQuestion").ready(() => {
            if (window.location.pathname.split("/")[4] === "im-Fragesatz") {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#NextQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var url = "/AnswerQuestion/RenderQuestionBySetAnswerBody/?questionId=" + questionId + "&setId=" + setId;
                    this.loadAnswerBody(url);

                    // - change url
                    // -- pager
                    // (- sync server side)

                    //header changes

                    //set menu history  (client and server)

                    //load answer details

                    //care about comments
                    //care about suggestion

                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#PreviousQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var url = "/AnswerQuestion/RenderQuestionBySetAnswerBody/?questionId=" + questionId + "&setId=" + setId;
                    this.loadAnswerBody(url);
                });
            } else {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var url = "/AnswerQuestion/RenderNextQuestionAnswerBody/?pager=" + pager;
                    this.loadAnswerBody(url);

                    //( - change url
                    // -- pager
                    // - sync server side)

                    //header changes

                    //set menu history  (client and server) - done just have to reload menu

                    //load answer details

                    //care about comments
                    //care about suggestion

                });

                $("#PreviousQuestionLink").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var url = "/AnswerQuestion/RenderPreviousQuestionAnswerBody/?pager=" + pager;
                    this.loadAnswerBody(url);
                });
            }
        });
    }

    private loadAnswerBody(url: string) {
                $.ajax({
                    url: url,
                    success: result => {
                        result = JSON.parse(result);
                        $("div#LicenseQuestion").remove();
                        $("#AnswerBody")
                            .replaceWith(result.answerBodyAsHtml);
                        this.updateNavigationBar(result.navBarData);
                        history.pushState({}, $(".QuestionText").html(), result.url);
                        new PageInit();
                    }
                });
    }

    private updateNavigationBar(navBarData: any) {
        $("#AnswerQuestionPager .Current").replaceWith($(navBarData.currentHtml).find(".Current"));
        $("#NextQuestionLink").attr("href", navBarData.nextUrl);
        $("#PreviousQuestionLink").attr("href", navBarData.previousUrl);
        $("#NextQuestionLink, #PreviousQuestionLink").unbind();
    }
}