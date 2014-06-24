var CategoryApi = (function () {
    function CategoryApi() {
    }
    CategoryApi.NavigatoTo = function (categoryName) {
        $.get("/Api/Category/GetUrl?categoryName=" + categoryName, function (data) {
            window.location = data;
        });
    };
    return CategoryApi;
})();
//# sourceMappingURL=CategoryApi.js.map
