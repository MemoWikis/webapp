var Validations = (function () {
    function Validations() {
    }
    //http://stackoverflow.com/questions/46155/validate-email-address-in-javascript
    Validations.IsEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    return Validations;
})();
//# sourceMappingURL=Validations.js.map
