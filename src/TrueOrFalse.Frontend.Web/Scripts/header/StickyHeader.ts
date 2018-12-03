var timeout;

$(window).scroll(event => {
        StickyHeader();
});

window.onresize = event => {
    StickyHeader();
}

window.onload = event => {
    var position = $(document).scrollTop();
    if (window.innerWidth <= 767 && top.location.pathname === '/') {
        if (position < 80) {
            $('#Breadcrumb').hide();
        } else {
            $('#Breadcrumb').show();
        }
    }

    if ($('#Breadcrumb').offsetParent() === null) {
        $('#MasterMainContent').css('margin-top', '0px');
    } else {
        $('#MasterMainContent').css('margin-top', '55px');
    }

    if (window.innerWidth < 720) {
        $('#Breadcrumb').css('position', 'unset');
        $('#MasterMainContent').css('margin-top', '0px');
    }
}

function StickyHeader() {
    var header = document.getElementById("Breadcrumb");
    var positionSticky = window.getComputedStyle(header).getPropertyValue("position");

    var position = $(document).scrollTop();
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

        if (positionSticky !== "sticky") {
                header.classList.add("sticky");
        }

        if (document.getElementById("MasterMainContent").style.marginTop !== "0px") {
            $('#MasterMainContent').css('margin-top', '0px');
        }

        if (top.location.pathname === '/' && $('#Breadcrumb').offsetParent() === null) {
            $('#Breadcrumb').show();
        }

    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'absolute');
        $('#Breadcrumb').removeClass('ShowBreadcrumb');
        $('#RightMainMenu').css('position', 'absolute');
        $('#RightMainMenu').css('top', '60px');
        $('#BreadCrumbTrail').css('max-width', '');

        if ($("#BreadcrumbUserDropdownImage").hasClass("open")) {
            $("#BreadcrumbUserDropdownImage").removeClass("open");
            $("#HeaderUserDropdown").addClass("open");
        }

        if ($('#Breadcrumb').offsetParent() === null) {
            $('#MasterMainContent').css('margin-top', '0px');
        } else {
             $('#MasterMainContent').css('margin-top', '55px');
        }

        if (top.location.pathname === '/') {
            $('#Breadcrumb').hide();
        }
    }

    if (top.location.pathname === '/') {
        if ($('#Breadcrumb').offsetParent() === null) {
            $('#MasterMainContent').css('margin-top', '0px');
        } else {
            $('#MasterMainContent').css('margin-top', '55px');
        }
    }

    if (window.innerWidth < 768 && position < 80) {
        $('#Breadcrumb').css('top', '50px');
    }

    if (countLines(document.getElementById("Breadcrumb")) === 1) {
        $('#Breadcrumb').css('height', '55px');
    } else
    {
        $('#Breadcrumb').css('height', 'auto');
    }

    reorientateMenu(position);
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

function reorientateMenu(pos) {

    if (pos > 80) {
        $('#RightMainMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        $('#BreadcrumbUserDropdown').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
    } else {
        $('#RightMainMenu').css('margin-right', '');
        $('#BreadcrumbUserDropdown').css('margin-right', '');
    }
}