class UserRow {

    static GetUserId(elem : JQuery) : number {
        var parentRow = elem.closest("[data-UserId]");
        return parseInt(parentRow.attr("data-UserId"));
    }
}