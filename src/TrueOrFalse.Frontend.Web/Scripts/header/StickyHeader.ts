

class StickeyHeaderClass {
    private Breadcrumb;
    private RightMainMenu;
    private Position;
    private Header;


    constructor() {
        this.Breadcrumb = $('#Breadcrumb').get(0);
        this.RightMainMenu = $("#RightMainMenu").get(0);
        this.Position = $(window).scrollTop();
        this.Header = $("#MasterHeader").get(0);
    }

    public StickyHeader() {
        var breadcrumb = $('#Breadcrumb').get(0);
        var rightMainMenu = $("#RightMainMenu").get(0);
        var position = $(window).scrollTop();
        var header = $("#MasterHeader").get(0);

        if (position > 80) {
            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $("#Breadcrumb").css("z-index", 100);


            breadcrumb.style.top = "0";
            breadcrumb.classList.add("ShowBreadcrumb");
            breadcrumb.classList.add("sticky");

            rightMainMenu.style.position = "absolute";
            rightMainMenu.style.top = "52px";

            $('#BreadCrumbTrail').css('max-width', "51%");

        } else {
            breadcrumb.style.top = (80 + header.scrollTop).toString() + "px";
            breadcrumb.style.position = "absolute";
            $("#Breadcrumb").css("z-index", 100);

            if (breadcrumb.classList.contains("ShowBreadcrumb")) breadcrumb.classList.remove("ShowBreadcrumb");

            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();

            rightMainMenu.style.position = "absolute";
            rightMainMenu.style.top = "64px";
            rightMainMenu.style.position = "absolute";

            $('#BreadCrumbTrail').css("max-width", "");

            if (breadcrumb.classList.contains("sticky")) {
                breadcrumb.classList.remove("sticky");
            }

            if (top.location.pathname === "/") {
                breadcrumb.style.display = "none";
            }

            if (window.innerWidth < 768) {
                breadcrumb.style.top = (50 + header.scrollTop).toString() + "px";
            }
        }

        if (this.countLines(breadcrumb) === 1) {
            breadcrumb.style.height = "55px";
        } else {
            breadcrumb.style.height = "auto";
        }

        this.reorientateMenu(position);
    }

   private countLines(target) {

        document.getElementById("Breadcrumb").style.height = "auto";
        var style = window.getComputedStyle(target, null);
        var height = parseInt(style.getPropertyValue("height"));
        var font_size = parseInt(style.getPropertyValue("font-size"));
        var line_height = parseInt(style.getPropertyValue("line-height"));
        var box_sizing = style.getPropertyValue("box-sizing");

        if (isNaN(line_height)) line_height = font_size * 1.2;

        if (box_sizing == 'border-box') {
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

   private reorientateMenu(pos: number):void {
        if (pos > 80) {
            $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        } else {
            $('#BreadcrumbUserDropdown').css('margin-right', '');
        }
    }
}

var SHC = new StickeyHeaderClass(); 

$(window).scroll(event => {
    SHC.StickyHeader();
});

window.onresize = event => {
    SHC.StickyHeader();
}