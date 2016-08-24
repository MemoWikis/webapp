//If changed, please change server side backup validation as well

fnAddRegExMethod("Email", /.+@.+\..+/, "Bitte gib eine gültige Emailadresse an.");
fnAddRegExMethod("UserName", /^[A-Z\u00e4\u00f6\u00fc\u00df0-9 _-]*$/i, "Bitte verwende nur Buchstaben (ohne Akzente etc.), Zahlen, Bindestrich, Unterstrich und Leerzeichen.");
fnAddRegExMethod("UserName2", /[A-Z\u00e4\u00f6\u00fc\u00df].*[A-Z\u00e4\u00f6\u00fc\u00df]/i, "Bitte verwende mindestens 2 Buchstaben.");
fnAddRegExMethod("UserName3", /^[^_-].*[^_-]$/, "Bitte verwende keinen Bindestrich oder Unterstrich am Anfang oder am Ende.");


var validationSettingsRegistration = {
    rules: {
        Name: {
            required: true,
            minlength: 2,
            maxlength: 40,
            UserName: true,
            UserName2: true,
            UserName3: true,
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
