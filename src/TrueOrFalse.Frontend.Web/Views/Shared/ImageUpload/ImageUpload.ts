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

    Load() {
        $("#divWikimediaSpinner").show();
        $("#divWikimediaError").hide();
        $("#previewWikimediaImage").hide();

        var url = $("#txtWikimediaUrl").val();
        var self = this;

        $.ajax({
            type: 'POST', true: false, cache: false,
            url: "/ImageUpload/FromWikimedia/",
            data: "url=" + url,
            error: function (error) { console.log(error); alert("Ein Fehler ist aufgetreten."); },
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
    }
}

class ImageUploadModal
{
    Mode: ImageUploadModalMode;
    WikimediaPreview: WikimediaPreview = new WikimediaPreview();

    ImageThumbUrl: string;
    ImageGuid: string;
    LicenceOwner: string;

    constructor() {
        this.Mode = ImageUploadModalMode.Wikimedia;
        this.InitUploader();
        this.InitTypeRadios();
        this.InitLicenceRadio();

        var self = this;
        $("#txtWikimediaUrl").change(function () { self.WikimediaPreview.Load(); });
        $("#aSaveImage").click(function () {  self.SaveImage();  });
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
            console.log(event + " " + id + " " + filename + " " + reason);
            alert("Ein Fehler ist aufgetreten");
        })
        .on('complete', function (event, id, filename, responseJSON) {
            $("#divUploadProgress").hide();
            $("#previewImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.FilePath + '"> ');
            $("#previewImage").show();
            $("#divLegalInfo").show();
            $("#modalBody").stop().scrollTo('100%', 800);

            self.ImageThumbUrl = responseJSON.FilePath;
            self.ImageGuid = responseJSON.Guid;
            self.LicenceOwner = $("#txtLicenceOwner").val();

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

    InitLicenceRadio() {
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
    }

    SaveImage() {
        if (this.Mode == ImageUploadModalMode.Wikimedia) {
            SaveWikipediaImage.Run(this.WikimediaPreview, this._onSave);
        }

        if (this.Mode == ImageUploadModalMode.Upload) {
            SaveUploadedImage.Run(this.ImageThumbUrl, this._onSave);
        }
    }

    _onSave: Function;
    OnSave(func: Function) { 
        this._onSave = func;
    }

    SetTitle(title: string) {
        $("#modalImageUpload .modal-title").html(title);
    }
}

class SaveWikipediaImage
{
    static Run(wikiMediaPreview: WikimediaPreview, fnOnSave: Function) {
        if (!wikiMediaPreview.SuccessfullyLoaded) {
            alert("Bitte lade ein Bild ueber eine Wikipedia URL.");//bei diesem Fehler bleibt wmd pane
        } else {
            fnOnSave(wikiMediaPreview.ImageThumbUrl);
            $("#modalImageUpload").modal("hide");
        }
    }
}

class SaveUploadedImage
{
    static Run(imageThumbUrl : string, fnOnSave: Function) {

        if ($('#divLegalInfo:hidden').length != 0) {
            alert("Hups, bitte wähle zuerst ein Bild aus.");
            return;
        }

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
    }
}