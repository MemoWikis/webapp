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
    WikimediaPreview.prototype.Load = function (onSuccess, onError) {
        $("#divWikimediaError").hide();
        $("#previewWikimediaImage").hide();

        var url = $("#txtWikimediaUrl").val();
        var self = this;

        $.ajax({
            type: 'POST', true: false, cache: false,
            url: "/ImageUpload/FromWikimedia/",
            data: "url=" + url,
            error: function (error) {
                window.console.log(error);
                window.alert("Ein Fehler ist aufgetreten.");
            },
            success: function (responseJSON) {
                onSuccess();

                if (responseJSON.ImageNotFound) {
                    $("#divWikimediaError").show();

                    //self.SuccessfullyLoadedImageUrl = "";
                    //self.SuccessfullyLoaded = false;
                    onError();
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
        this.InitLicenseRadio();

        this.SaveButtonWiki = $("#ButtonsWikimedia .aSaveImage");
        this.SaveButtonWikiSpinner = this.SaveButtonWiki.find("i");
        this.SaveButtonUpload = $("#ButtonsUserUpload .aSaveImage");
        this.SaveButtonUploadSpinner = this.SaveButtonUpload.find("i");

        this.SaveButtonWikiSpinner.hide();

        var self = this;

        this._onPreviewLoadSuccess = function () {
            self.SaveButtonWikiSpinner.hide();
            self.SaveButtonWiki.removeClass('disabled').find($('span')).html('Bild speichern');
            self.PreviewLoadOngoing = false;
        };

        this._onPreviewLoadError = function () {
            self.ResetModal();
        };

        $('.modalImageUploadDismiss').click(function () {
            self.ResetModal(true);
        });

        $('#txtWikimediaUrl').on('input', function () {
            if ($('#txtWikimediaUrl').val() !== "") {
                self.SaveButtonWiki.removeClass('disabled');
            } else {
                self.SaveButtonWiki.addClass('disabled');
            }
        });
        $("#txtWikimediaUrl").change(function () {
            self.StartPreviewLoad();
        });
        this.SaveButtonWiki.click(function (e) {
            if (!$(e.target).hasClass('disabled')) {
                if (self.WikimediaPreview.SuccessfullyLoaded) {
                    self.SaveImage();
                } else if (!self.PreviewLoadOngoing) {
                    self.StartPreviewLoad();
                }
            }
        });
        this.SaveButtonUpload.click(function (e) {
            if (!$(e.target).hasClass('disabled')) {
                self.SaveImage();
            }
        });

        $(window).on('enableSaveButtonUserUpload', function () {
            self.EnableSaveButtonUpload();
        });

        $("#txtLicenseOwner").change(function () {
            self.LicenseOwner = $("#txtLicenseOwner").val();
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
                allowedExtensions: ['jpeg', 'jpg', 'png', 'svg'],
                sizeLimit: 10485760
            }
        }).on('error', function (event, id, filename, reason) {
            window.console.log(event + " " + id + " " + filename + " " + reason);
            window.alert("Ein Fehler ist aufgetreten");
        }).on('complete', function (event, id, filename, responseJSON) {
            self.SaveButtonUpload.removeClass('disabled');
            $("#divUserUploadProgress").hide();
            $("#previewImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.FilePath + '"> ');
            $("#previewImage").show();
            $("#divLegalInfo").show();
            $("#modalBody").stop().scrollTo('100%', 800);

            self.ImageThumbUrl = responseJSON.FilePath;
            self.ImageGuid = responseJSON.Guid;
            self.LicenseOwner = $("#txtLicenseOwner").val();
        }).on('progress', function (event, id, filename, uploadedBytes, totalBytes) {
            $("#previewImage").hide();
            $("#divUserUploadProgress").show();
            $("#divUserUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    };

    ImageUploadModal.prototype.InitTypeRadios = function () {
        var self = this;

        $("#rdoImageWikimedia").change(function () {
            if ($(this).is(':checked')) {
                $("#divUserUpload, #ButtonsUserUpload").hide();
                $("#divWikimedia, #ButtonsWikimedia").show();
                self.Mode = 0 /* Wikimedia */;
            }
        });

        $("#rdoImageUpload").change(function () {
            if ($(this).is(':checked')) {
                $("#divUserUpload, #ButtonsUserUpload").show();
                $("#divWikimedia, #ButtonsWikimedia").hide();
                self.Mode = 1 /* Upload */;
            }
        });
    };

    ImageUploadModal.prototype.InitLicenseRadio = function () {
        $("#rdoLicenseByUploader").change(function () {
            if ($(this).is(':checked')) {
                $("#divLicenseUploader").show();
                $("#divLicenseForeign").hide();
            }
        });

        $("#rdoLicenseForeign").change(function () {
            if ($(this).is(':checked')) {
                $("#divLicenseUploader").hide();
                $("#divLicenseForeign").show();
            }
        });
    };

    ImageUploadModal.prototype.StartPreviewLoad = function () {
        this.SaveButtonWiki.addClass("disabled");
        this.SaveButtonWikiSpinner.show();
        if (!this.PreviewLoadOngoing) {
            this.WikimediaPreview = new WikimediaPreview();
            this.WikimediaPreview.Load(this._onPreviewLoadSuccess, this._onPreviewLoadError);
            this.PreviewLoadOngoing = true;
            $('#txtWikimediaUrl').attr('disabled', 'disabled');
        }
    };

    ImageUploadModal.prototype.SaveImage = function () {
        var self = this;

        if (self.Mode === 0 /* Wikimedia */) {
            this.SaveButtonWikiSpinner.show();
            this.SaveButtonWiki.addClass("disabled");

            window.setTimeout(function () {
                SaveWikipediaImage.Run(self.WikimediaPreview, self._onSave);
            }, 10);
        }

        if (self.Mode === 1 /* Upload */) {
            this.SaveButtonUploadSpinner.show();
            this.SaveButtonUpload.addClass("disabled");

            window.setTimeout(function () {
                SaveUploadedImage.Run(self.ImageThumbUrl, self._onSave);
            }, 10);
        }
    };

    ImageUploadModal.prototype.ResetModal = function (resetInput) {
        if (typeof resetInput === "undefined") { resetInput = false; }
        this.WikimediaPreview = new WikimediaPreview();
        this.SaveButtonWikiSpinner.hide();
        $('#previewWikimediaImage').html('');
        if (resetInput) {
            $('#txtWikimediaUrl').val('');
        }
        $('#txtWikimediaUrl').removeAttr('disabled').trigger('input');

        this.SaveButtonWiki.find($('span')).html('Vorschau laden');
        this.PreviewLoadOngoing = false;

        this.SaveButtonUploadSpinner.hide();
        $("#previewImage").empty().hide();
        $("#divLegalInfo").hide();

        //$("#txtLicenseOwner").val('');
        $('input:radio[name="imgLicenseType"]:checked').prop('checked', false);
        $('#divLicenseUploader').hide();
    };

    ImageUploadModal.prototype.OnSave = function (func) {
        this._onSave = func;
    };

    ImageUploadModal.prototype.SetTitle = function (title) {
        $("#modalImageUpload .modal-title").html(title);
    };

    ImageUploadModal.prototype.EnableSaveButtonUpload = function () {
        this.SaveButtonUpload.removeClass('disabled');
        this.SaveButtonUploadSpinner.hide();
    };
    return ImageUploadModal;
})();

var SaveWikipediaImage = (function () {
    function SaveWikipediaImage() {
    }
    SaveWikipediaImage.Run = function (wikiMediaPreview, fnOnSave) {
        if (!wikiMediaPreview.SuccessfullyLoaded) {
            window.alert("Bitte lade ein Bild über eine Wikimedia-URL.");
        } else {
            fnOnSave(wikiMediaPreview.ImageThumbUrl);
        }
    };
    return SaveWikipediaImage;
})();

var SaveUploadedImage = (function () {
    function SaveUploadedImage() {
    }
    SaveUploadedImage.Run = function (imageThumbUrl, fnOnSave) {
        if ($('#divLegalInfo:hidden').length !== 0) {
            window.alert("Hups, bitte wähle zuerst ein Bild aus.");
            $(window).trigger('enableSaveButtonUserUpload');
            return;
        }

        if (!$("#rdoLicenseForeign").is(':checked') && !$("#rdoLicenseByUploader").is(':checked')) {
            window.alert("Bitte wähle eine andere Lizenz.");
            $(window).trigger('enableSaveButtonUserUpload');
            return;
        }

        if ($("#rdoLicenseForeign").is(':checked')) {
            window.alert("Bitte wähle eine andere Lizenz. Wir bitten dich das Bild auf Wikimedia hochzuladen und so einzubinden.");
            $(window).trigger('enableSaveButtonUserUpload');
            return;
        }

        if ($("#rdoLicenseByUploader").is(':checked')) {
            var licenseOwner = $("#txtLicenseOwner").val();
            if (licenseOwner.trim() === "") {
                window.alert("Bitte gib deinen Namen als Lizenzgeber an.");
                $(window).trigger('enableSaveButtonUserUpload');
                return;
            }

            fnOnSave(imageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    };
    return SaveUploadedImage;
})();
//# sourceMappingURL=ImageUpload.js.map
