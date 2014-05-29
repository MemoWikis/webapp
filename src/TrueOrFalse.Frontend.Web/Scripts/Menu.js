var Menu = (function () {
    function Menu() {
        $("#mainMenu .list-group-item").hover(function () {
            $(this).find(".show-on-hover").show(150);
        }, function () {
            $(this).find(".show-on-hover").hide(150);
        });
    }
    return Menu;
})();

var MenuMobile = (function () {
    function MenuMobile() {
        var _this = this;
        this._isOpen = false;
        this._isInProgress = false;
        $("#MenuButton").click(function () {
            if (_this._isOpen)
                _this.closeMenu();
            else
                _this.openMenu();
        });

        //close on click outside the menu
        $(document).mouseup(function (e) {
            if (!this._isOpen)
                return;

            if ($("#mainMenu").has(e.target).length === 0 && $("#MenuButton").has(e.target).length === 0) {
                this.closeMenu();
            }
        });

        //close on ESC
        $(document).keyup(function (e) {
            if (!_this._isOpen)
                return;

            if (e.keyCode == 27) {
                _this.closeMenu();
            }
        });
    }
    MenuMobile.prototype.openMenu = function () {
        if (this._isInProgress)
            return;

        this._isInProgress = true;
        $("#mainMenu").slideDown();
        this._isOpen = true;
        this._isInProgress = false;
    };

    MenuMobile.prototype.closeMenu = function () {
        if (this._isInProgress)
            return;

        this._isInProgress = true;
        $("#mainMenu").slideUp();
        this._isOpen = false;
        this._isInProgress = false;
    };
    return MenuMobile;
})();

$(function () {
    new MenuMobile();
    new Menu();
});
//# sourceMappingURL=Menu.js.map
