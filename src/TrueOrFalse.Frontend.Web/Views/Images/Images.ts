declare function encodeURIComponent(text : string) : any;

var fnInitImages = function () {
    $('.LicensedImage').each(function () {
        $(this).attr('src', $(this).attr('data-image-url')).removeAttr('data-image-url');
    });

    fnInitItemImages();
    fnInitImageDetailModal();
    fnInitMarkdownImages();
}

var fnInitItemImages = function () {
    $('.JS-InitImage').each(function() {
        $("<div class='SelectAreaCheckbox'>" +
            "<div class='CheckboxIconContainer'>" +
                "<i class='Checked-Icon fa fa-check-square-o'></i>" +
                "<i class='Unchecked-Icon fa fa-square-o'></i>" +
                "<div class='CheckboxText'>Auswählen</div>" +
            "</div>" +
        "</div>").insertAfter($(this));
    });
    $('.LicensedImage.JS-InitImage').each(function() {
        $(  "<div class='SelectAreaImageInfo'>" +
                "<div data-image-id ='" + $(this).attr('data-image-id') + "' class='HoverMessage JS-InitImageDetailModal'>Bild- und Lizenzinfos</div>" +
            "</div>").insertAfter($(this).parent().find('.SelectAreaCheckbox'));
    });
    $('.LicensedImage.JS-InitImage').each(function () {
        var ancestorToInsertAfter = $(this).attr('data-append-image-link-to');
        $(this).removeAttr('data-append-image-link-to');
        $("<a data-image-id ='" + $(this).attr('data-image-id') + "' class='ImageLicenseCaption JS-InitImageDetailModal' href='#'>Bild- und Lizenzinfos</a>")
            .insertAfter($(this).closest("." + ancestorToInsertAfter));
        $(this).removeClass('JS-InitImage');
    });
}

var fnInitMarkdownImages = function () {
    $('.Markdown img').each(function () {
        var self = $(this);
        $.ajax({
            type: "GET",
            url: "/Images/GetQuestionImageId?encodedPath=" + encodeURIComponent($(this).attr('src')),
            success: function (data) {
                self.attr('data-image-id', data).addClass('ItemImage LicensedImage JS-InitImage').attr('data-append-image-link-to', 'ImageContainer');
                self.wrap('<div class="ImageContainer"></div>');
                fnInitItemImages();
                fnInitImageDetailModal();
            } 
        });
        
    });
}
