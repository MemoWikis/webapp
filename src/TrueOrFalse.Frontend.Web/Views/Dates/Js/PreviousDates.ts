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
    }
} 