var validationSettings = {
    rules: {
        Question: {
            required: true
        },
        Description: {
            required: true
        },
        ConfirmContentRights: {
            required: true
        }
    },
    messages: {
        ConfirmContentRights: {
            required: "Bitte bestätige:"
        }
    }
};

$(function () {
    var validator = fnValidateForm("#EditQuestionForm", validationSettings, false);
});
//# sourceMappingURL=Validation.js.map
