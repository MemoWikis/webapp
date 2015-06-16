var NotLoggedIn = (function () {
    function NotLoggedIn() {
    }
    NotLoggedIn.Yes = function () {
        if ($("#IsLoggedIn").val() == "False")
            return true;

        return false;
    };

    NotLoggedIn.ShowErrorMsg = function () {
        $('#modalNotLoggedIn').modal('show');
    };
    return NotLoggedIn;
})();
//# sourceMappingURL=NotLoggedIn.js.map
