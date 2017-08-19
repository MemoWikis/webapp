fnAddRegExMethod("VideoUrlRegex", /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/, "Mmmmh, das scheint keine gültige Youtube-URL zu sein.");
var validationSettings_EditSet = {
    rules: {
        Title: {
            required: true,
        },
        VideoUrl: {
            VideoUrlRegex: true,
        }
    },
};
$(function () {
    var validator = fnValidateForm("#EditSetForm", validationSettings_EditSet, false);
});
//# sourceMappingURL=Validation.js.map