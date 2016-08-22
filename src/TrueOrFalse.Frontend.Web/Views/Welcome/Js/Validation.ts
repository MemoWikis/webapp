fnAddRegExMethod("Email", /.+@.+\..+/, "Bitte gib eine gültige Emailadresse an.");

var validationSettingsRegistration = {
    rules: {
        Name: {
            required: true,
            minlength: 4,
            maxlength: 40,
        },
        Email: {
            required: true,
            Email: true,
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
        },
        Email: {
            required: "Du musst eine Emailadresse angeben.",
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
