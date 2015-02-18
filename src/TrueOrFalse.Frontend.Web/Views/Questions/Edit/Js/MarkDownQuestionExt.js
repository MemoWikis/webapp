/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/markdown.d.ts" />
/// <reference path="../../../Images/ImageUpload/ImageUpload.ts" />
var MarkdownQuestionExt = (function () {
    function MarkdownQuestionExt() {
        var _this = this;
        $("#openExtendedQuestion").click(function (e) {
            e.preventDefault();
            $("#extendedQuestion").show();

            if (!_this._isInitialized)
                _this.InitEditor();
        });

        $("#hideExtendedQuestion").click(function (e) {
            e.preventDefault();
            $("#extendedQuestion").hide();
        });

        $("#OpenImageUpload").click(function (e) {
            e.preventDefault();
            $("#extendedQuestion").show();
            if (!_this._isInitialized)
                _this.InitEditor();
            $('#wmd-image-button-1').trigger('click');
        });

        $("#extendedQuestion").watch({
            properties: "display",
            callback: this.ToggleButtons
        });

        if ($("#wmd-input-1").html().trim().length > 0) {
            $("#extendedQuestion").show();
            this.InitEditor();
        }

        $("#wmd-input-1").on('input paste change', function () {
            if ($(this).val()) {
                $('#hideExtendedQuestion').hide();
            } else {
                $('#hideExtendedQuestion').show();
            }
        });
    }
    MarkdownQuestionExt.prototype.InitEditor = function () {
        var converter = Markdown.getSanitizingConverter();
        converter.hooks.chain("preBlockGamut", function (text, rbg) {
            return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                return "<blockquote>" + rbg(inner) + "</blockquote>\n";
            });
        });

        var editor = new Markdown.Editor(converter, "-1");
        editor.hooks.set("insertImageDialog", function (callback) {
            var imageUploadModal = new ImageUploadModal();
            imageUploadModal.OnSave(function (url) {
                var sourceString = imageUploadModal.Mode === 0 /* Wikimedia */ ? "wikimedia" : "upload";

                $.ajax({
                    type: "POST",
                    url: "/Fragen/Bearbeite/StoreImage",
                    async: false,
                    data: {
                        imageSource: sourceString,
                        questionId: $("#questionId").val(),
                        wikiFileName: imageUploadModal.WikimediaPreview.ImageName,
                        uploadImageGuid: imageUploadModal.ImageGuid,
                        uploadImageLicenseOwner: imageUploadModal.LicenseOwner,
                        markupEditor: ""
                    },
                    success: function (result) {
                        if (result.NewQuestionId !== -1) {
                            $("#questionId").val(result.NewQuestionId);
                        }
                        callback(result.PreviewUrl);

                        if (window.getSelection)
                            window.getSelection().removeAllRanges(); //Clear selection to avoid text in markdown being accidentally replaced http://stackoverflow.com/a/13415236
                        else if (window.document.selection)
                            window.document.selection.empty();

                        $("#wmd-input-1").trigger('change');
                        $("#modalImageUpload").modal("hide");
                    },
                    error: function (x, y) {
                        window.alert('Das Bild konnte leider nicht gespeichert werden.');
                        $("#modalImageUpload").modal("hide");
                        imageUploadModal.ResetModal();
                    }
                });
            });

            var extendedMarkupContainsImage = $("#wmd-input-1").val().toLowerCase().indexOf('/images/questions/') >= 0;

            if (!extendedMarkupContainsImage) {
                $("#modalImageUpload").modal('show');
            } else {
                window.alert('Zur Zeit kann leider nur ein Bild pro Frage gespeichert werden. ' + 'Um ein anderes Bild zu verwenden, lösche bitte das vorhandene (dazu den automatisch eingefügten Code aus dem Textfeld "Frage erweitert" löschen).');
            }

            return true;
        });

        editor.run();
        this._isInitialized = true;
    };

    MarkdownQuestionExt.prototype.ToggleButtons = function (data, i) {
        if (data.vals[i] === "none") {
            $('#openExtendedQuestion').closest('.form-group').show();
            $('#hideExtendedQuestion').hide();
        } else {
            $('#openExtendedQuestion').closest('.form-group').hide();
            if (!$('#wmd-input-1').val()) {
                $('#hideExtendedQuestion').show();
            }
        }
    };
    return MarkdownQuestionExt;
})();
//# sourceMappingURL=MarkdownQuestionExt.js.map
