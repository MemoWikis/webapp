
class StickyHeaderClass {
    private _breadcrumb;
    private _rightMainMenu;   
    private _header;
    private _masterHeaderOuterHeight = $("#MasterHeader").outerHeight();
    private  _stickyHeaderisFixed = false;

    constructor() {
        this._breadcrumb = $('#Breadcrumb').get(0);
        this._rightMainMenu = $("#RightMainMenu").get(0);
        this._header = $("#MasterHeader").get(0);
        this._rightMainMenu.style.position = "absolute"; 
        this.doubleStuff();
        this.StickyHeader();
        if (IsLoggedIn.Yes)
            $("#userDropdown").css("top", $("#Breadcrumb").outerHeight() + $("#MasterHeader").outerHeight() - $("#HeaderUserDropdown").offset().top + "px");

        $(window).scroll(() => {
                this.StickyHeader();
        });

        $(window).resize(() => {
            this.StickyHeader();
            this.doubleStuff();
        });
    }

    public StickyHeader() {
        if (IsLoggedIn.Yes)
            $("#BreadcrumbUserDropdown").css("top", $("#Breadcrumb").outerHeight() + "px");
            this._rightMainMenu.style.top = ($("#Breadcrumb").outerHeight() + "px")

        if ($(window).scrollTop() >= this._masterHeaderOuterHeight) {


            if (this._stickyHeaderisFixed) {
                return;
            }
            this._breadcrumb.style.zIndex = 100;
            this._breadcrumb.style.top = "0";
            this._breadcrumb.classList.add("ShowBreadcrumb");
            this._breadcrumb.style.position = "fixed";

            this._stickyHeaderisFixed = true;

            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');



            $('#BreadCrumbTrail').css('max-width', "51%");

            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");
            this.calculateTheSizeOfTheMenu($("#BreadcrumbUserDropdown"));

            this._rightMainMenu.style.position = "fixed";
            ;



        } else {

            //if (!this._stickyHeaderisFixed) {
            //    return;
            //} 

                this._stickyHeaderisFixed = false;
                this._breadcrumb.style.top = ($("#MasterHeader").outerHeight() + this._header.scrollTop) + "px";
                this._breadcrumb.style.position = "absolute";
                this._breadcrumb.classList.remove("ShowBreadcrumb");
                this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");

                if (IsLoggedIn.Yes)
                    $("#userDropdown").css("top",
                        $("#Breadcrumb").outerHeight() +
                        $("#MasterHeader").outerHeight() -
                        $("#HeaderUserDropdown").offset().top +
                        "px");

                $("#Breadcrumb").css("z-index", 100);
                $('#BreadcrumbLogoSmall').hide();
                $('#StickyHeaderContainer').hide();

                this._rightMainMenu.style.top = ($("#MasterHeader").outerHeight() +
                    $("#Breadcrumb").outerHeight() -
                    $("#MenuButtonContainer").offset().top +
                    "px");
                this._rightMainMenu.style.position = "absolute";

                $('#BreadCrumbTrail').css("max-width", "");

                if (top.location.pathname === "/") {
                    this._breadcrumb.style.display = "none";
                }
            }

            if (this.countLines(this._breadcrumb) !== 1)
                this._breadcrumb.style.height = "auto";
            else
                this._breadcrumb.style.height =
                    "55px"; // Warum geht hier Auto nicht , bearbeiten , theoretisch muss Höhe doch nicht festgelegt werden  
            if (IsLoggedIn.Yes)
                this.reorientatedMenu($(window).scrollTop());
    }

   private countLines(target) {

        document.getElementById("Breadcrumb").style.height = "auto";
        var style = window.getComputedStyle(target, null);
        var height = parseInt(style.getPropertyValue("height"));
        var font_size = parseInt(style.getPropertyValue("font-size"));
        var line_height = parseInt(style.getPropertyValue("line-height"));
        var box_sizing = style.getPropertyValue("box-sizing");

        if (isNaN(line_height)) line_height = font_size * 1.2;

        if (box_sizing === 'border-box') {
            var padding_top = parseInt(style.getPropertyValue("padding-top"));
            var padding_bottom = parseInt(style.getPropertyValue("padding-bottom"));
            var border_top = parseInt(style.getPropertyValue("border-top-width"));
            var border_bottom = parseInt(style.getPropertyValue("border-bottom-width"));
            height = height - padding_top - padding_bottom - border_top - border_bottom
        }
        var lines = Math.ceil(height / line_height);
        lines = lines - 1;
        return lines;
    }

   private reorientatedMenu(pos: number):void {
        if (pos > this._masterHeaderOuterHeight) {
            $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
            $('#RightMainMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        } else {
            $('#BreadcrumbUserDropdown').css('margin-right', '');
            $('#RightMainMenu').css('margin-right', '');
        }
    }

    private calculateTheSizeOfTheMenu(menu: JQuery,) {
            menu.css("max-height", $(window).innerHeight() - $("#Breadcrumb").outerHeight() + "px");
            menu.css( "overflow", "scroll");
    }

    private toggleClass(removeClassFromElement: JQuery, addClassToElement: JQuery, toggleClass: string) {
        if (removeClassFromElement.hasClass(toggleClass)) {
            removeClassFromElement.removeClass(toggleClass);
            addClassToElement.addClass(toggleClass);
        }
    }

    private doubleStuff() {
        this.calculateTheSizeOfTheMenu($("#RightMainMenu"));
        this.calculateTheSizeOfTheMenu($("#userDropdown"));
    }

}

$(() => {
    var s = new StickyHeaderClass(); 
    s.StickyHeader();

});