$(window).scroll(function (event) {
    var position = $(this).scrollTop();

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').css('display', 'flex');
        $('#Breadcrumb').css('top', '0px');
        $('#Breadcrumb').css('position', 'sticky');
        $('#Breadcrumb').addClass('ShowBreadcrumb');
        $('#RightMainMenu').css('position', 'fixed');
        $('#RightMainMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));
        $('#DropdownMenu').css('margin-right', $('#BreadCrumbContainer').css('margin-right'));

    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
        $('#Breadcrumb').removeClass('ShowBreadcrumb');
        $('#RightMainMenu').css('position', 'absolute');
        $('#RightMainMenu').css('margin-right', '');

    }
});