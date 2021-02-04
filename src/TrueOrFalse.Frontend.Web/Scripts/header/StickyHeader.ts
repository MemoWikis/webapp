
class StickyHeaderClass {
    private _breadcrumb;
    private _rightMainMenu;
    private _header;
    private _masterHeaderOuterHeight = $("#MasterHeader").outerHeight();
    private _stickyHeaderisFixed = true;
    private _breadCrumbDistanceRight: string;
    private _breadCrumbContainerCount: number;
    private _breadCrumbContainerElementsCopy: JQuery;
    private _breadCrumbCounter = 0;
    private _isChevronHide = false;
    private _isAddEllipsis = false;
    private _isFirstLoaded = false;
    private _breadCrumbFirstElementWidth: number;
    private _blockSecondIteration = false;

    constructor() {
        this._breadcrumb = $('#Breadcrumb').get(0);
        this._rightMainMenu = $("#RightMainMenu").get(0);
        this._header = $("#MasterHeader").get(0);
        this._breadCrumbContainerElementsCopy = $("#BreadCrumbTrail > div").clone();
        this._breadCrumbContainerCount = this._breadCrumbContainerElementsCopy.length;
        this._breadCrumbFirstElementWidth = parseInt($('#BreadCrumbTrail > div:eq(0)').css("width"));
        this.firstLoad();

        $("#UserWorldButton").on("click", () => {
            $.post("/User/SetUserWorldInUserCache"); 
        }); 

        $(window).scroll(() => {
            this.stickyHeader();
        });

        $(window).resize(() => {
            this.stickyHeader();
            // this.computePositionBreadCrumb();
            if (window.scrollY < this._masterHeaderOuterHeight) {
                this._breadcrumb.style.top = ($("#MasterHeader").outerHeight()) + "px";
                this.positioningMenus($("#RightMainMenu"), false);
                this.positioningMenus($("#userDropdown"), false);
                this.computeBreadcrumb(70);
            } else {
                this.positioningMenus($("#RightMainMenu"), true);
                this.positioningMenus($("#BreadcrumbUserDropdown"), true);
                this.computeBreadcrumb(260);
            }
        });
    }

