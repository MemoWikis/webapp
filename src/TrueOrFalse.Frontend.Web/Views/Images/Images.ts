declare function encodeURIComponent(text: string): any;

var fnInitImages = function () {
    fnReplaceDummyImages();
    fnInitItemImages();
    fnInitImageDetailModal();
    fnInitMarkdownImages();
}

var fnReplaceDummyImages = function() {
    $('.LicensedImage').each(function () {
        if (!$(this).hasClass('JS-CantBeDisplayed')) {
            $(this).attr('src', $(this).attr('data-image-url')).removeAttr('data-image-url');
        }
    });
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
    $('.RenderedMarkdown img').each(function () {
        var self = $(this);

        $.ajax({
            type: "GET",
            url: "/Images/GetQuestionImageId?encodedPath=" + encodeURIComponent($(this).attr('src')),
            success: function (data) {
                self.attr('data-image-id', data).addClass('ItemImage LicensedImage JS-InitImage').attr('data-append-image-link-to', 'ImageContainer');
                self.wrap('<div class="MarkdownImage"><div class="ImageContainer"></div></div>');
                fnSetMaxWidth(self); //restrict width of container to original image width
                fnInitItemImages();
                fnInitImageDetailModal();
            } 
        });
    });
}

var fnSetMaxWidth = function (image : any, count : number = 0) {

    var imgOriginalWidth;

    setTimeout(function () {
        imgOriginalWidth = image[0].naturalWidth;
        if (imgOriginalWidth > 0) {
            image.closest('.MarkdownImage').css('max-width', imgOriginalWidth);
        } else if (count < 10) {
            count++;
            fnSetMaxWidth(image, count);
        }
    }, 10);
}
