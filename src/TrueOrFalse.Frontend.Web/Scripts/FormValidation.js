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
//# sourceMappingURL=FormValidation.js.map
