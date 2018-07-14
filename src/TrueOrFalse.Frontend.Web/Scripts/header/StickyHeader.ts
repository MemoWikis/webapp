$(window).scroll(function (event) {
    var position = $(this).scrollTop();

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').css('display', 'flex');
        $('#Breadcrumb').css('top', '0px');
        $('#Breadcrumb').css('position', 'sticky');
        $('#Breadcrumb').css('display', 'block');
        $('#RightMainMenu').css('position', 'fixed');
        $('#RightMainMenu').css('top', '75px');
        $('#RightMainMenu').addClass('StickyMainMenu');

    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
        $('#RightMainMenu').css('position', 'absolute');
        $('#RightMainMenu').css('top', '18px');
        $('#RightMainMenu').removeClass('StickyMainMenu');

    }
});