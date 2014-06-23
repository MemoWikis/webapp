var SetsApi = (function () {
    function SetsApi() {
    }
    SetsApi.Pin = function (setId) {
        $.post("/Api/Sets/Pin/", { setId: setId });
    };

    SetsApi.Unpin = function (setId) {
        $.post("/Api/Sets/Unpin/", { setId: setId });
    };
    return SetsApi;
})();
//# sourceMappingURL=SetsApi.js.map
