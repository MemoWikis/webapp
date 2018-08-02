$(window).scroll(function (event) {
    var position = $(this).scrollTop();

    if (position > 80) {
        $('#BreadcrumbLogoSmall').show();
        $('#StickyHeaderContainer').css('display', 'flex');
        $('#Breadcrumb').css('top', '0px');
        $('#Breadcrumb').css('position', 'sticky');
        $('#Breadcrumb').addClass('ShowBreadcrumb');
        $('#RightMainMenu').css('position', 'fixed');
        $('#RightMainMenu').css('right', '100px');
        $('#mainMenuThemeNavigation').css('border-left', 'none');
        $('#mainMenuQuestionsSetsCategories').css('border-left', 'none');
        $('#mainMenuGamesUsersMessages').css('border-left', 'none');

    } else {
        $('#BreadcrumbLogoSmall').hide();
        $('#StickyHeaderContainer').hide();
        $('#Breadcrumb').css('top', '80px');
        $('#Breadcrumb').css('position', 'unset');
        $('#Breadcrumb').removeClass('ShowBreadcrumb');
        $('#RightMainMenu').css('position', 'absolute');
        $('#RightMainMenu').css('right', '');
        $('#mainMenuThemeNavigation').css('border-left', 'solid #707070 1px');
        $('#mainMenuQuestionsSetsCategories').css('border-left', 'solid #707070 1px');
        $('#mainMenuGamesUsersMessages').css('border-left', 'solid #707070 1px');

    }
});