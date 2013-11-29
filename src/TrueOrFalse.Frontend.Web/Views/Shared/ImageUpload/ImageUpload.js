var ImageUploadModalMode;
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
        this.Mode = ImageUploadModalMode.Wikimedia;
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
                endpoint: '/ImageUpload/File'
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
            if($(this).is(':checked')) {
                $("#divUpload").hide();
                $("#divWikimedia").show();
                self.Mode = ImageUploadModalMode.Wikimedia;
            }
        });
        $("#rdoImageUpload").change(function () {
            if($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                self.Mode = ImageUploadModalMode.Upload;
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
        if(this.Mode == ImageUploadModalMode.Wikimedia) {
            SaveWikipediaImage.Run(this.WikimediaPreview, this._onSave);
        }
        if(this.Mode == ImageUploadModalMode.Upload) {
            SaveUploadedImage.Run(this.ImageThumbUrl, this._onSave);
        }
    };
    ImageUploadModal.prototype.OnSave = function (func) {
        this._onSave = func;
    };
    return ImageUploadModal;
})();
var SaveWikipediaImage = (function () {
    function SaveWikipediaImage() { }
    SaveWikipediaImage.Run = function Run(wikiMediaPreview, fnOnSave) {
        if(!wikiMediaPreview.SuccessfullyLoaded) {
            alert("Bitte lade ein Bild über eine Wikipedia URL.");
        } else {
            fnOnSave(wikiMediaPreview.ImageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    }
    return SaveWikipediaImage;
})();
var SaveUploadedImage = (function () {
    function SaveUploadedImage() { }
    SaveUploadedImage.Run = function Run(imageThumbUrl, fnOnSave) {
        if(!$("#rdoLicenceForeign").is(':checked') && !$("#rdoLicenceByUloader").is(':checked')) {
            alert("Bitte wähle eine andere Lizenz");
            return;
        }
        if($("#rdoLicenceForeign").is(':checked')) {
            alert("Bitte wähle eine andere Lizenz. Wir bitten Dich das Bild auf Wikimedia hochzuladen und so einzubinden.");
            return;
        }
        if($("#rdoLicenceByUloader").is(':checked')) {
            var licenceOwner = $("#txtLicenceOwner").val();
            if(licenceOwner.trim() == "") {
                alert("Bitte gib Deinen Namen als Lizenzgeber an.");
                return;
            }
            fnOnSave(imageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    }
    return SaveUploadedImage;
})();
