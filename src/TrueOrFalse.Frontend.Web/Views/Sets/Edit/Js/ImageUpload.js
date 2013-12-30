/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../Shared/ImageUpload/ImageUpload.ts" />

$(function () {
    var imageUploadModal = new ImageUploadModal();
    imageUploadModal.OnSave(function (url) {
        $("#questionSetImg").attr("src", url);

        if (imageUploadModal.Mode == 0 /* Wikimedia */) {
            $("#ImageIsNew").val("true");
            $("#ImageSource").val("wikimedia");
            $("#ImageWikiFileName").val(imageUploadModal.WikimediaPreview.ImageName);
        }

        if (imageUploadModal.Mode == 1 /* Upload */) {
            $("#ImageIsNew").val("true");
            $("#ImageSource").val("upload");
            $("#ImageGuid").val(imageUploadModal.ImageGuid);
            $("#ImageLicenceOwner").val(imageUploadModal.LicenceOwner);
        }
    });

    $("#aImageUpload").click(function () {
        $("#modalImageUpload").modal('show');
    });

    $("#txtLicenceOwner").val("Vorname Nachname");

    $('#Title').defaultText();
    $('#Text').defaultText();
});
//# sourceMappingURL=ImageUpload.js.map