    public stickyHeader() {

        if ($(window).scrollTop() >= this._masterHeaderOuterHeight) {
            if (this._stickyHeaderisFixed)
                return;

            this.positioningMenus($("#BreadcrumbUserDropdown"), true);
            this.positioningMenus($("#RightMainMenu"), true);

            this._breadcrumb.style.top = "0";
            this._breadcrumb.classList.add("ShowBreadcrumb");
            $("#Breadcrumb").css("position", "fixed");

            this._stickyHeaderisFixed = true;

            $('#StickyHeaderContainer').css('display', 'flex');


            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");

            this._rightMainMenu.style.position = "fixed";

            if (top.location.pathname !== "/")
                this.computeBreadcrumb(260);

        } else if ($(window).scrollTop() < this._masterHeaderOuterHeight) {

            if (!this._stickyHeaderisFixed)
                return;

            this.positioningMenus($("#RightMainMenu"), false);
            this.positioningMenus($("#userDropdown"), false);
            this._stickyHeaderisFixed = false;
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight() + this._header.scrollTop) + "px";
            this._breadcrumb.style.position = "absolute";
            this._breadcrumb.classList.remove("ShowBreadcrumb");
            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");
            this.computeBreadcrumb(70);

            $('#StickyHeaderContainer').hide();
            this._rightMainMenu.style.position = "absolute";

            if (top.location.pathname === "/") {
                this._breadcrumb.style.display = "none";
            }
        }
    }

    private firstLoad() {
        if ($(window).scrollTop() >= this._masterHeaderOuterHeight) {

            this.positioningMenus($("#RightMainMenu"), true);

            this._breadcrumb.style.top = "0";
            this._breadcrumb.classList.add("ShowBreadcrumb");
            this._breadcrumb.style.position = "fixed";

            this._stickyHeaderisFixed = true;

            $('#StickyHeaderContainer').css('display', 'flex');
            $("#BreadcrumbUserDropdown").css("margin-top", "0");


            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");

            this._rightMainMenu.style.position = "fixed";

            if (top.location.pathname !== "/")
                this.computeBreadcrumb(260);
        } else {

            this.positioningMenus($("#RightMainMenu"), false);
            this.positioningMenus($("#userDropdown"), false);
            this._stickyHeaderisFixed = false;
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight()) + "px";
            this._breadcrumb.style.position = "absolute";
            this._breadcrumb.classList.remove("ShowBreadcrumb");
            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");

            $("#BreadcrumbUserDropdown").css("margin-top", "0");
            $('#StickyHeaderContainer').hide();

            if (top.location.pathname === "/")
                this._breadcrumb.style.display = "none";
            if (top.location.pathname !== "/")
                this.computeBreadcrumb(30);

        }

        this._isFirstLoaded = true;
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
        else if (window.innerWidth < 768 && window.innerWidth > 580) {
            menu.css("top",
                $("#MasterHeader").outerHeight() -
                parseInt($(".HeaderMainRow").css("margin-top")) -
                parseInt($("#loginAndHelp").css("margin-top"))) +
                "px";
        } else if (window.innerWidth < 581) {

            menu.css("top", $(".col-LoginAndHelp").outerHeight() + parseInt($(".col-LoginAndHelp").css("margin-bottom")) - 1 + "px");
        }
    }

    public computeBreadcrumb(widthStickyHeaderContainer: number) {

        var breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) + widthStickyHeaderContainer;
        var masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width"));

        this.computeFirstElementFadeIn(breadCrumbTrailWidth, masterMainWrapperInnerWidth);
        breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) + widthStickyHeaderContainer;

        while (breadCrumbTrailWidth > masterMainWrapperInnerWidth && this._breadCrumbCounter < this._breadCrumbContainerCount - 1) {
            this._breadCrumbCounter++;

            $('#BreadCrumbTrail > div').eq(this._breadCrumbCounter).hide();
            $('#BreadCrumbTrail > div').eq(this._breadCrumbCounter).addClass("none");

            if (!this._isAddEllipsis) {
                this.addEllipsis();
                this._breadCrumbCounter++;
                this._breadCrumbContainerCount++;
            } else if (this._isAddEllipsis) {
                $('#BreadCrumbTrail > div:eq(1)').fadeIn();
            }
            if (this._breadCrumbCounter > 1)
                $("#Path").append(this._breadCrumbContainerElementsCopy[this._breadCrumbCounter - 1]);
            breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) + widthStickyHeaderContainer;
        }

        this.computeWhenBreadCrumbTrailIsToLow(breadCrumbTrailWidth, masterMainWrapperInnerWidth);

        this.computeFirstElementHide(breadCrumbTrailWidth,
            masterMainWrapperInnerWidth,
            widthStickyHeaderContainer);
        this.hideAndFadeInChevron();

        if (typeof $("#BreadCrumbTrail > div:eq(1)").offset() !== "undefined")
            $("#Path").css("left", $("#BreadCrumbTrail > div:eq(1)").offset().left);
    }

    private computeFirstElementHide(breadCrumbTrailWidth: number, masterMainWrapperInnerWidth: number, widthStickyHeaderContainer: number) {
        if ($('#BreadCrumbTrail >div:eq(0)').hasClass("none"))
            breadCrumbTrailWidth -= this._breadCrumbFirstElementWidth;

        if (breadCrumbTrailWidth > masterMainWrapperInnerWidth && this._breadCrumbCounter === this._breadCrumbContainerCount - 1) {
            $('#BreadCrumbTrail > div').eq(0).hide();
            $('#BreadCrumbTrail > div').eq(0).addClass("none");
            $("#Path").prepend(this._breadCrumbContainerElementsCopy[0]);
            if (!this._isAddEllipsis)
                this.addEllipsis();
        }

        if ($("#Path div").length > 0)
            $("#PathMobileBreadCrumb").fadeIn();
    }

    private computeFirstElementFadeIn(breadCrumbTrailWidth, masterMainWrapperInnerWidth) {
        breadCrumbTrailWidth += this._breadCrumbFirstElementWidth;

        if (breadCrumbTrailWidth < masterMainWrapperInnerWidth &&
            this._breadCrumbCounter === this._breadCrumbContainerCount - 1 && $("#BreadCrumbTrail > div:eq(0)").hasClass("none")) {
            $('#BreadCrumbTrail > div:eq(0)').fadeIn();
            $('#BreadCrumbTrail > div').eq(0).removeClass("none");
            $("#Path > div:eq(0)").remove();
        }
        if ($("#Path div").length < 1) {
            $("#PathMobileBreadCrumb").hide();
        }
    }

    private computeWhenBreadCrumbTrailIsToLow(breadCrumbTrailWidth: number, masterMainWrapperInnerWidth: number) {
        if (breadCrumbTrailWidth <= masterMainWrapperInnerWidth && !$("#BreadCrumbTrail > div:eq(0)").hasClass("none")) {
            while (this._breadCrumbCounter >= 1) {
                breadCrumbTrailWidth += parseInt($("#BreadCrumbTrail > div").eq(this._breadCrumbCounter).css("width"));

                if (breadCrumbTrailWidth <= masterMainWrapperInnerWidth) {
                    $("#BreadCrumbTrail > div").eq(this._breadCrumbCounter).fadeIn();

                    $("#Path > div").last().remove();
                    if (this._breadCrumbCounter !== 1) {
                        this._breadCrumbCounter--;
                    }
                    if (this._breadCrumbCounter === 1) {
                        $("#PathMobileBreadCrumb").hide();
                        this._breadCrumbCounter--;
                    }

                } else {
                    breadCrumbTrailWidth -= parseInt($("#BreadCrumbTrail > div").eq(this._breadCrumbCounter)
                        .css("width"));
                    break;
                }
            }
        }
    }

    private computePositionBreadCrumb() {
        if ($(window).scrollTop() <= 84)
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight()) + "px";
    }

    private hideAndFadeInChevron() {
        if (!$("#BreadCrumbTrail > div:eq(0)").hasClass("none")) {
            if (this._breadCrumbCounter === this._breadCrumbContainerCount - 1 && !this._isChevronHide) {
                $('#BreadCrumbTrail > div').eq(1).children().eq(1).hide();
                this._isChevronHide = true;
            } else if (this._breadCrumbCounter === this._breadCrumbContainerCount - 2 && this._isChevronHide) {
                $('#BreadCrumbTrail > div').eq(1).children().eq(1).fadeIn();
                this._isChevronHide = false;
            }
        } else {
            $('#BreadCrumbTrail > div').eq(1).children().eq(1).hide();
        }
    }

    private addEllipsis() {
        $('#BreadCrumbTrail > div:eq(0)')
            .after(
            '<div id="PathMobileBreadCrumb" class="path" style="font-size: 14px;" ><span class="fas fa-ellipsis-h" style="margin-left: 10px;"></span><i class="fa fa-chevron-right"></i></div>');
        this._isAddEllipsis = true;
    }
}

$(() => {
    if (!Utils.IsInWidget())
        var s = new StickyHeaderClass();
});