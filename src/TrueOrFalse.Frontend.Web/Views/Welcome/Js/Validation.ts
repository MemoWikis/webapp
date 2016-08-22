fnAddRegExMethod("Email", /.+@.+\..+/, "Bitte gib eine gültige Emailadresse an.");

var validationSettingsRegistration = {
    rules: {
        Name: {
            required: true,
            minlength: 4,
            maxlength: 40,
            remote: {
                url: "/Registrieren/IsUserNameAvailable",
                type: "post",
                data: {
                    selectedName: function () {
                        return $("#Name").val();
                    }
                },
                dataType: 'json',
                dataFilter: function(data) {
                    var json = JSON.parse(data);
                    if (json.isAvailable == true) {
                        return "true";
                    } else {
                        return "false";
                    }
                }
            }
        },
        Email: {
            required: true,
            Email: true,
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
                    if (json.isAvailable == true) {
                        return "true";
                    } else {
                        return "false";
                    }
                }
            }
        },
        Password: {
            required: true,
            minlength: 5,
            maxlength: 40,
        },
        TermsAndConditionsApproved: {
            required: true,  
        }
        
    },
    messages: {
       
        Name: {
            required: "Du musst einen Benutzernamen angeben.",
            remote: "Dieser Nutzername ist bereits vergeben."
        },
        Email: {
            required: "Du musst eine Emailadresse angeben.",
            remote: "Diese Email-Adresse ist bereits registriert."
        },
        Password: {
            required: "Du musst ein Passwort angeben.",
        },
        TermsAndConditionsApproved: {
            required: "Bitte bestätige: ",
        }
       
    },
}

$(function () {
    var validator =  fnValidateForm("#RegistrationForm", validationSettingsRegistration, false);
});
