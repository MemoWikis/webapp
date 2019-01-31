// Todos
// toggleClass function anlegen.


class StickeyHeaderClass {
    private Breadcrumb;
    private RightMainMenu;   
    private Header;
    private OuterHeightBreadCrumb;


    constructor() {
        this.Breadcrumb = $('#Breadcrumb').get(0);
        this.RightMainMenu = $("#RightMainMenu").get(0);
        this.Header = $("#MasterHeader").get(0);
        this.OuterHeightBreadCrumb = $("#Breadcrumb").outerHeight();     
        $("#userDropdown").css("top", $("#Breadcrumb").outerHeight() + $("#MasterHeader").outerHeight() - $("#HeaderUserDropdown").offset().top  + "px");



        $(window).scroll(() => {
            this.StickyHeader();
        });

        $(window).resize(() => {
            this.StickyHeader();
        });

       
    }

 public StickyHeader() {

        if ($(window).scrollTop() > 80) {
            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $("#Breadcrumb").css("z-index", 100);

            $("#BreadcrumbUserDropdown").css("top", $("#Breadcrumb").outerHeight() + "px");  



            this.Breadcrumb.style.top = "0";
            this.Breadcrumb.classList.add("ShowBreadcrumb");
            this.Breadcrumb.classList.add("sticky");

            this.RightMainMenu.style.position = "absolute";
            //this.RightMainMenu.style.top = "52px";               // überprüfen, die Größe der Breadcrumb ändert sich doch, also kann man das doch nicht fest verdrahten (fest verdrahten eh bööööse) 

            $('#BreadCrumbTrail').css('max-width', "51%");

        } else {
            $("#userDropdown").css("top", $("#Breadcrumb").outerHeight() + $("#MasterHeader").outerHeight() -20 +   "px");
            this.Breadcrumb.style.top = (80 + this.Header.scrollTop).toString() + "px";
            this.Breadcrumb.style.position = "absolute";
            $("#Breadcrumb").css("z-index", 100);

            if (this.Breadcrumb.classList.contains("ShowBreadcrumb")) this.Breadcrumb.classList.remove("ShowBreadcrumb");

            $('#BreadcrumbLogoSmall').hide();
            $('#StickyHeaderContainer').hide();

            // this.RightMainMenu.style.position = "absolute";
            this.RightMainMenu.style.top = ($("#MasterHeader").outerHeight() + $("#Breadcrumb").outerHeight() - $("#MenuButtonContainer").offset().top + "px");
            this.RightMainMenu.style.position = "absolute";

            $('#BreadCrumbTrail').css("max-width", "");

            if (this.Breadcrumb.classList.contains("sticky")) {
                this.Breadcrumb.classList.remove("sticky");
            }

            if (top.location.pathname === "/") {
                this.Breadcrumb.style.display = "none";
            }

            if (window.innerWidth < 768) {
                this.Breadcrumb.style.top = (50 + this.Header.scrollTop).toString() + "px";
            }
        }

        if (this.countLines(this.Breadcrumb) === 1) {
            this.Breadcrumb.style.height = "55px";
        } else {
            this.Breadcrumb.style.height = "auto";
        }

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

   private reorientatedMenu(pos: number):void {
        if (pos > 80) {
            $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        } else {
            $('#BreadcrumbUserDropdown').css('margin-right', '');
        }
    }
}

$(() => {
    var SHC = new StickeyHeaderClass(); 
    SHC.StickyHeader();
});