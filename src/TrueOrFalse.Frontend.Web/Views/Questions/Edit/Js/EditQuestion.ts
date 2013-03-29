/// <reference path="MarkdownEditor.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />


$(function () {
    $('#Question').defaultText();
    $('#Description').defaultText();

    var markdown = new MarkDownEditor();
});