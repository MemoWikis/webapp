$(window).scroll(function (event) {
    StickyHeader();
});

window.onresize = function (event) {
    StickyHeader();
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
       
    }

    ReorientateMenu();
}

function ReorientateMenu() {
 var position = $(this).scrollTop();

    if (position > 80) {

        $('#RightMainMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        $('#DropdownMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
    } else {
        $('#RightMainMenu').css('margin-right', '');
        $('#DropdownMenu').css('margin-right', '');
    }
}