var validationSettings_PasswordResetForm = {
    rules: {
        NewPassword1: {
            required: true,
            minlength: 5,
            maxlength: 40,
        },
        NewPassword2: {
            required: true,
            equalTo: "#NewPassword1"
        },
    },
    messages: {

        NewPassword2: {
            equalTo: "Die beiden Passwörter stimmen nicht überein.",
        },
    },
    onkeyup: false
}

$(function () {
    var validator = fnValidateForm("#PasswordResetForm", validationSettings_PasswordResetForm, false);
});