/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/markdown.d.ts" />
/// <reference path="../../../Shared/ImageUpload/ImageUpload.ts" />

class MarkdownQuestionExt
{ 
    _isOpen: boolean;

    constructor() {
        $("#openExtendedQuestion").click((e) => { 
            e.preventDefault();
            $("#extendedQuestion").toggle();

            if(!this._isOpen)
                this.InitEditor();
        });

        if ($("#wmd-input-1").html().trim().length > 0) {
            $("#extendedQuestion").show();
            this.InitEditor();
        }
    }

    InitEditor() 
    { 
        var converter = Markdown.getSanitizingConverter();
        converter.hooks.chain("preBlockGamut", function (text, rbg) {
            return text.replace(/^ {0,3}""" *\n((?:.*?\n)+?) {0,3}""" *$/gm, function (whole, inner) {
                return "<blockquote>" + rbg(inner) + "</blockquote>\n";
            });
        });

        var editor = new Markdown.Editor(converter, "-1");
        editor.hooks.set("insertImageDialog", function (callback) {

            var imageUploadModal = new ImageUploadModal();
            imageUploadModal.OnSave(function (url: string) {

                var sourceString = imageUploadModal.Mode == ImageUploadModalMode.Wikimedia ? "wikimedia" : "upload";

                $.post("/Fragen/Bearbeite/StoreImage",
                {
                    "imageSource": sourceString,
                    "questionId": $("#questionId").val(),
                    "wikiFileName": imageUploadModal.WikimediaPreview.ImageName,
                    "uploadImageGuid": imageUploadModal.ImageGuid,
                    "uploadImageLicenceOwner": imageUploadModal.LicenceOwner,
                    "markupEditor" : ""
                },
                    function (result) {
                        if (result.NewQuestionId != -1) {
                            $("questionId").val(result.NewQuestionId);
                        }   

                        callback(result.PreviewUrl);
                    });
                });

                $('#modalImageUploadDismiss').click(function() { callback(null); });//To dismiss markdown image upload dialogue together with modal

                $("#modalImageUpload").modal('show');

                return true; // tell the editor that we'll take care of getting the image url
        });

        editor.run();
        this._isOpen = true;
    }
}