$(function () {
    var imageUploadModal = new ImageUploadModal();
    imageUploadModal.OnSave(function (url) {
        $("#questionSetImg").attr("src", url);
        if(imageUploadModal.Mode == ImageUploadModalMode.Wikimedia) {
            $("#ImageIsNew").val("true");
            $("#ImageSource").val("wikimedia");
            $("#ImageWikiFileName").val(imageUploadModal.WikimediaPreview.ImageName);
        }
        if(imageUploadModal.Mode == ImageUploadModalMode.Upload) {
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
