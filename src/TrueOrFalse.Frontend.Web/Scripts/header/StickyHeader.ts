$(window).scroll(function (event) {
    var position = $(this).scrollTop();

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').css('display', 'flex');
        $('#Breadcrumb').css('top', '0px');
        $('#Breadcrumb').css('position', 'sticky');
        $('#Breadcrumb').css('display', 'block');
        $('#LongMenu').css('position', 'fixed');
        $('#LongMenu').css('right', '10%');
  
    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
        $('#LongMenu').css('position', 'absolute');
        $('#LongMenu').css('right', '');
    }
});