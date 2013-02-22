/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/jquery.scrollTo.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/fineUploader.d.ts" />

enum ImageUploadModalMode{ 
    Wikimedia,
    Upload
}

class WikimediaPreview
{
}

class ImageUploadModal
{
    _mode: ImageUploadModalMode;

    constructor() {
        this._mode = ImageUploadModalMode.Wikimedia;
        this.InitUploader();
        this.InitTypeRadios();
        this.InitLicenceRadio();

        var self = this;
        $("#aSaveImage").click(function () { self.SaveImage(); });
        $("#txtWikimediaUrl").change(function () { })
    }

    InitUploader() {

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
        })
        .on('progress', function (event, id, filename, uploadedBytes: number, totalBytes: number) {
            $("#previewImage").hide();
            $("#divUploadProgress").show();
            $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    }

    InitTypeRadios() {
        $("#rdoImageWikimedia").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").hide();
                $("#divWikimedia").show();
                this._mode = ImageUploadModalMode.Wikimedia;
            }
        });

        $("#rdoImageUpload").change(function () {
            if ($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                this._mode = ImageUploadModalMode.Upload;
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
        if (this._mode = ImageUploadModalMode.Wikimedia) {
            alert("save wikimedia")
        }
        if (this._mode = ImageUploadModalMode.Upload) {
            if ($("#rdoLicenceForeign").is(':checked')) {
                alert("Bitte wählen Sie eine andere Lizenz. Wir bitten Dich das Bild auf Wikimedia hochzuladen und so einzubinden.")
            }

            if ($("#rdoLicenceByUloader").is(':checked')) {
                var licenceOwner = $("#txtLicenceOwner").val();
                if (licenceOwner.trim() == "") {
                    alert("Bitte gib Deinen Namen als Lizenzgeber an.")
                }
            }

            //get GUID
            //send licenceType and licenceOwner 

            alert("save upload")
        }
    }
}