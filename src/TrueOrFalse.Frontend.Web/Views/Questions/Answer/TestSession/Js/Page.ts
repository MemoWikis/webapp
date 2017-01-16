$(function() {

    let tsr = new TestSessionResult();
    $(document).ready(tsr.PositionIndicatorAverageText);
    $(window).resize(tsr.PositionIndicatorAverageText);

    new Pin(PinRowType.Set);
    $("#divCallForRegistration")
        .delay(1500)
        .fadeIn()
        .animate({ opacity: 1 }, 1500);

    setInterval(() => {
            $(".shakeInInterval").removeClass("tada animated");
            window.setTimeout(() => {
                    $(".shakeInInterval").addClass("tada animated");
                },
                1000);
        },
        10000);
});
