var validationSettings_EditSet = {
    rules: {
        Title: {
            required: true,
        },
    },
}

$(function () {
    var validator =  fnValidateForm("#EditSetForm", validationSettings_EditSet, false);
});
