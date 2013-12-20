/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/markdown.d.ts" />
/// <reference path="../../../Shared/ImageUpload/ImageUpload.ts" />
var MarkdownQuestionExt = (function () {
    function MarkdownQuestionExt() {
        var _this = this;
        $("#openExtendedQuestion").click(function (e) {
            e.preventDefault();
            $("#extendedQuestion").toggle();

            if (!_this._isOpen)
                _this.InitEditor();
        });

        if ($("#wmd-input-1").html().trim().length > 0) {
            $("#extendedQuestion").show();
            this.InitEditor();
        }
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
                var sourceString = imageUploadModal.Mode == 0 /* Wikimedia */ ? "wikimedia" : "upload";

                $.post("/Fragen/Bearbeite/StoreImage", {
                    "imageSource": sourceString,
                    "questionId": $("#questionId").val(),
                    "wikiFileName": imageUploadModal.WikimediaPreview.ImageName,
                    "uploadImageGuid": imageUploadModal.ImageGuid,
                    "uploadImageLicenceOwner": imageUploadModal.LicenceOwner,
                    "markupEditor": ""
                }, function (result) {
                    if (result.NewQuestionId != -1) {
                        $("questionId").val(result.NewQuestionId);
                    }

                    callback(result.PreviewUrl);
                });
            });

            $("#modalImageUpload").modal('show');

            return true;
        });

        editor.run();
        this._isOpen = true;
    };
    return MarkdownQuestionExt;
})();
