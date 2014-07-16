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
            if (_this._isOpen) {
                _this.closeMenu();
            } else {
                _this.openMenu();
            }
        });

        //close on click outside the menu
        $(document).mouseup(function (e) {
            if (!_this._isOpen && !_this._isInProgress) {
                return;
            }

            if ($("#mainMenu").has(e.target).length === 0 && $("#MenuButton").has(e.target).length === 0) {
                _this.closeMenu();
            }
        });

        //close on ESC
        $(document).keyup(function (e) {
            if (!_this._isOpen && !_this._isInProgress) {
                return;
            }

            if (e.keyCode == 27) {
                _this.closeMenu();
            }
        });
    }
    MenuMobile.prototype.openMenu = function () {
        var _this = this;
        if (this._isInProgress) {
            return;
        }

        this._isInProgress = true;
        $("#mainMenu").slideDown(400, function () {
            _this._isOpen = true;
            _this._isInProgress = false;
        });
    };

    MenuMobile.prototype.closeMenu = function () {
        var _this = this;
        if (this._isInProgress) {
            return;
        }

        this._isInProgress = true;
        $("#mainMenu").slideUp(400, function () {
            _this._isOpen = false;
            _this._isInProgress = false;
        });
    };
    return MenuMobile;
})();

$(function () {
    new MenuMobile();
    new Menu();
});
//# sourceMappingURL=Menu.js.map
