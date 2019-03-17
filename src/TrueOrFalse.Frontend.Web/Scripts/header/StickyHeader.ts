
class StickyHeaderClass {
    private _breadcrumb;
    private _rightMainMenu;
    private _header;
    private _masterHeaderOuterHeight = $("#MasterHeader").outerHeight();
    private _stickyHeaderisFixed = true;
    private _breadCrumbDistanceRight: string;


    constructor() {
        this._breadcrumb = $('#Breadcrumb').get(0);
        this._rightMainMenu = $("#RightMainMenu").get(0);
        this._header = $("#MasterHeader").get(0);
        this._rightMainMenu.style.position = "absolute";
        this._breadcrumb.style.height = "55px";

       // this.StickyHeader();
        this.firstLoad();

        $(window).scroll(() => {
            this.StickyHeader();
        });

        $(window).resize(() => {
            this.StickyHeader();
            if (window.scrollY < this._masterHeaderOuterHeight) {
                this.positioningMenus($("#RightMainMenu"), false);
                this.positioningMenus($("#userDropdown"), false);
            } else {
                this.positioningMenus($("#RightMainMenu"), true);
                this.positioningMenus($("#BreadcrumbUserDropdown"), true);
            }
        });
    }

    public StickyHeader() {

        if ($(window).scrollTop() >= this._masterHeaderOuterHeight) {

            if (this._stickyHeaderisFixed) 
                return;

            this.positioningMenus($("#BreadcrumbUserDropdown"), true);
            this.positioningMenus($("#RightMainMenu"), true);

            this._breadcrumb.style.top = "0";
            this._breadcrumb.classList.add("ShowBreadcrumb");
            this._breadcrumb.style.position = "fixed";

            this._stickyHeaderisFixed = true;

            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');


            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");

            this._rightMainMenu.style.position = "fixed";

        } else if ($(window).scrollTop() < this._masterHeaderOuterHeight) {

            if (!this._stickyHeaderisFixed ) 
                return;
        
            this.positioningMenus($("#RightMainMenu"), false);
            this.positioningMenus($("#userDropdown"), false);
            this._stickyHeaderisFixed = false;
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight() + this._header.scrollTop) + "px";
            this._breadcrumb.style.position = "absolute";
            this._breadcrumb.classList.remove("ShowBreadcrumb");
            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");


            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();
            this._rightMainMenu.style.position = "absolute";

            if (top.location.pathname === "/") {
                this._breadcrumb.style.display = "none";
            }
        }

        this._breadcrumb.style.height = "55px";

    }

    private firstLoad() {
        if ($(window).scrollTop() >= this._masterHeaderOuterHeight) {

            this.positioningMenus($("#BreadcrumbUserDropdown"), true);
            this.positioningMenus($("#RightMainMenu"), true);
            

            this._breadcrumb.style.top = "0";
            this._breadcrumb.classList.add("ShowBreadcrumb");
            this._breadcrumb.style.position = "fixed";

            this._stickyHeaderisFixed = true;

            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $("#BreadcrumbUserDropdown").css("margin-top", "0");


            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");

            this._rightMainMenu.style.position = "fixed";
            

        } else {

            this.positioningMenus($("#RightMainMenu"), false);
            this.positioningMenus($("#userDropdown"), false);
           

            this._stickyHeaderisFixed = false;
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight() + this._header.scrollTop) + "px";
            this._breadcrumb.style.position = "absolute";
            this._breadcrumb.classList.remove("ShowBreadcrumb");
            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");
            $("#BreadcrumbUserDropdown").css("margin-top", "0");


            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();
            this._rightMainMenu.style.position = "absolute";

            if (top.location.pathname === "/") {
                this._breadcrumb.style.display = "none";
            }
        }
    }


    private positioningMenus(menu: JQuery, isScrollGreather: boolean) {
        if (!isScrollGreather && menu.selector !== "#userDropdown") 
            menu.css("top", $("#MasterHeader").outerHeight());
        else if (!isScrollGreather && menu.selector === "#userDropdown") {
            this.computePositionUserDropDownNoScroll($("#userDropdown"));
        }
        else 
            menu.css("top", $("#Breadcrumb").outerHeight());
           
        
    }

    private toggleClass(removeClassFromElement: JQuery, addClassToElement: JQuery, toggleClass: string) {
        if (removeClassFromElement.hasClass(toggleClass)) {
            removeClassFromElement.removeClass(toggleClass);
            addClassToElement.addClass(toggleClass);
        }
    }

    private computePositionUserDropDownNoScroll(menu: JQuery) {

        if (window.innerWidth > 768) {
            menu.css("top",
                $("#MasterHeader").outerHeight() -
                parseInt($(".col-LoginAndHelp").css("margin-top")) -
                parseInt($(".HeaderMainRow").css("margin-top"))) +
                "px";
        }
        else if (window.innerWidth < 768 && window.innerWidth > 580 ) {
            menu.css("top",
                $("#MasterHeader").outerHeight() -
                parseInt($(".HeaderMainRow").css("margin-top")) -
                parseInt($("#loginAndHelp").css("margin-top"))) +
                "px";
        } else if (window.innerWidth < 581) {
           
            menu.css("top", $(".col-LoginAndHelp").outerHeight() + parseInt($(".col-LoginAndHelp").css("margin-bottom")) - 1    + "px");
        }
    }
}


class ComputeBradCrumb {
    private _masterMainWrapperInnerWidth: number;
    private _breadCrumbTrailWidth: number;
    private _breadCrumbContainerCount: number;


    constructor() {
        this._masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width")) - 230;
        debugger;
        this._breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width"));
        this._breadCrumbContainerCount = $('#BreadCrumbTrail').children().length - 3;
    }

    public computeBreadcrumb(widthStickyHeaderContainer: number) {
        var i = 1;
        var j = 1;

        if (this._breadCrumbTrailWidth > this._masterMainWrapperInnerWidth) {
            $('#BreadCrumbTrail > div').eq(1).attr('title',
                    "Zur Themenseite " + $(i + "BreadCrumb").text())
                .tooltip('fixTitle');
            $('#BreadCrumbTrail > div').eq(1).children("span").children("a").text("...");
            this._masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width")) - widthStickyHeaderContainer;
        }

        while (this._breadCrumbTrailWidth > this._masterMainWrapperInnerWidth && i <= this._breadCrumbContainerCount) {
            try {
                $('#' + i + 'BreadCrumbContainer').attr('title',
                        "Zur Themenseite " + $(i + "BreadCrumb").text())
                    .tooltip('fixTitle');
                document.getElementById(i + 'BreadCrumb').innerHTML = "...";
                this._breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) - widthStickyHeaderContainer;
                i++;
            } catch (e) {
                console.log("incorrect counter Breadcrumb");
                break;
            }
        }
    }
}

$(() => {
    var s = new StickyHeaderClass();
    s.StickyHeader();
    var c = new ComputeBradCrumb();
    c.computeBreadcrumb(230);


});