/// <reference path="../../../../Scripts/typescript.defs/jquery.d.ts" />
/// <reference path="../../../../scripts/formvalidation.ts" />
var validationSettings_UserSettingsForm = {
    rules: {
        Name: {
            required: true
        },
        Email: {
            required: true
        }
    },
    messages: {
        Name: {
            required: "Bitte gib einen Nutzernamen an."
        },
        Email: {
            required: "Bitte gib eine gültige Emailadresse an."
        }
    }
};

$(function () {
    var validator = fnValidateForm("#UserSettingsForm", validationSettings_UserSettingsForm, false);
});
//# sourceMappingURL=Validation.js.map
