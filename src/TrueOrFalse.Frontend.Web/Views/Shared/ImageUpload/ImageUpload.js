/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.scrollTo.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/fineUploader.d.ts" />
var ImageUploadModalMode;
(function (ImageUploadModalMode) {
    ImageUploadModalMode[ImageUploadModalMode["Wikimedia"] = 0] = "Wikimedia";
    ImageUploadModalMode[ImageUploadModalMode["Upload"] = 1] = "Upload";
})(ImageUploadModalMode || (ImageUploadModalMode = {}));

var WikimediaPreview = (function () {
    function WikimediaPreview() {
        this.SuccessfullyLoaded = false;
    }
    WikimediaPreview.prototype.Load = function () {
        $("#divWikimediaSpinner").show();
        $("#divWikimediaError").hide();
        $("#previewWikimediaImage").hide();

        var url = $("#txtWikimediaUrl").val();
        var self = this;

        $.ajax({
            type: 'POST', true: false, cache: false,
            url: "/ImageUpload/FromWikimedia/",
            data: "url=" + url,
            error: function (error) {
                console.log(error);
                alert("Ein Fehler ist aufgetreten.");
            },
            success: function (responseJSON) {
                $("#divWikimediaSpinner").hide();

                if (responseJSON.ImageNotFound) {
                    $("#divWikimediaError").show();
                    self.SuccessfullyLoadedImageUrl = "";
                    self.SuccessfullyLoaded = false;
                    return;
                }

                self.SuccessfullyLoadedImageUrl = url;
                self.SuccessfullyLoaded = true;
                self.ImageThumbUrl = responseJSON.ImageThumbUrl;
                self.ImageName = url;

                $("#previewWikimediaImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.ImageThumbUrl + '"> ');
                $("#previewWikimediaImage").show();

                $("#modalBody").stop().scrollTo('100%', 800);
            }
        });
    };
    return WikimediaPreview;
})();

var ImageUploadModal = (function () {
    function ImageUploadModal() {
        this.WikimediaPreview = new WikimediaPreview();
        this.Mode = 0 /* Wikimedia */;
        this.InitUploader();
        this.InitTypeRadios();
        this.InitLicenceRadio();

        var self = this;
        $("#txtWikimediaUrl").change(function () {
            self.WikimediaPreview.Load();
        });
        $("#aSaveImage").click(function () {
            self.SaveImage();
        });
    }
    ImageUploadModal.prototype.InitUploader = function () {
        var self = this;
        $('#fileUpload').fineUploader({
            uploaderType: 'basic',
            button: $('#fileUpload')[0],
            request: { endpoint: '/ImageUpload/File' },
            multiple: false,
            debug: false,
            validation: {
                allowedExtensions: ['jpeg', 'jpg', 'png'],
                sizeLimit: 2097152
            }
        }).on('error', function (event, id, filename, reason) {
            console.log(event + " " + id + " " + filename + " " + reason);
            alert("Ein Fehler ist aufgetreten");
        }).on('complete', function (event, id, filename, responseJSON) {
            $("#divUploadProgress").hide();
            $("#previewImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.FilePath + '"> ');
            $("#previewImage").show();
            $("#divLegalInfo").show();
            $("#modalBody").stop().scrollTo('100%', 800);

            self.ImageThumbUrl = responseJSON.FilePath;
            self.ImageGuid = responseJSON.Guid;
            self.LicenceOwner = $("#txtLicenceOwner").val();
        }).on('progress', function (event, id, filename, uploadedBytes, totalBytes) {
            $("#previewImage").hide();
            $("#divUploadProgress").show();
            $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    };

    ImageUploadModal.prototype.InitTypeRadios = function () {
        var self = this;

        $("#rdoImageWikimedia").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").hide();
                $("#divWikimedia").show();
                self.Mode = 0 /* Wikimedia */;
            }
        });

        $("#rdoImageUpload").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                self.Mode = 1 /* Upload */;
            }
        });
    };

    ImageUploadModal.prototype.InitLicenceRadio = function () {
        $("#rdoLicenceByUloader").change(function () {
            if ($(this).is(':checked')) {
                $("#divLicenceUploader").show();
                $("#divLicenceForeign").hide();
            }
        });

        $("#rdoLicenceForeign").change(function () {
            if ($(this).is(':checked')) {
                $("#divLicenceUploader").hide();
                $("#divLicenceForeign").show();
            }
        });
    };

    ImageUploadModal.prototype.SaveImage = function () {
        if (this.Mode == 0 /* Wikimedia */) {
            SaveWikipediaImage.Run(this.WikimediaPreview, this._onSave);
        }

        if (this.Mode == 1 /* Upload */) {
            SaveUploadedImage.Run(this.ImageThumbUrl, this._onSave);
        }
    };

    ImageUploadModal.prototype.OnSave = function (func) {
        this._onSave = func;
    };

    ImageUploadModal.prototype.SetTitle = function (title) {
        $("#modalImageUpload .modal-title").html(title);
    };
    return ImageUploadModal;
})();

var SaveWikipediaImage = (function () {
    function SaveWikipediaImage() {
    }
    SaveWikipediaImage.Run = function (wikiMediaPreview, fnOnSave) {
        if (!wikiMediaPreview.SuccessfullyLoaded) {
            alert("Bitte lade ein Bild ueber eine Wikipedia URL.");
        } else {
            fnOnSave(wikiMediaPreview.ImageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    };
    return SaveWikipediaImage;
})();

var SaveUploadedImage = (function () {
    function SaveUploadedImage() {
    }
    SaveUploadedImage.Run = function (imageThumbUrl, fnOnSave) {
        if (!$("#rdoLicenceForeign").is(':checked') && !$("#rdoLicenceByUloader").is(':checked')) {
            alert("Bitte waehle eine andere Lizenz");
            return;
        }

        if ($("#rdoLicenceForeign").is(':checked')) {
            alert("Bitte waehle eine andere Lizenz. Wir bitten Dich das Bild auf Wikimedia hochzuladen und so einzubinden.");
            return;
        }

        if ($("#rdoLicenceByUloader").is(':checked')) {
            var licenceOwner = $("#txtLicenceOwner").val();
            if (licenceOwner.trim() == "") {
                alert("Bitte gib Deinen Namen als Lizenzgeber an.");
                return;
            }

            fnOnSave(imageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    };
    return SaveUploadedImage;
})();
//# sourceMappingURL=ImageUpload.js.map
