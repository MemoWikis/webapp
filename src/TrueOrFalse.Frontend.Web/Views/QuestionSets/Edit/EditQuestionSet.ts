/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/lib.d.ts" />

declare var isEditMode: bool;
declare var questionSetId: number;

class InitQuestionSetUploader
{
    constructor(isEditMode : bool, questionSetId : number) {
        if (isEditMode) {
            $("#fileUpload").attr("data-endpoint", "/QuestionSet/UploadImage/" + questionSetId);
        } else {
            $("#fileUpload").attr("data-endpoint", "/QuestionSet/UploadImage/");
        }
    }
}

$(function () {
    new InitQuestionSetUploader(isEditMode, questionSetId);

    $("#aImageUpload").click(function () { 
        $("#modalImageUpload").modal('show');
    }); 

    $("#txtLicenceOwner").val("Robert Mischke");

    $('#Title').defaultText();
    $('#Text').defaultText();
});