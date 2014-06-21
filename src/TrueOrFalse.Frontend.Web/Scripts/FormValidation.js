var fnValidateForm = function (formSelector, customSettings) {
    var validationSettings = {
        highlight: function (element, errorClass, validClass) {
            if (element.type === "radio") {
                this.findByName(element.name).addClass(errorClass).removeClass(validClass);
            } else {
                $(element).addClass(errorClass).removeClass(validClass);
            }
            $(element).closest(".form-group").addClass(errorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            if (element.type === "radio") {
                this.findByName(element.name).removeClass(errorClass).addClass(validClass);
            } else {
                $(element).removeClass(errorClass).addClass(validClass);
            }
            $(element).closest(".form-group").removeClass(errorClass).addClass(validClass);
        },
        errorPlacement: function (error, element) {
            if (element.hasClass("JS-ValidationGroupMember")) {
                error.appendTo(element.closest(".JS-ValidationGroup"));
            } else
                error.insertAfter(element);
        },
        errorClass: "ValidationError"
    };

    $.extend(true, validationSettings, customSettings);

    $(formSelector).validate(validationSettings);
};

var fnAddRegExMethod = function (name, regEx, message) {
    jQuery.validator.addMethod(name, function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        if (typeof regEx === "string") {
            regEx = new RegExp(regEx);
        }
        return regEx.test(value);
    }, message);
};
//$.validator.addMethod(
//    "requiredOrCheckbox",
//    function (value, element) {
//        debugger;
//        if ($(element).closest(".JS-InputWithCheckbox").find("input[type='checkbox']").length != 0){
//            var checkboxChecked = $(element).closest(".JS-InputWithCheckbox").find("input[type='checkbox']").is(":checked");
//            if (value == "") {
//                if (checkboxChecked) {
//                    return true;
//                } else
//                    return false;
//            } else
//                return true;
//        } else
//                return true;
//    },
//    "Bitte fülle das Feld aus oder klicke die Checkbox an."
//);
//$.validator.addMethod(
//    "test",
//    function (value, element) {
//        debugger;
//        return false;
//    },
//    "test"
//);
//# sourceMappingURL=FormValidation.js.map
