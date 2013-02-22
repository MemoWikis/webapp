/// <reference path="../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../Shared/ImageUpload/ImageUpload.ts" />

declare var isEditMode: bool;
declare var questionSetId: number;

class ImageUploadModalInit
{
    constructor(isEditMode : bool, questionSetId : number) {
        if (isEditMode) {
            $("#modalImageUpload").attr("data-endpoint", "/QuestionSet/UploadImage/" + questionSetId);
        } else {
            $("#modalImageUpload").attr("data-endpoint", "/QuestionSet/UploadImage/");
        }
    }
}

$(function () {
    new ImageUploadModalInit(isEditMode, questionSetId);
    var imageUploadModal = new ImageUploadModal();

    $("#aImageUpload").click(function () { 
        $("#modalImageUpload").modal('show');
    }); 

    $("#txtLicenceOwner").val("Robert Mischke");

    $('#Title').defaultText();
    $('#Text').defaultText();
});