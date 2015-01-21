/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/bootstrap.d.ts" />
/// <reference path="../../../../Scripts/typescript.defs/lib.d.ts" />
/// <reference path="../../../Images/ImageUpload/ImageUpload.ts" />

$(function () {
    var imageUploadModal = new ImageUploadModal();
    imageUploadModal.SetTitle("Kategoriebild hochladen");
    imageUploadModal.OnSave(function (url) {
        $("#categoryImg").attr("src", url);

        if (imageUploadModal.Mode === 0 /* Wikimedia */) {
            $("#ImageIsNew").val("true");
            $("#ImageSource").val("wikimedia");
            $("#ImageWikiFileName").val(imageUploadModal.WikimediaPreview.ImageName);
        }

        if (imageUploadModal.Mode === 1 /* Upload */) {
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
//# sourceMappingURL=ImageUpload.js.map
