var UserRow = (function () {
    function UserRow() {
    }
    UserRow.GetUserId = function (elem) {
        var parentRow = elem.closest("[data-UserId]");
        return parseInt(parentRow.attr("data-UserId"));
    };
    return UserRow;
})();
//# sourceMappingURL=UserRow.js.map
