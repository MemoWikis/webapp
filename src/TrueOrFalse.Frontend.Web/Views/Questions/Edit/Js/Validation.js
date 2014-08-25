var validationSettings = {
    rules: {
        Question: {
            required: true
        },
        Text: {
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
