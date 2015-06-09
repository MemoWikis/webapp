var DateRow = (function () {
    function DateRow() {
    }
    DateRow.HideRow = function (dateId) {
        $("[data-date-id=" + dateId + "]").fadeOut(600);
    };
    return DateRow;
})();
//# sourceMappingURL=DateRow.js.map
