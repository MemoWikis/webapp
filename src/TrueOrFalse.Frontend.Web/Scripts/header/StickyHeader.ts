$(window).scroll(function (event) {
    StickyHeader();
});

window.onresize = function (event) {
    StickyHeader();
}

window.onload = function(event) {
    if (window.innerWidth <= 767 && top.location.pathname === '/') {
            $('#Breadcrumb').show();
    }
}

function StickyHeader() {
    var position = $(this).scrollTop();
    var header = document.getElementById("Breadcrumb");
    var positionSticky = window.getComputedStyle(header).getPropertyValue("position");

    if (position > 80 && window.innerWidth >= 720) {

            $('#BreadcrumbLogoSmall').show();
            $('#StickyHeaderContainer').css('display', 'flex');
            $('#Breadcrumb').css('top', '0px');
            $('#Breadcrumb').css('position', 'sticky');
            $('#RightMainMenu').css('position', 'fixed');
            $('#RightMainMenu').css('top', '52px');
            $('#Breadcrumb').addClass('ShowBreadcrumb');
            $('#BreadCrumbTrail').css('max-width', '51%');

            if ($("#HeaderUserDropdown").hasClass("open")) {
                $("#HeaderUserDropdown").removeClass("open");
                $("#BreadcrumbUserDropdownImage").addClass("open");
            }

        if (positionSticky != "sticky") {
                header.classList.add("sticky");
        }

    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
        $('#Breadcrumb').removeClass('ShowBreadcrumb');
        $('#RightMainMenu').css('position', 'absolute');
        $('#RightMainMenu').css('top', '60px');
        $('#BreadCrumbTrail').css('max-width', '');

        if ($("#BreadcrumbUserDropdownImage").hasClass("open")) {
            $("#BreadcrumbUserDropdownImage").removeClass("open");
            $("#HeaderUserDropdown").addClass("open");
        }

        if (window.innerWidth <= 767 && top.location.pathname === '/') {
            $('#Breadcrumb').show();
        }
    }
   
    if (countLines(document.getElementById("Breadcrumb")) === 1) {
            $('#Breadcrumb').css('height', '55px')
    } else
    {
        $('#Breadcrumb').css('height', 'auto');
    }
    ReorientateMenu();
}

function countLines(target) {
    $('#Breadcrumb').css('height', 'auto');
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

function ReorientateMenu() {
 var position = $(this).scrollTop();

    if (position > 80) {

        $('#RightMainMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
    } else {
        $('#RightMainMenu').css('margin-right', '');
        $('#BreadcrumbUserDropdown').css('margin-right', '');
    }
}