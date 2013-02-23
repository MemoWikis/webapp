﻿var ImageUploadModalMode;
(function (ImageUploadModalMode) {
    ImageUploadModalMode._map = [];
    ImageUploadModalMode._map[0] = "Wikimedia";
    ImageUploadModalMode.Wikimedia = 0;
    ImageUploadModalMode._map[1] = "Upload";
    ImageUploadModalMode.Upload = 1;
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
            type: 'POST',
            true: false,
            cache: false,
            url: "/ImageUpload/FromWikimedia/",
            data: "url=" + url,
            error: function (error) {
                console.log(error);
                alert("Ein Fehler ist aufgetreten.");
            },
            success: function (responseJSON) {
                $("#divWikimediaSpinner").hide();
                if(responseJSON.ImageNotFound) {
                    $("#divWikimediaError").show();
                    self.SuccessfullyLoadedImageUrl = "";
                    self.SuccessfullyLoaded = false;
                    return;
                }
                self.SuccessfullyLoadedImageUrl = url;
                self.SuccessfullyLoaded = true;
                self.ImageThumbUrl = responseJSON.ImageThumbUrl;
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
        this._mode = ImageUploadModalMode.Wikimedia;
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
            request: {
                endpoint: $('#modalImageUpload').attr("data-endpoint")
            },
            multiple: false,
            debug: false,
            validation: {
                allowedExtensions: [
                    'jpeg', 
                    'jpg', 
                    'png'
                ],
                sizeLimit: 2097152
            }
        }).on('error', function (event, id, filename, reason) {
            alert("Ein Fehler ist aufgetreten");
        }).on('complete', function (event, id, filename, responseJSON) {
            $("#divUploadProgress").hide();
            $("#previewImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.filePath + '"> ');
            $("#previewImage").show();
            $("#divLegalInfo").show();
            $("#modalBody").stop().scrollTo('100%', 800);
            self.ImageThumbUrl = responseJSON.filePath;
        }).on('progress', function (event, id, filename, uploadedBytes, totalBytes) {
            $("#previewImage").hide();
            $("#divUploadProgress").show();
            $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    };
    ImageUploadModal.prototype.InitTypeRadios = function () {
        var self = this;
        $("#rdoImageWikimedia").change(function () {
            if($(this).is(':checked')) {
                $("#divUpload").hide();
                $("#divWikimedia").show();
                self._mode = ImageUploadModalMode.Wikimedia;
            }
        });
        $("#rdoImageUpload").change(function () {
            if($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                self._mode = ImageUploadModalMode.Upload;
            }
        });
    };
    ImageUploadModal.prototype.InitLicenceRadio = function () {
        $("#rdoLicenceByUloader").change(function () {
            if($(this).is(':checked')) {
                $("#divLicenceUploader").show();
                $("#divLicenceForeign").hide();
            }
        });
        $("#rdoLicenceForeign").change(function () {
            if($(this).is(':checked')) {
                $("#divLicenceUploader").hide();
                $("#divLicenceForeign").show();
            }
        });
    };
    ImageUploadModal.prototype.SaveImage = function () {
        if(this._mode == ImageUploadModalMode.Wikimedia) {
            this.SaveWikimediaImage();
        }
        if(this._mode == ImageUploadModalMode.Upload) {
            this.SaveUploadedImage();
        }
    };
    ImageUploadModal.prototype.SaveWikimediaImage = function () {
        if(!this.WikimediaPreview.SuccessfullyLoaded) {
            alert("Bitte lade ein Bild über eine Wikipedia URL.");
        } else {
            console.log(this.WikimediaPreview);
            this._onSave(this.WikimediaPreview.ImageThumbUrl);
        }
    };
    ImageUploadModal.prototype.SaveUploadedImage = function () {
        if(!$("#rdoLicenceForeign").is(':checked') && !$("#rdoLicenceByUloader").is(':checked')) {
            alert("Bitte wähle eine andere Lizenz");
        }
        if($("#rdoLicenceForeign").is(':checked')) {
            alert("Bitte wähle eine andere Lizenz. Wir bitten Dich das Bild auf Wikimedia hochzuladen und so einzubinden.");
        }
        if($("#rdoLicenceByUloader").is(':checked')) {
            var licenceOwner = $("#txtLicenceOwner").val();
            if(licenceOwner.trim() == "") {
                alert("Bitte gib Deinen Namen als Lizenzgeber an.");
            }
            this._onSave(this.ImageThumbUrl);
        }
    };
    ImageUploadModal.prototype.OnSave = function (func) {
        this._onSave = func;
    };
    return ImageUploadModal;
})();
