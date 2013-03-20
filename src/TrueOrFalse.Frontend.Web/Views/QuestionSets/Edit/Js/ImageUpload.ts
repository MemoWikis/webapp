/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../Shared/ImageUpload/ImageUpload.ts" />

declare var isEditMode: bool;
declare var questionSetId: number;


$(function () {
    var imageUploadModal = new ImageUploadModal();
    imageUploadModal.OnSave(function (url: string) {
        $("#questionSetImg").attr("src", url);

        if (imageUploadModal.Mode == ImageUploadModalMode.Wikimedia) {
            $("#ImageIsNew").val("true")
            $("#ImageSource").val("wikimedia")
            $("#ImageWikiFileName").val(imageUploadModal.WikimediaPreview.ImageName)
        }

        if (imageUploadModal.Mode == ImageUploadModalMode.Upload) {
            $("#ImageIsNew").val("true")
            $("#ImageSource").val("upload")
            $("#ImageGuid").val(imageUploadModal.ImageGuid);
            $("#ImageLicenceOwner").val(imageUploadModal.LicenceOwner);
        }
    });

    $("#aImageUpload").click(function () { 
        $("#modalImageUpload").modal('show');
    }); 

    $("#txtLicenceOwner").val("Robert Mischke");

    $('#Title').defaultText();
    $('#Text').defaultText();
});