class PreviousDates {
    constructor() {
        $("#btnShowPreviousDates").click(function () {

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
        $("#allDateRows").on("click", "#btnHidePreviousDates", function () {

            $("#previousDates")
                .animate({ opacitiy: 0.00 }, 900)
                .empty();
            $("#divShowPreviousDates").show();
        });
    }
} 