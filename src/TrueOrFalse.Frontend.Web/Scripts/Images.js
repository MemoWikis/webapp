var fnInitImages = function () {
    $('.LicensedImage').each(function () {
        $(this).attr('src', $(this).attr('data-image-url')).removeAttr('data-image-url');
    });

    fnInitImagesItemRow();
};

var fnInitImagesItemRow = function () {
    $('.ItemRowImage').each(function () {
        $("<div class='SelectAreaUpper'>" + "<div class='CheckboxIconContainer'>" + "<i class='Checked-Icon fa fa-check-square-o'></i>" + "<i class='Unchecked-Icon fa fa-square-o'></i>" + "<div class='CheckboxText'>Auswählen</div>" + "</div>" + "</div>").insertAfter($(this));
    });
    $('.LicensedImage.ItemRowImage').each(function () {
        $("<div class='SelectAreaLower JS-ImageDetailModal'>" + "<div data-image-id ='" + $(this).attr('data-image-id') + "' class='HoverMessage JS-ImageDetailModal'>Bild- und Lizenzinfos</div>" + "</div>").insertAfter($(this).parent().find('.SelectAreaUpper'));
    });
    $('.LicensedImage.ItemRowImage').each(function () {
        $("<a data-image-id ='" + $(this).attr('data-image-id') + "' class='ImageLicenseCaption JS-ImageDetailModal' href='#'>Bild- und Lizenzinfos</a>").appendTo($(this).closest(".column-Image"));
    });
};
//# sourceMappingURL=Images.js.map
