class AsyncLoading {
    constructor() {
        if (window.location.pathname.split("/")[4] === "im-Fragesatz") {
            $("#AnswerQuestion").ready(() => {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var NextQuestionLinkArgs = $("#NextQuestionLink").attr("href").split("/");
                    var setId = NextQuestionLinkArgs[5];
                    var questionId = NextQuestionLinkArgs[3];
                    var url = "/AnswerQuestion/RenderQuestionBySetAnswerBody/?questionId=" + questionId + "&setId=" + setId;
                    this.loadAnswerBody(url);

                    // - change url
                    // -- pager
                    // - sync server side

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
            });
        } else {
            $("#AnswerQuestion").ready(() => {
                $("#NextQuestionLink, #btnNext").click((e) => {
                    e.preventDefault();
                    var pager = $("#NextQuestionLink").attr("href").split("?")[1].split("=")[1];
                    var url = "/AnswerQuestion/RenderNextQuestionAnswerBody/?pager=" + pager;
                    this.loadAnswerBody(url);

                    // - change url
                    // -- pager
                    // - sync server side

                    //header changes

                    //set menu history  (client and server)

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
            });
        }
    }

    private loadAnswerBody(url: string) {
                $.ajax({
                    url: url,
                    success: htmlResult => {
                        $("div#LicenseQuestion").remove();
                        $("#AnswerBody")
                            .replaceWith(htmlResult);
                        //TODO:Julian fix multiple question skip bug
                        new PageInit();
                    }
                });

    }
}