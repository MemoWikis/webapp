var InitQuestionSetUploader = (function () {
    function InitQuestionSetUploader(isEditMode, questionSetId) {
        if(isEditMode) {
            $("#fileUpload").attr("data-endpoint", "/QuestionSet/UploadImage/" + questionSetId);
        } else {
            $("#fileUpload").attr("data-endpoint", "/QuestionSet/UploadImage/");
        }
    }
    return InitQuestionSetUploader;
})();
$(function () {
    new InitQuestionSetUploader(isEditMode, questionSetId);
    $("#aImageUpload").click(function () {
        $("#modalImageUpload").modal('show');
    });
    $("#txtLicenceOwner").val("Robert Mischke");
    $('#Title').defaultText();
    $('#Text').defaultText();
});
