var validationSettings_PasswordRecoveryForm = {
    rules: {
        Email: {
            required: true,
            remote: {
                url: "/Registrieren/IsEmailAvailable",
                type: "post",
                data: {
                    selectedEmail: function () {
                        return $("#Email").val();
                    }
                },
                dataType: 'json',
                dataFilter: function (data) {
                    var json = JSON.parse(data);
                    if (json.isAvailable != true) {
                        return "true";
                    } else {
                        return "false";
                    }
                }
            }
        },
       
    },
    messages: {

        Email: {
            remote: "Diese E-Mail-Adresse ist uns unbekannt. Wir benötigen die E-Mail-Adresse, mit der du dich registriert hast.",
        },

    },
    onkeyup: false
}

$(function () {
    var validator = fnValidateForm("#PasswordRecoveryForm", validationSettings_PasswordRecoveryForm, false);
});