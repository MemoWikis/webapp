var ImageUploadModalInit = (function () {
    function ImageUploadModalInit(isEditMode, questionSetId) {
        if(isEditMode) {
            $("#modalImageUpload").attr("data-endpoint", "/QuestionSet/UploadImage/" + questionSetId);
        } else {
            $("#modalImageUpload").attr("data-endpoint", "/QuestionSet/UploadImage/");
        }
    }
    return ImageUploadModalInit;
})();
$(function () {
    new ImageUploadModalInit(isEditMode, questionSetId);
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
            $("#ImageUploadedGuid").val("--");
        }
    });
    $("#aImageUpload").click(function () {
        $("#modalImageUpload").modal('show');
    });
    $("#txtLicenceOwner").val("Robert Mischke");
    $('#Title').defaultText();
    $('#Text').defaultText();
});
