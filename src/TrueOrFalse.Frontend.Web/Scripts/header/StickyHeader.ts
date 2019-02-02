// Todos



class StickeyHeaderClass {
    private Breadcrumb;
    private RightMainMenu;   
    private Header;
    private OuterHeightBreadCrumb;
    private UserDropDownMenuBreadCrumb;


    constructor() {
        this.Breadcrumb = $('#Breadcrumb').get(0);
        this.RightMainMenu = $("#RightMainMenu").get(0);
        this.Header = $("#MasterHeader").get(0);
        this.OuterHeightBreadCrumb = $("#Breadcrumb").outerHeight();
        this.UserDropDownMenuBreadCrumb = $("#BreadcrumbUserDropdown");
        this.RightMainMenu.style.position = "fixed"; 
        this.calculateTheSizeOfTheMenu($("#RightMainMenu"));
        this.calculateTheSizeOfTheMenu($("#userDropdown"));
        this.StickyHeader();

        $("#userDropdown").css("top", $("#Breadcrumb").outerHeight() + $("#MasterHeader").outerHeight() - $("#HeaderUserDropdown").offset().top + "px");

        $(window).scroll(() => {
                this.StickyHeader();
        });

        $(window).resize(() => {
            this.StickyHeader();
            this.calculateTheSizeOfTheMenu($("#RightMainMenu"));
            this.calculateTheSizeOfTheMenu($("#userDropdown"));
        });
    }

    public StickyHeader() {
        
        if ($(window).scrollTop() > 80) {
            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $("#BreadcrumbUserDropdown").css("top", $("#Breadcrumb").outerHeight() + "px");
            $('#BreadCrumbTrail').css('max-width', "51%");

            this.toggleClass($("#HeaderUserDropdown"), $("#BreadcrumbUserDropdownImage"), "open");
            this.calculateTheSizeOfTheMenu($("#BreadcrumbUserDropdown"));
            

            this.RightMainMenu.style.top = ($("#Breadcrumb").outerHeight() + "px");
            this.Breadcrumb.style.zIndex = 100;
            this.Breadcrumb.style.top = "0";
            this.Breadcrumb.classList.add("ShowBreadcrumb");
            this.Breadcrumb.style.position = "fixed";
        }
        else {
            this.Breadcrumb.style.top = ($("#MasterHeader").outerHeight() + this.Header.scrollTop) + "px";
            this.Breadcrumb.style.position = "absolute";
            this.Breadcrumb.classList.remove("ShowBreadcrumb");

            this.toggleClass($("#BreadcrumbUserDropdownImage"), $("#HeaderUserDropdown"), "open");

            $("#userDropdown").css("top", $("#Breadcrumb").outerHeight() + $("#MasterHeader").outerHeight() - $("#HeaderUserDropdown").offset().top + "px");
            $("#Breadcrumb").css("z-index", 100);
            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();

            this.RightMainMenu.style.top = ($("#MasterHeader").outerHeight() + $("#Breadcrumb").outerHeight() + "px");

            $('#BreadCrumbTrail').css("max-width", "");

            if (top.location.pathname === "/") {
                this.Breadcrumb.style.display = "none";
            }
        }

        if (this.countLines(this.Breadcrumb) !== 1) 
            this.Breadcrumb.style.height = "auto";
        else
            this.Breadcrumb.style.height = "55px";                                                      // Warum geht hier Auto nicht , bearbeiten , theoretisch muss Höhe doch nicht festgelegt werden  

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
        if (pos > 80) {
            $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        } else {
            $('#BreadcrumbUserDropdown').css('margin-right', '');
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

}

$(() => {
    new StickeyHeaderClass(); 
});