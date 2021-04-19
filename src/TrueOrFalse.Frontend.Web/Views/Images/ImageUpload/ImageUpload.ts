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
            type: 'POST', cache: false,
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

    SaveButtonWiki: JQuery;
    SaveButtonWikiSpinner: JQuery;
    SaveButtonUpload: JQuery;
    SaveButtonUploadSpinner: JQuery;

    PreviewLoadOngoing: boolean;

    _onPreviewLoadSuccess: Function;
    _onPreviewLoadError: Function;

    AllowedExtensions = ['jpeg', 'jpg', 'png', 'svg', 'gif'];
    MaxImageSize = 10485760; //10MB (in bytes)

    constructor() {
        this.Mode = ImageUploadModalMode.Wikimedia;
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
            self.SaveButtonWiki.removeClass('disabled').find($('span')).html('Bild übernehmen');
            self.PreviewLoadOngoing = false;
        }

        this._onPreviewLoadError = function () {
            self.ResetModal();
        }

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
            if (self.ExtensionIsAllowed($("#txtWikimediaUrl").val())) {
                self.StartPreviewLoad();
            } else if ($.trim($("#txtWikimediaUrl").val()) !== "") {
                window.alert('Zur Zeit können leider nur Bilder in folgenden Dateiformaten hochgeladen werden: ' + self.AllowedExtensions.join(', ') + ".");
            }

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

        $(window).on('enableSaveButtonUserUpload', function() {
                self.EnableSaveButtonUpload();
            }
        );

        $("#txtLicenseOwner").change(function () {
            self.LicenseOwner = $("#txtLicenseOwner").val();
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
                allowedExtensions: self.AllowedExtensions,
                sizeLimit: self.MaxImageSize
    }
        })
        .on('error', function (event, id, filename, reason) {
            window.console.log(event + " " + id + " " + filename + " " + reason);
            if (reason.indexOf('is too large, maximum file size is') !== -1) {
                window.alert("Das Bild ist leider zu groß. Die maximal erlaubte Größe ist " + self.MaxImageSize / 1048576 + " MB.");
            } else {
                window.alert("Ein Fehler ist aufgetreten");
            }
        })
        .on('complete', function (event, id, filename, responseJSON) {
            self.SaveButtonUpload.removeClass('disabled');
            $("#divUserUploadProgress").hide();
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
            $("#divUserUploadProgress").show();
            $("#divUserUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    }

    InitTypeRadios() {
        var self = this;

        $("#rdoImageWikimedia").change(function () {
            if ($(this).is(':checked')) {
                $("#divUserUpload, #ButtonsUserUpload").hide();
                $("#divWikimedia, #ButtonsWikimedia").show();
                self.Mode = ImageUploadModalMode.Wikimedia;
            }
        });

        $("#rdoImageUpload").change(function () {
            if ($(this).is(':checked')) {
                $("#divUserUpload, #ButtonsUserUpload").show();
                $("#divWikimedia, #ButtonsWikimedia").hide();
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
        this.SaveButtonWiki.addClass("disabled");
        this.SaveButtonWikiSpinner.show();
        if (!this.PreviewLoadOngoing) {
            this.WikimediaPreview = new WikimediaPreview();
            this.WikimediaPreview.Load(this._onPreviewLoadSuccess, this._onPreviewLoadError);
            this.PreviewLoadOngoing = true;
            //$('#txtWikimediaUrl').attr('disabled', 'disabled');
        }
    }

    SaveImage() {
       
        var self = this;

        if (self.Mode === ImageUploadModalMode.Wikimedia) {

            this.SaveButtonWikiSpinner.show();
            this.SaveButtonWiki.addClass("disabled");

            window.setTimeout(function () { //Timeout to have spinner spin
                SaveWikipediaImage.Run(self.WikimediaPreview, self._onSave);
            }, 10);
        }

        if (self.Mode === ImageUploadModalMode.Upload) {
        
            this.SaveButtonUploadSpinner.show();
            this.SaveButtonUpload.addClass("disabled");

            window.setTimeout(function () { //Timeout to have spinner spin
                SaveUploadedImage.Run(self.ImageThumbUrl, self._onSave);
            }, 10);
        }
    }

    ResetModal(resetInput: boolean = false) {
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
    }

    _onSave: Function;
    OnSave(func: Function) { 
        this._onSave = func;
    }

    SetTitle(title: string) {
        $("#modalImageUpload .modal-title").html(title);
    }

    EnableSaveButtonUpload() {
        this.SaveButtonUpload.removeClass('disabled');
        this.SaveButtonUploadSpinner.hide();
    }

    ExtensionIsAllowed(fileName: string): boolean {
        var fileExtension = fileName.toLowerCase().split(".").pop();

        if (this.AllowedExtensions.indexOf(fileExtension) !== -1)
            return true;

        return false;
    }
}

class SaveWikipediaImage
{
    static Run(wikiMediaPreview: WikimediaPreview, fnOnSave: Function) {
        if (!wikiMediaPreview.SuccessfullyLoaded) {
            window.alert("Bitte lade ein Bild über eine Wikimedia-URL.");
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
    }
}