declare function encodeURIComponent(text: string): any;

class Images {
    static Init() {
        Images.ReplaceDummyImages();
        Images.InitItemImages();
        ImageDetailModal.Init();
        Images.InitMarkdownImages();        
    }

    public static ReplaceDummyImages() {
        $('.LicensedImage').each(function () {
            if (!$(this).hasClass('JS-CantBeDisplayed')) {
                $(this).attr('src', $(this).attr('data-image-url')).removeAttr('data-image-url');
            }
        });        
    }

    private static InitItemImages() {
        Images.AddHoverCheckboxArea();
        //Images.AddHoverLicenseArea();
        Images.AddLicenseInfoContainer();
        //Images.AddLicenseCaption();
    }

    private static AddHoverCheckboxArea() {
        $('.JS-InitImage').each(function () {
            $(this).wrap("<div style='position: relative;'></div>");
            $("<div class='SelectAreaCheckbox'>" +
                "<div class='CheckboxIconContainer'>" +
                "<i class='Checked-Icon fa fa-check-square-o'></i>" +
                "<i class='Unchecked-Icon fa fa-square-o'></i>" +
                "<div class='CheckboxText'>Auswählen</div>" +
                "</div>" +
                "</div>").insertAfter($(this));
        });
    }

    private static GetYoutubeAttribute(elem : JQuery) {

        if (!elem.is("[data-is-youtube-video]"))
            return "";

        return "data-is-youtube-video='" + elem.attr('data-is-youtube-video') + "'";
    }

    private static AddHoverLicenseArea() {
        $('.LicensedImage.JS-InitImage').each(function () {
            $("<div class='SelectAreaImageInfo'>" +
                "<div data-image-id ='" + $(this).attr('data-image-id') + "' class='HoverMessage JS-InitImageDetailModal' " + Images.GetYoutubeAttribute($(this)) + " >Bild- und Lizenzinfos</div>" +
                "</div>").insertAfter($(this).parent().find('.SelectAreaCheckbox'));
        });
    }

    private static AddLicenseInfoContainer() {

        $('.LicensedImage.JS-InitImage').each(function () {

            if ($(this).data("LicenseContainerAdded")) {
                return;
            }

            $("<div data-image-id ='" + $(this).attr('data-image-id') + "' class='LicenseInfo JS-InitImageDetailModal' " + Images.GetYoutubeAttribute($(this)) + "></div>")
                .appendTo($(this)
                .closest('.ImageContainer'));

            $(this).data("LicenseContainerAdded", true);
        });
    }

    private static AddLicenseCaption() {
        $('.JS-InitImage').each(function () {
            var ancestorToInsertAfter = $(this).attr('data-append-image-link-to');
            $(this).removeAttr('data-append-image-link-to');

            var htmlToInsert = $(this).hasClass('LicensedImage')
                ? $("<div class='ImageLicenseCaption'><a data-image-id ='" +
                    $(this).attr('data-image-id') +
                    "' class='JS-InitImageDetailModal' href='#'>Bild- und Lizenzinfos</a></div>")
                : $("<div class='ImageLicenseCaption'></div>");

            htmlToInsert.insertAfter($(this).closest("." + ancestorToInsertAfter));
           
            $(this).removeClass('JS-InitImage');
        });      
    }

    private static InitMarkdownImages() {
        $('.RenderedMarkdown img').each(function () {
            var self = $(this);

            $.ajax({
                type: "GET",
                url: "/Images/GetQuestionImageId?encodedPath=" + encodeURIComponent($(this).attr('src')),
                success: function (data) {
                    self.attr('data-image-id', data).addClass('ItemImage LicensedImage JS-InitImage').attr('data-append-image-link-to', 'ImageContainer');
                    self.wrap('<div class="MarkdownImage"><div class="ImageContainer"></div></div>');
                    Images.SetMaxWidth(self); //restrict width of container to original image width
                    Images.InitItemImages();
                    ImageDetailModal.Init();
                }
            });
        });        
    }

    private static SetMaxWidth(image: any, count: number = 0) {
        var imgOriginalWidth;

        window.setTimeout(() => {
            imgOriginalWidth = image[0].naturalWidth;
            if (imgOriginalWidth > 0) {
                image.closest('.MarkdownImage').css('max-width', imgOriginalWidth);
            } else if (count < 10) {
                count++;
                Images.SetMaxWidth(image, count);
            }
        }, 10);        
    }
}