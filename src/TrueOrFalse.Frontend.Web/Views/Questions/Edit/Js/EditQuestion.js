$(function () {
    $('#Question').defaultText();
    $('#Description').defaultText();
    $('.control-label .show-tooltip').append($("<i class='fa fa-info-circle'></i>"));
    var editorQuestion = new MarkdownQuestionExt();
    var editorDescription = new MarkdownDescription();
});
