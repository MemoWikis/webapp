/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.scrollTo.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/fineUploader.d.ts" />

enum ImageUploadModalMode{ 
    Wikimedia,
    Upload
}

class WikimediaPreview
{
    SuccessfullyLoaded: bool = false;
    SuccessfullyLoadedImageUrl: string;

    ImageThumbUrl: string;

    Load() {
        $("#divWikimediaSpinner").show();
        $("#divWikimediaError").hide()
        $("#previewWikimediaImage").hide()

        var url = $("#txtWikimediaUrl").val();
        var self = this;

        $.ajax({
            type: 'POST', true: false, cache: false,
            url: "/ImageUpload/FromWikimedia/",
            data: "url=" + url,
            error: function (error) { console.log(error); alert("Ein Fehler ist aufgetreten.") },
            success: function (responseJSON) {
                $("#divWikimediaSpinner").hide()

                if (responseJSON.ImageNotFound) {
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
    }
}

class ImageUploadModal
{
    _mode: ImageUploadModalMode;
    WikimediaPreview: WikimediaPreview = new WikimediaPreview();

    ImageThumbUrl: string;

    constructor() {
        this._mode = ImageUploadModalMode.Wikimedia;
        this.InitUploader();
        this.InitTypeRadios();
        this.InitLicenceRadio();

        var self = this;
        $("#txtWikimediaUrl").change(function () { self.WikimediaPreview.Load(); })
        $("#aSaveImage").click(function () {  self.SaveImage();  });
    }

    InitUploader() {
        var self = this;
        $('#fileUpload').fineUploader({
            uploaderType: 'basic',
            button: $('#fileUpload')[0],
            request: { endpoint: $('#modalImageUpload').attr("data-endpoint") },
            multiple: false,
            debug: false,
            validation: {
                allowedExtensions: ['jpeg', 'jpg', 'png'],
                sizeLimit: 2097152 // 2MB = 2048 * 1024 bytes
            }
        })
        .on('error', function (event, id, filename, reason) {
            alert("Ein Fehler ist aufgetreten");
        })
        .on('complete', function (event, id, filename, responseJSON) {
            $("#divUploadProgress").hide();
            $("#previewImage").html('<b>Bildvorschau:</b><br/><img src="' + responseJSON.filePath + '"> ');
            $("#previewImage").show();
            $("#divLegalInfo").show();
            $("#modalBody").stop().scrollTo('100%', 800 );
            self.ImageThumbUrl = responseJSON.filePath;
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
                self._mode = ImageUploadModalMode.Wikimedia;
            }
        });

        $("#rdoImageUpload").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                self._mode = ImageUploadModalMode.Upload;
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

        if (this._mode == ImageUploadModalMode.Wikimedia) {
            this.SaveWikimediaImage();
        }

        if (this._mode == ImageUploadModalMode.Upload) {
            this.SaveUploadedImage();
        }
    }

    SaveWikimediaImage() { 
        if (!this.WikimediaPreview.SuccessfullyLoaded) {
            alert("Bitte lade ein Bild über eine Wikipedia URL.");
        } else { 
            console.log(this.WikimediaPreview);
            this._onSave(this.WikimediaPreview.ImageThumbUrl);
        }
    }

    SaveUploadedImage() { 

        if (!$("#rdoLicenceForeign").is(':checked') && !$("#rdoLicenceByUloader").is(':checked')) { 
            alert("Bitte wähle eine andere Lizenz");
        }

        if ($("#rdoLicenceForeign").is(':checked')) {
            alert("Bitte wähle eine andere Lizenz. Wir bitten Dich das Bild auf Wikimedia hochzuladen und so einzubinden.")
        }

        if ($("#rdoLicenceByUloader").is(':checked')) {
            var licenceOwner = $("#txtLicenceOwner").val();
            if (licenceOwner.trim() == "") {
                alert("Bitte gib Deinen Namen als Lizenzgeber an.")
            }

            //get GUID
            //send licenceType and licenceOwner
            this._onSave(this.ImageThumbUrl);
        }    
    }

    _onSave: Function;
    OnSave(func: Function) { 
        this._onSave = func;
    }
}