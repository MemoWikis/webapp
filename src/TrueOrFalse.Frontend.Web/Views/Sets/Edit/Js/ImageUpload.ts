 /// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../Images/ImageUpload/ImageUpload.ts" />

declare var isEditMode: boolean;
declare var questionSetId: number;


$(function () {
    var imageUploadModal = new ImageUploadModal();
    imageUploadModal.SetTitle("Bild für Lernset hochladen");
    imageUploadModal.OnSave(function (url: string) {
        $("#questionSetImg").attr("src", url);

        if (imageUploadModal.Mode == ImageUploadModalMode.Wikimedia) {
            $("#ImageIsNew").val("true");
            $("#ImageSource").val("wikimedia");
            $("#ImageWikiFileName").val(imageUploadModal.WikimediaPreview.ImageName);
        }

        if (imageUploadModal.Mode == ImageUploadModalMode.Upload) {
            $("#ImageIsNew").val("true");
            $("#ImageSource").val("upload");
            $("#ImageGuid").val(imageUploadModal.ImageGuid);
            $("#ImageLicenseOwner").val(imageUploadModal.LicenseOwner);
        }

        $("#modalImageUpload").modal("hide");
        imageUploadModal.ResetModal();
    });

    $("#aImageUpload").click(function () { 
        $("#modalImageUpload").modal('show');
    }); 
});