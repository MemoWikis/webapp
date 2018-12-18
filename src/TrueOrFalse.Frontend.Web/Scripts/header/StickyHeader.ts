$(window).scroll(event => {
     StickyHeader();
});

window.onresize = event => {
    StickyHeader();
}

window.onload = event => {
    var position = $(window).scrollTop();
    var rightMainMenu = document.getElementById("RightMainMenu");
    var breadcrumb = document.getElementById("Breadcrumb");
    var header = document.getElementById("MasterHeader");

    if (position > 80) {
        breadcrumb.style.top = "0px";
        rightMainMenu.style.top = "52px";
    } else {
        breadcrumb.style.top = (80 + header.scrollTop).toString() + "px";
        rightMainMenu.style.top = "60px";

        if (window.innerWidth < 768) {
            breadcrumb.style.top = "50px";
        }
    }

    if (countLines(document.getElementById("Breadcrumb")) === 1) {
        $('#Breadcrumb').css('height', '55px');
    } else {
        $('#Breadcrumb').css('height', 'auto');
    }
};

function StickyHeader() {
    var breadcrumb = document.getElementById("Breadcrumb");
    var rightMainMenu = document.getElementById("RightMainMenu");
    var position = $(window).scrollTop();
    var header = document.getElementById("MasterHeader");

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').css('display', 'flex');

        breadcrumb.style.top = "0px";
        breadcrumb.classList.add("ShowBreadcrumb");
        breadcrumb.classList.add("sticky");

        rightMainMenu.style.position = "fixed";
        rightMainMenu.style.top = "52px";

        $('#BreadCrumbTrail').css('max-width', "51%");

        if ($("#HeaderUserDropdown").hasClass("open")) {
              $("#HeaderUserDropdown").removeClass("open");
              $("#BreadcrumbUserDropdownImage").addClass("open");
        }

    } else {
        breadcrumb.style.top = (80 + header.scrollTop).toString() + "px";
        breadcrumb.style.position = "absolute";

        if (breadcrumb.classList.contains("ShowBreadcrumb")) breadcrumb.classList.remove("ShowBreadcrumb");


        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();

        rightMainMenu.style.position = "absolute";
        rightMainMenu.style.top = "60px";
        rightMainMenu.style.position = "absolute";

        $('#BreadCrumbTrail').css("max-width", "");

        if ($("#BreadcrumbUserDropdownImage").hasClass("open")) {
            $("#BreadcrumbUserDropdownImage").removeClass("open");
            $("#HeaderUserDropdown").addClass("open");
        }

        if (breadcrumb.classList.contains("sticky")) {
           breadcrumb.classList.remove("sticky");
        }


        if (top.location.pathname === "/") {
            breadcrumb.style.display = "none";
        }

        if (window.innerWidth < 768) {
            breadcrumb.style.top = (50 + header.scrollTop).toString() + "px"
        }
    }

    if (countLines(document.getElementById("Breadcrumb")) === 1) {
        breadcrumb.style.height = "55px";
    } else {
        breadcrumb.style.height = "auto";
    }

    reorientateMenu(position);
}

function countLines(target) {

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

function reorientateMenu(pos) {
    if (pos > 80) {
        $('#RightMainMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
    } else {
        $('#RightMainMenu').css('margin-right', '');
        $('#BreadcrumbUserDropdown').css('margin-right', '');
    }
}