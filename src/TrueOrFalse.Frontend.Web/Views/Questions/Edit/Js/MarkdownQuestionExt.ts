/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/markdown.d.ts" />
/// <reference path="../../../Shared/ImageUpload/ImageUpload.ts" />

class MarkdownQuestionExt
{ 
    _isOpen: bool;

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
                //take image guid
                //send store command
                //send current markup
                //retrieve previe url

                if (imageUploadModal.Mode == ImageUploadModalMode.Wikimedia) {
                    callback(imageUploadModal.WikimediaPreview.ImageThumbUrl);
                }

                if (imageUploadModal.Mode == ImageUploadModalMode.Upload) {
                    callback(url);
                }                
            });

            $("#modalImageUpload").modal('show');

            return true; // tell the editor that we'll take care of getting the image url
        });

        editor.run();
        this._isOpen = true;
    }
}