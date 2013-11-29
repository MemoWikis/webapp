/// <reference path="MarkdownQuestionExt.ts" />
/// <reference path="MarkdownDescription.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />


$(function () {
    $('#Question').defaultText();
    $('#Description').defaultText();

    $('.control-label .show-tooltip').append($("<i class='fa fa-info-circle'></i>"));

    var editorQuestion = new MarkdownQuestionExt();
    var editorDescription = new MarkdownDescription();
});