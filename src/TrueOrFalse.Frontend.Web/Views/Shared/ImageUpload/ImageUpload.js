var ImageUploadModalMode;
(function (ImageUploadModalMode) {
    ImageUploadModalMode._map = [];
    ImageUploadModalMode._map[0] = "Wikimedia";
    ImageUploadModalMode.Wikimedia = 0;
    ImageUploadModalMode._map[1] = "Upload";
    ImageUploadModalMode.Upload = 1;
})(ImageUploadModalMode || (ImageUploadModalMode = {}));
var WikimediaPreview = (function () {
    function WikimediaPreview() { }
    return WikimediaPreview;
})();
var ImageUploadModal = (function () {
    function ImageUploadModal() {
        this._mode = ImageUploadModalMode.Wikimedia;
        this.InitUploader();
        this.InitTypeRadios();
        this.InitLicenceRadio();
        var self = this;
        $("#aSaveImage").click(function () {
            self.SaveImage();
        });
        $("#txtWikimediaUrl").change(function () {
        });
    }
    ImageUploadModal.prototype.InitUploader = function () {
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
        }).on('progress', function (event, id, filename, uploadedBytes, totalBytes) {
            $("#previewImage").hide();
            $("#divUploadProgress").show();
            $("#divUploadProgress").html("'<b>" + filename + "</b>' wird hochgeladen.");
        });
    };
    ImageUploadModal.prototype.InitTypeRadios = function () {
        $("#rdoImageWikimedia").change(function () {
            if($(this).is(':checked')) {
                $("#divUpload").hide();
                $("#divWikimedia").show();
                this._mode = ImageUploadModalMode.Wikimedia;
            }
        });
        $("#rdoImageUpload").change(function () {
            if($(this).is(':checked')) {
                $("#divUpload").show();
                $("#divWikimedia").hide();
                this._mode = ImageUploadModalMode.Upload;
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
        if(this._mode = ImageUploadModalMode.Wikimedia) {
            alert("save wikimedia");
        }
        if(this._mode = ImageUploadModalMode.Upload) {
            if($("#rdoLicenceForeign").is(':checked')) {
                alert("Bitte wählen Sie eine andere Lizenz. Wir bitten Dich das Bild auf Wikimedia hochzuladen und so einzubinden.");
            }
            if($("#rdoLicenceByUloader").is(':checked')) {
                var licenceOwner = $("#txtLicenceOwner").val();
                if(licenceOwner.trim() == "") {
                    alert("Bitte gib Deinen Namen als Lizenzgeber an.");
                }
            }
            alert("save upload");
        }
    };
    return ImageUploadModal;
})();
