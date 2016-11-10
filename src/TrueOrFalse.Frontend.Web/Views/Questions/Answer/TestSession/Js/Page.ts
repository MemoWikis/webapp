$(function() {

    let tsr = new TestSessionResult();
    $(document).ready(tsr.PositionIndicatorAverageText);
    $(window).resize(tsr.PositionIndicatorAverageText);

    new Pin(PinRowType.Set);
});
