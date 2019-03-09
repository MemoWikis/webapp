
class StickyHeaderClass {
    private _breadcrumb;
    private _rightMainMenu;
    private _header;
    private _masterHeaderOuterHeight = $("#MasterHeader").outerHeight();
    private _stickyHeaderisFixed = false;
    private _breadCrumbDistanceRight: string;

    constructor() {
        this._breadcrumb = $('#Breadcrumb').get(0);
        this._rightMainMenu = $("#RightMainMenu").get(0);
        this._header = $("#MasterHeader").get(0);
        this._rightMainMenu.style.position = "absolute";
       
        this.StickyHeader();
        if (IsLoggedIn.Yes)
            $("#userDropdown").css("top", $("#Breadcrumb").outerHeight() + $("#MasterHeader").outerHeight() - $("#HeaderUserDropdown").offset().top + "px");

        $(window).scroll(() => {
            this.StickyHeader();
        });

        $(window).resize(() => {
            this.StickyHeader();
        });
    }

    public StickyHeader() {

        if ($(window).scrollTop() >= this._masterHeaderOuterHeight) {
            if (IsLoggedIn.Yes)
                this.positioningMenus($("#userDropdown"), true);

            this.positioningMenus($("#RightMainMenu"), true);

            if (this._stickyHeaderisFixed) {
                return;
            }
            this._breadcrumb.style.zIndex = "100";
            this._breadcrumb.style.top = "0";
            this._breadcrumb.classList.add("ShowBreadcrumb");
            this._breadcrumb.style.position = "fixed";

            this._stickyHeaderisFixed = true;

            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');


            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");

            this._rightMainMenu.style.position = "fixed";

        } else {
            this.positioningMenus($("#userDropdown"), false);
            this.positioningMenus($("#RightMainMenu"), false);


            if (!this._stickyHeaderisFixed) {
                return;
            }

            this._stickyHeaderisFixed = false;
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight() + this._header.scrollTop) + "px";
            this._breadcrumb.style.position = "absolute";
            this._breadcrumb.classList.remove("ShowBreadcrumb");
            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");


            $("#Breadcrumb").css("z-index", 100);
            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();
            this._rightMainMenu.style.position = "absolute";

            if (top.location.pathname === "/") {
                this._breadcrumb.style.display = "none";
            }
        }

        this._breadcrumb.style.height = "55px";
    }

    private positioningMenus(menu: JQuery, isScrollGreather: boolean) {
        if (!isScrollGreather && top.location.pathname !== "/")
            menu.css("top", $("#MasterHeader").outerHeight());
        else {
            menu.css("top", $("#Breadcrumb").outerHeight());
           
        }
    }

    private toggleClass(removeClassFromElement: JQuery, addClassToElement: JQuery, toggleClass: string) {
        if (removeClassFromElement.hasClass(toggleClass)) {
            removeClassFromElement.removeClass(toggleClass);
            addClassToElement.addClass(toggleClass);
        }
    }

}

$(() => {
    var s = new StickyHeaderClass();
    s.StickyHeader();

});