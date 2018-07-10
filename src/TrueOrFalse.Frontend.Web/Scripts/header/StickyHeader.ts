$(window).scroll(function (event) {
    var position = $(this).scrollTop();

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').css('display', 'flex');
        $('#Breadcrumb').css('top', '0px');
        $('#Breadcrumb').css('position', 'sticky');
        $('#Breadcrumb').css('display', 'block');
        $('#MainMenu').css('position', 'fixed');
        $('#MainMenu').css('top', '75px');
        $('#MainMenu').addClass('StickyMainMenu');

    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
        $('#MainMenu').css('position', 'absolute');
        $('#MainMenu').css('top', '18px');
        $('#MainMenu').removeClass('StickyMainMenu');

    }
});