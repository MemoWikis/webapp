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
                    var pager = $("#PreviousQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var url = "/AnswerQuestion/RenderPreviousQuestionAnswerBody/?pager=" + pager;
                    this.loadAnswerBody(url);
                });
            }

            $(window).off("popstate");
            $(window).on("popstate", () => {
                location.reload();
            });
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
                        //TODO:Julian Check here with Modernizr
                        document.title = $(".QuestionText").html();
                        this.updateUrl(result.url);
                        new PageInit();
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
        history.pushState({}, $(".QuestionText").html(), url);
    }
}