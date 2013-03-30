$(function () {
    $('#Question').defaultText();
    $('#Description').defaultText();
    $('.control-label .show-tooltip').append($("<i class='icon-info-sign'></i>"));
    var editorQuestion = new MarkdownQuestionExt();
    var editorDescription = new MarkdownDescription();
});
