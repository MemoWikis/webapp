var MarkdownQuestionExt = (function () {
    function MarkdownQuestionExt() {
        var _this = this;
        $("#openExtendedQuestion").click(function (e) {
            e.preventDefault();
            $("#extendedQuestion").toggle();
            if(!_this._isOpen) {
                _this.InitEditor();
            }
        });
        if($("#wmd-input-1").html().trim().length > 0) {
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
                if(imageUploadModal.Mode == ImageUploadModalMode.Wikimedia) {
                    callback(imageUploadModal.WikimediaPreview.ImageThumbUrl);
                }
                if(imageUploadModal.Mode == ImageUploadModalMode.Upload) {
                    callback(url);
                }
            });
            $("#modalImageUpload").modal('show');
            return true;
        });
        editor.run();
        this._isOpen = true;
    };
    return MarkdownQuestionExt;
})();
