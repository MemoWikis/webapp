var fnValidateForm = function (formSelector : string, customSettings : any, resetValidator = false) {

    var validationSettings = {
        highlight: function(element, errorClass, validClass) {
            if (element.type === "radio") {
                this.findByName(element.name).addClass(errorClass).removeClass(validClass);
            } else {
                $(element).addClass(errorClass).removeClass(validClass);
            }
            $(element).closest(".form-group:not(.JS-ValidationGroup)").addClass(errorClass).removeClass(validClass);
        },

        unhighlight: function(element, errorClass, validClass) {
            if (element.type === "radio") {
                this.findByName(element.name).removeClass(errorClass).addClass(validClass);
            } else {
                $(element).removeClass(errorClass).addClass(validClass);
            }
            $(element).closest(".form-group:not(.JS-ValidationGroup)").removeClass(errorClass).addClass(validClass);
        },

        errorPlacement: function(error, element) {
            if (element.hasClass("JS-ValidationGroupMember")) {
                error.appendTo(element.closest(".JS-ValidationGroup"));
            } else {
                if (element.attr('type') == "checkbox") {
                    error.append('</br>').css('display', 'inline');
                }
                error.insertAfter(element);
            }
        },

        errorClass: "ValidationError",
        ignore: ":hidden, .JS-ValidationIgnore",
    }

    $.extend(true, validationSettings, customSettings);

    var elemForm = $(formSelector).first();
    if (resetValidator) {
        if (elemForm.data("validator") != undefined)
            elemForm.data("validator", null);    
    }

    //jQuery.validator.addMethod(
    //    "numberCommaFormat",
    //    function (value, element) {
    //        window.alert('Format-Regel');

    //        return fnNumberFormatCorrect(value)            
    //    },
    //    "Die Zahl konnte nicht erkannt werden. Bitte verwende nur Ziffern und Komma."
    //);
    
    var validator = $(formSelector).validate(validationSettings);

    return validator;
}

var fnAddMethodWithOptionalCallback = function (
    ruleName: string,
    condition: (val) => boolean,
    message: string,
    callbackOnSuccess: (val, elem) => any = function (val, elem) { },
    callbackOnError: (val, elem) => any = function (val, elem) { }
    ) {
    $.validator.addMethod(
        ruleName,
        function (value, element) {
            if (condition(value)) {
                callbackOnSuccess(value, element);
                return true;
            } else {
                callbackOnError(value, element);
                return false;
            }
        },
        message
        );
}

var fnAddGermanDecimalRule = function (
    callbackOnSuccess: (val, elem) => any = function (val, elem) { },
    callbackOnError: (val, elem) => any = function (val, elem) { }) {
    fnAddMethodWithOptionalCallback(
    'GermanDecimal',
    fnNumberFormatCorrect,
    'Die Zahl konnte nicht erkannt werden. Bitte verwende nur Ziffern und Komma.',
    callbackOnSuccess,
    callbackOnError
    );}

var fnNumberFormatCorrect = function (value): boolean {
    value = $.trim(value);
    return (value === "")
            || (/^\d+[.,]*\d*$/.test(value)
                || /^\d*[.,]*\d+$/.test(value)
                 && isFinite(parseFloat(value.replace(',', '.'))));
}

var fnAddRegExMethod = function(name, regEx, message){

    $.validator.addMethod(
        name,
        function (value, element) {

            if (this.optional(element)) {
                return true;
            }
            if (typeof regEx === "string") {
                regEx = new RegExp(regEx);
            }
            return regEx.test(value);
        },
        message
    );
}

var fnAddAllOrNothingMethod = function (name, message) {

    $.validator.addMethod(
        name,
        function (value, element) {
            var members = $(element).closest('.JS-ValidationGroup').find('.JS-ValidationGroupMember');
            var firstFilledIn = ($(members[0]).val() != "");
            var allOfSameValue = true;
            
            if (members.length > 0) {
                var i = 1;
                while (i < members.length && allOfSameValue) {
                    if (firstFilledIn != ($(members[i]).val() != ""))
                        allOfSameValue = false;
                    i++;
                }
            }
            return allOfSameValue;
        },
        message
    );

}

//Add require_from_group method with custom message
//jQuery.validator.addMethod("methodName", $.validator.methods.require_from_group, "Custom massage");

//var fnDependentField = function(selectorPrimaryField, selectorDependentField, domDependentField) {
    
//}

//var fnAddOtherFieldFirstMethod = function (selectorPrimaryField, selectorDependentField, methodName, message) {

//    jQuery.validator.addMethod(
//        name,
//        function (value, element) {

//            if (this.optional(element)) {
//                return true;
//            }
//            if (typeof regEx === "string") {
//                regEx = new RegExp(regEx);
//            }
//            return regEx.test(value);
//        },
//        message
//        );
//}


//jQuery.validator.addMethod("otherFieldFirst", )

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