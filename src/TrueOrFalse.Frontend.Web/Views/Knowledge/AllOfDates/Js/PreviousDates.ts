class PreviousDates {
    constructor() {
        $("#btnShowPreviousDates").click(function (e) {
            e.preventDefault();
            $("#divShowPreviousDates").hide();
            $("#captionPreviousDate").show();

            $.get("/Dates/RenderPreviousDates",
                htmlResult => {
                    $("#previousDates")
                        .empty()
                        .animate({ opacity: 0.00 }, 0)
                        .append(htmlResult)
                        .animate({ opacity: 1.00 }, 600);

                    InitTooltips();
                });
        });
        $("#allDateRows").on("click", "#btnHidePreviousDates", function (e) {
            e.preventDefault();
            $("#previousDates")
                .animate({ opacitiy: 0.00 }, 900)
                .empty();
            $("#divShowPreviousDates").show();
        });
    }
} 