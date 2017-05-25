fnAddRegExMethod("VideoUrlRegex", /youtube.com\/watch.*[\?\&]v=/, "Mmmmh, das scheint keine gültige Youtube-URL zu sein.");

var validationSettings_EditSet = {
    rules: {
        Title: {
            required: true,
        },
        VideoUrl: {
            VideoUrlRegex: true,
        }
    },
}

$(function () {
    var validator =  fnValidateForm("#EditSetForm", validationSettings_EditSet, false);
});
