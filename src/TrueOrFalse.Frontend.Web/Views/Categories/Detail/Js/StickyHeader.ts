$(window).scroll(function (event) {
    var position = $(this).scrollTop();

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').show();
        $('#Breadcrumb').css('top', '0px');
        $('#Breadcrumb').css('position', 'sticky');
    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
    }
});