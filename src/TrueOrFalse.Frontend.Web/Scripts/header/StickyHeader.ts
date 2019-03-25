
class StickyHeaderClass {
    private _breadcrumb;
    private _rightMainMenu;
    private _header;
    private _masterHeaderOuterHeight = $("#MasterHeader").outerHeight();
    private _stickyHeaderisFixed = true;
    private _breadCrumbDistanceRight: string;
    private _breadCrumbContainerCount: number;
    private _breadCrumbContainerElementsCopy;
    private _breadCrumbCounter = 1;
    private _isChevronHide = false;
    private _isAddEllipsis = false;
    private _isFirstLoaded = false;

    constructor() {
        this._breadcrumb = $('#Breadcrumb').get(0);
        this._rightMainMenu = $("#RightMainMenu").get(0);
        this._header = $("#MasterHeader").get(0);
        this._rightMainMenu.style.position = "absolute";
       // this._breadcrumb.style.height = "55px";
        this._breadCrumbContainerElementsCopy = $("#BreadCrumbTrail > div").clone();
        this._breadCrumbContainerCount = this._breadCrumbContainerElementsCopy.length;
        this.firstLoad();
        $(window).scroll(() => {
            this.StickyHeader();
        });

        $(window).resize(() => {
            this.StickyHeader();
            this.computePositionBreadCrumb();
            if (window.scrollY < this._masterHeaderOuterHeight) {
                this._breadcrumb.style.top = ($("#MasterHeader").outerHeight()) + "px";
                this.positioningMenus($("#RightMainMenu"), false);
                this.positioningMenus($("#userDropdown"), false);
                this.computeBreadcrumb(30, "resize");
            } else {
                this.positioningMenus($("#RightMainMenu"), true);
                this.positioningMenus($("#BreadcrumbUserDropdown"), true);
                this.computeBreadcrumb(250, "resize");
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

            if (top.location.pathname !== "/")
                this.computeBreadcrumb(300);

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
            this.computeBreadcrumb(70, "scrollDown");

            $('#BreadcrumbLogoSmall').hide();
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

            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $("#BreadcrumbUserDropdown").css("margin-top", "0");


            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");

            this._rightMainMenu.style.position = "fixed";

            if (top.location.pathname !== "/") 
                this.computeBreadcrumb(300);
        } else {

            this.positioningMenus($("#RightMainMenu"), false);
            this.positioningMenus($("#userDropdown"), false);
            this._stickyHeaderisFixed = false;
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight()) + "px";
            this._breadcrumb.style.position = "absolute";
            this._breadcrumb.classList.remove("ShowBreadcrumb");
            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");
            this._rightMainMenu.style.position = "absolute";

            if (top.location.pathname !== "/")
                this.computeBreadcrumb(70);

            $("#BreadcrumbUserDropdown").css("margin-top", "0");
            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();

            if (top.location.pathname === "/") 
                this._breadcrumb.style.display = "none";
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

    public computeBreadcrumb(widthStickyHeaderContainer: number, ResizeOrScroll: string = "") {

        var breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) + widthStickyHeaderContainer;
        var masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width"));
        var isBreadCrumbTrailWidthToBig = false;

        if (breadCrumbTrailWidth > masterMainWrapperInnerWidth)
            isBreadCrumbTrailWidthToBig = true;

        while (breadCrumbTrailWidth > masterMainWrapperInnerWidth && this._breadCrumbCounter < this._breadCrumbContainerCount) {
            try {
                //if (this._isFirstLoaded) {
                //    this._isFirstLoaded = false;                                                // block second Iteration
                //    this._breadCrumbCounter++;
                //}
                if (isBreadCrumbTrailWidthToBig && !this._isAddEllipsis) {
                    $('#BreadCrumbTrail > div:eq(0)')
                        .after('<div id="PathMobileBreadCrumb" class="path" style="font-size: 14px;" ><span class="fas fa-ellipsis-h" style="margin-left: 10px;"></span><i class="fa fa-chevron-right"></i></div>');
                    this._isAddEllipsis = true;
                    this._breadCrumbContainerCount = $("#BreadCrumbTrail > div").length -1;
                    this._breadCrumbCounter++;
                }

                $('#BreadCrumbTrail > div').eq(this._breadCrumbCounter).hide();
                $('#BreadCrumbTrail > div').eq(this._breadCrumbCounter).addClass("none");

                breadCrumbTrailWidth = parseInt($("#BreadCrumbTrail").css("width")) + widthStickyHeaderContainer;
                $("#Path").append(this._breadCrumbContainerElementsCopy[this._breadCrumbCounter]);

                if(breadCrumbTrailWidth > masterMainWrapperInnerWidth)
                    this._breadCrumbCounter++;
                //console.log(this._breadCrumbCounter);
            } catch (e) {
                console.log("incorrect counter Breadcrumb");
                break;
            }
        }


        
        //masterMainWrapperInnerWidth = parseInt($("#MasterMainContent").css("width"));

        
        if (breadCrumbTrailWidth < masterMainWrapperInnerWidth) {
            

            var iterationBool = true;
            //console.log(this._breadCrumbCounter);

            while (this._breadCrumbCounter >= 2) {
                breadCrumbTrailWidth += parseInt($("#BreadCrumbTrail > div").eq(this._breadCrumbCounter).css("width"));

                if (breadCrumbTrailWidth < masterMainWrapperInnerWidth) {
                    $("#BreadCrumbTrail > div").eq(this._breadCrumbCounter).fadeIn();
                    if (this._breadCrumbCounter !== 2)
                        this._breadCrumbCounter--;
                    else {
                        $("#PathMobileBreadCrumb").remove();
                        this._isAddEllipsis = false;
                    }

                } else {
                    breadCrumbTrailWidth -= parseInt($("#BreadCrumbTrail > div").eq(this._breadCrumbCounter)
                        .css("width"));
                    break;
                }
            }
        }

        if (this._breadCrumbCounter === this._breadCrumbContainerCount + 1 && !this._isChevronHide)
            $('#BreadCrumbTrail > div').eq(1).children().eq(1).hide();


        var breadCrumbPath = $("#BreadCrumbTrail > div:eq(1)").offset().left;

        $("#Path").css("left", breadCrumbPath);

    }


    private computePositionBreadCrumb() {
        if($(window).scrollTop() <= 84)
            this._breadcrumb.style.top = ($("#MasterHeader").outerHeight()) + "px";
    }
}


$(() => {
    var s = new StickyHeaderClass();
   
});