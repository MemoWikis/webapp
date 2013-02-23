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
    });
    $("#aImageUpload").click(function () {
        $("#modalImageUpload").modal('show');
    });
    $("#txtLicenceOwner").val("Robert Mischke");
    $('#Title').defaultText();
    $('#Text').defaultText();
});
