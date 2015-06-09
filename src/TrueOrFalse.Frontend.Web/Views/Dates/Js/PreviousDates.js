var PreviousDates = (function () {
    function PreviousDates() {
        $("#btnShowPreviousDates").click(function () {
            $("#divShowPreviousDates").hide();
            $("#captionPreviousDate").show();

            $.get("/Dates/RenderPreviousDates", function (htmlResult) {
                $("#previousDates").empty().animate({ opacity: 0.00 }, 0).append(htmlResult).animate({ opacity: 1.00 }, 600);

                $(".show-tooltip").tooltip();
            });
        });
    }
    return PreviousDates;
})();
//# sourceMappingURL=PreviousDates.js.map
