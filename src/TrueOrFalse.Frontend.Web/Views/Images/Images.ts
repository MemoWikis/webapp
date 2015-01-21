var fnInitImages = function () {
    $('.LicensedImage').each(function () {
        $(this).attr('src', $(this).attr('data-image-url')).removeAttr('data-image-url');
    });

    fnInitImagesItemRow();
    fnInitImageDetailModal($('.JS-ImageDetailModal'));
}

var fnInitImagesItemRow = function() {
    $('.ItemImage').each(function() {
        $("<div class='SelectAreaCheckbox'>" +
            "<div class='CheckboxIconContainer'>" +
                "<i class='Checked-Icon fa fa-check-square-o'></i>" +
                "<i class='Unchecked-Icon fa fa-square-o'></i>" +
                "<div class='CheckboxText'>Auswählen</div>" +
            "</div>" +
        "</div>").insertAfter($(this));
    });
    $('.LicensedImage.ItemImage').each(function() {
        $(  "<div class='SelectAreaImageInfo'>" +
                "<div data-image-id ='" + $(this).attr('data-image-id') + "' class='HoverMessage JS-ImageDetailModal'>Bild- und Lizenzinfos</div>" +
            "</div>").insertAfter($(this).parent().find('.SelectAreaCheckbox'));
    });
    $('.LicensedImage.ItemImage').each(function () {
        var ancestorToInsertAfter = $(this).attr('data-append-image-link-to');
        $(this).removeAttr('data-append-image-link-to');
        $("<a data-image-id ='" + $(this).attr('data-image-id') + "' class='ImageLicenseCaption JS-ImageDetailModal' href='#'>Bild- und Lizenzinfos</a>")
            .insertAfter($(this).closest("." + ancestorToInsertAfter));
    });
}