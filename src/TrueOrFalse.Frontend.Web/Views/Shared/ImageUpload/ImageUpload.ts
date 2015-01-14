/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.scrollTo.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/fineUploader.d.ts" />

enum ImageUploadModalMode{ 
    Wikimedia,
    Upload
}

class WikimediaPreview
{
    SuccessfullyLoaded: boolean= false;
    SuccessfullyLoadedImageUrl: string;

    ImageThumbUrl: string;
    ImageName: string;

    Load(onSuccess : Function, onError : Function) {
        $("#divWikimediaError").hide();
        $("#previewWikimediaImage").hide();

        var url = $("#txtWikimediaUrl").val();
        var self = this;

        $.ajax({
            type: 'POST', true: false, cache: false,
            url: "/ImageUpload/FromWikimedia/",
            data: "url=" + url,
            error: function (error) { window.console.log(error); window.alert("Ein Fehler ist aufgetreten."); },
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
    }
}

class ImageUploadModal
{
    Mode: ImageUploadModalMode;
    WikimediaPreview = new WikimediaPreview();

    ImageThumbUrl: string;
    ImageGuid: string;
    LicenseOwner: string;

    SaveButton: JQuery;
    SaveButtonSpinner: JQuery;

    PreviewLoadOngoing: boolean;

    _onPreviewLoadSuccess: Function;
    _onPreviewLoadError: Function;

    constructor() {
        this.Mode = ImageUploadModalMode.Wikimedia;
        this.InitUploader();
        this.InitTypeRadios();
        this.InitLicenseRadio();

        this.SaveButton = $("#aSaveImage");
        this.SaveButtonSpinner = this.SaveButton.find("i");

        this.SaveButtonSpinner.hide();

        var self = this;

        this._onPreviewLoadSuccess = function () {
            self.SaveButtonSpinner.hide();
            self.SaveButton.removeClass('disabled').find($('span')).html('Bild speichern');
            self.PreviewLoadOngoing = false;
        }

        this._onPreviewLoadError = function () {
            self.ResetModal();
        }

        $('#modalImageUploadDismiss').click(function () {
            self.ResetModal(true);
        });

        $('#txtWikimediaUrl').on('input', function () {
            if ($('#txtWikimediaUrl').val() !== "") {
                self.SaveButton.removeClass('disabled');
            } else {
                self.SaveButton.addClass('disabled');
            }
        });
        $("#txtWikimediaUrl").change(function () {
            self.StartPreviewLoad();
        });
        this.SaveButton.click(function (e) {
            if (!$(e.target).hasClass('disabled')) {
                if (self.WikimediaPreview.SuccessfullyLoaded) {
                    self.SaveImage();
                } else if (!self.PreviewLoadOngoing) {
                    self.StartPreviewLoad();
                }
            }
        });
    }

    InitUploader() {
        var self = this;
        $('#fileUpload').fineUploader({
            uploaderType: 'basic',
            button: $('#fileUpload')[0],
            request: { endpoint: '/ImageUpload/File' },
            multiple: false,
            debug: false,
            validation: {
                allowedExtensions: ['jpeg', 'jpg', 'png'],
                sizeLimit: 2097152 // 2MB = 2048 * 1024 bytes
            }
        })
        .on('error', function (event, id, filename, reason) {
            window.console.log(event + " " + id + " " + filename + " " + reason);
            window.alert("Ein Fehler ist aufgetreten");
        })
        .on('complete', function (event, id, filename, responseJSON) {
            $("#divUploadProgress").hide();
            $("#previewImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.FilePath + '"> ');
            $("#previewImage").show();
            $("#divLegalInfo").show();
            $("#modalBody").stop().scrollTo('100%', 800);

            self.ImageThumbUrl = responseJSON.FilePath;
            self.ImageGuid = responseJSON.Guid;
            self.LicenseOwner = $("#txtLicenseOwner").val();

        })
        .on('progress', function (event, id, filename, uploadedBytes: number, totalBytes: number) {
            $("#previewImage").hide();
            $("#divUploadProgress").show();
            $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    }

    InitTypeRadios() {
        var self = this;

        $("#rdoImageWikimedia").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").hide();
                $("#divWikimedia").show();
                self.Mode = ImageUploadModalMode.Wikimedia;
            }
        });

        $("#rdoImageUpload").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                self.Mode = ImageUploadModalMode.Upload;
            }
        });
    }

    InitLicenseRadio() {
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
    }

    StartPreviewLoad() {
        this.SaveButton.addClass("disabled");
        this.SaveButtonSpinner.show();
        if (!this.PreviewLoadOngoing) {
            this.WikimediaPreview = new WikimediaPreview();
            this.WikimediaPreview.Load(this._onPreviewLoadSuccess, this._onPreviewLoadError);
            this.PreviewLoadOngoing = true;
            $('#txtWikimediaUrl').attr('disabled', 'disabled')
        }
    }

    SaveImage() {
        this.SaveButtonSpinner.show();
        this.SaveButton.addClass("disabled");

        window.setTimeout(function() { //Timeout to have spinner spin
            if (this.Mode === ImageUploadModalMode.Wikimedia) {
                SaveWikipediaImage.Run(this.WikimediaPreview, this._onSave);
            }

            if (this.Mode === ImageUploadModalMode.Upload) {
                SaveUploadedImage.Run(this.ImageThumbUrl, this._onSave);
            }
        }, 20);

       
    }

    ResetModal(resetInput: boolean = false) {
        this.WikimediaPreview = new WikimediaPreview();
        this.SaveButtonSpinner.hide();
        $('#previewWikimediaImage').html('');
        if (resetInput) {
            $('#txtWikimediaUrl').val('');
        }
        $('#txtWikimediaUrl').removeAttr('disabled').trigger('input');

        this.SaveButton.find($('span')).html('Vorschau laden');
        this.PreviewLoadOngoing = false;
    }

    _onSave: Function;
    OnSave(func: Function) { 
        this._onSave = func;
    }

    SetTitle(title: string) {
        $("#modalImageUpload .modal-title").html(title);
    }
}

