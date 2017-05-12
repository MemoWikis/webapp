class AsyncLoading {
    constructor() {
        $("#NextQuestion, #btnNext").click((e) => {
            e.preventDefault();
            //check for learnign, test, game
            //alert(elementOnPage);
            //var urlParams = Utils.GetQueryString();
            //var url = "/AnswerQuestion/RenderAnswerBody/?questionId=" + $("#questionId").val() + "&pager=" + urlParams.pager + "&elementOnPage=" + elementOnPage;

            var pager = $(".Next #NextQuestionLink").attr("href").split("?")[1].split("=")[1];
            var url = "/AnswerQuestion/RenderAnswerBodyRedirector/?pager=" + pager;
            $.post(url, (htmlResult) => {
                $("div#LicenseQuestion").remove();
                $("#AnswerBody")
                    .replaceWith(htmlResult); 
            });
            //load answer body
            // - change url
            // -- pager
            // - sync server side

            //header changes

            //set menu history  (client and server)

            //load answer details

            //care about comments
            //care about suggestion

        });
    }
}