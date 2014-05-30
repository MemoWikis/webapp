var QuestionsApi = (function () {
    function QuestionsApi() {
    }
    QuestionsApi.Pin = function (questionId) {
        $.post("/Api/Questions/Pin/", { questionId: questionId });
    };

    QuestionsApi.Unpin = function (questionId) {
        $.post("/Api/Questions/Unpin/", { questionId: questionId });
    };
    return QuestionsApi;
})();
//# sourceMappingURL=QuestionsApi.js.map
