/// <reference path="MarkdownQuestionExt.ts" />
/// <reference path="MarkdownDescription.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
$(function () {
    $('#Question').defaultText();
    $('#Description').defaultText();

    $('.control-label .show-tooltip').append($("<span> <i class='fa fa-info-circle'></i></span>"));

    var editorQuestion = new MarkdownQuestionExt();
    var editorDescription = new MarkdownDescription();

    $('#ConfirmContentRights').click(function () {
    });
});
//# sourceMappingURL=EditQuestion.js.map
