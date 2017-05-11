class AsyncLoading {
    constructor() {
        $("#NextQuestion, #btnNext").click((e) => {
            e.preventDefault();
            //check for learnign, test, game
            var splitUrl = $(".Next #NextQuestionLink").attr("href").split("?")[0].split("/");
            var elementOnPage = splitUrl[splitUrl.length - 1];
            alert(elementOnPage);
            var urlParams = Utils.GetQueryString();
            var url = "/AnswerQuestion/RenderAnswerBody/?questionId=" + $("#questionId").val() + "&pager=" + urlParams.pager + "&elementOnPage=" + elementOnPage;
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