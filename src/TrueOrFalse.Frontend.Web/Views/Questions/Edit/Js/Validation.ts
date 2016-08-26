var validationSettings = {
    rules: {
        Question: {
            required: true,
        },
        Text: {
            required: true,
        },
        ConfirmContentRights: {
            required: true,  
        },
        
    },
    messages: {
       
        ConfirmContentRights: {
            required: "Bitte bestätige:",    
        },
       
    },
    errorPlacement: function (error, element) {
        debugger;
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