//class SaveButton {
//    Button: JQuery;
//    SavesPreview: boolean;

//    constructor() {
//        this.Button = $("#aSaveImage");
//    }

//    Enable() {
//        this.Button.removeClass('disabled');
//    }

//    Disable() {
//        this.Button.addClass('disabled');
//    }

//    MakePreviewLoadButton() {
//        this.Button.html('Vorschau laden');
//    }

//    MakeImageSaveButton() {
//        this.Button.html('Bild speichern');
//    }
//}

class SaveWikipediaImage
{
    static Run(wikiMediaPreview: WikimediaPreview, fnOnSave: Function) {
        if (!wikiMediaPreview.SuccessfullyLoaded) {
            window.alert("Bitte lade ein Bild über eine Wikipedia-URL.");
        } else {
            fnOnSave(wikiMediaPreview.ImageThumbUrl);
        }
    }
}

class SaveUploadedImage
{
    static Run(imageThumbUrl : string, fnOnSave: Function) {

        if ($('#divLegalInfo:hidden').length !== 0) {
            window.alert("Hups, bitte wähle zuerst ein Bild aus.");
            return;
        }

        if (!$("#rdoLicenseForeign").is(':checked') && !$("#rdoLicenseByUploader").is(':checked')) {
            window.alert("Bitte wähle eine andere Lizenz");
            return;
        }

        if ($("#rdoLicenseForeign").is(':checked')) {
            window.alert("Bitte wähle eine andere Lizenz. Wir bitten dich das Bild auf Wikimedia hochzuladen und so einzubinden.");
            return;
        }

        if ($("#rdoLicenseByUploader").is(':checked')) {
            var licenseOwner = $("#txtLicenseOwner").val();
            if (licenseOwner.trim() === "") {
                window.alert("Bitte gib deinen Namen als Lizenzgeber an.");
                return;
            }

            fnOnSave(imageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    }
}