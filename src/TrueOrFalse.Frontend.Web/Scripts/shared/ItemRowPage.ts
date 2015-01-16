declare var _itemRowPage: ItemRowPage;

class ItemRowPage {

    Init() {
        fnInitImageDetailModal($('.JS-ImageDetailModal'));
    }
}

$(function () {
    _itemRowPage = new ItemRowPage();
    _itemRowPage.Init();
});
