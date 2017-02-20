var validationSettings = {
    rules: {
        QuestionText: {
            required: true,
        },
        Text: {
            required: true,
        },
        ConfirmContentRights: {
            required: true,  
        },
        "choice-0": {
            required: true,
        },
        "choice-1": {
            required: true,
        },
    },
    messages: {
       
        ConfirmContentRights: {
            required: "Bitte bestätige:",    
        },
        "choice-0": {
            required: "Pflichtfeld",
        },
        "choice-1": {
            required: "Bitte gib mindestens eine falsche Antwort ein.",
        },
       
    },
    errorPlacement: function (error, element) {
        if (element.parent().attr("class") == "input-group") {
            error.insertAfter($(element).parent());
        }
        else {
            error.insertAfter(element);
        }
    }
}

$(function () {
    var validator =  fnValidateForm("#EditQuestionForm", validationSettings, false);
});
