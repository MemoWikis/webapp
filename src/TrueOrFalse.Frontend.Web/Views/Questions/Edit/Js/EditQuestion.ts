/// <reference path="MarkdownQuestionExt.ts" />
/// <reference path="MarkdownDescription.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/jqueryui.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />

$(function () {
    $('.control-label .show-tooltip').append($("<span> <i class='fa fa-info-circle'></i></span>"));

    var editorQuestion = new MarkdownQuestionExt();
    var editorDescription = new MarkdownDescription();

    $('#ConfirmContentRights').click(function() {
        
    });

    function updateSolutionBody() {
        var selectedValue = $("#ddlAnswerType").val();
        $.ajax({
            url: $("#urlSolutionEditBody").val() + '?questionId=' + $("#questionId").val() +  '&type=' + selectedValue,
            type: 'GET',
            success: function (data) { $("#answer-body").html(data); }
        });
    }
    $("#ddlAnswerType").change(updateSolutionBody);
    updateSolutionBody();
    $('[name="Visibility"]').change(function () {
        if ($('input[name="Visibility"]:checked').val() == "Owner") {
            $('#Agreement').hide();
        } else {
            $('#Agreement').show();
        }
    });
});