var ItemRowPage = (function () {
    function ItemRowPage() {
    }
    ItemRowPage.prototype.Init = function () {
        fnInitImageDetailModal($('.JS-ImageDetailModal'));
    };
    return ItemRowPage;
})();

$(function () {
    _itemRowPage = new ItemRowPage();
    _itemRowPage.Init();
});
//# sourceMappingURL=ItemRowPage.js.map
