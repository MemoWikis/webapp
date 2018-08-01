$(function () {
    //var tourInit = new TourInit();
    //new SendRequestNewsletter();
    new Pin(PinType.Category);

    $("[data-type=learn-wishknowledge]").click(function (e) {
        if (NotLoggedIn.Yes()) {
            e.preventDefault();
            NotLoggedIn.ShowErrorMsg("LearnWishKnowledge");
            return;
        }


    });
    new NumbersCountUp();
    $("#team").css("cursor", "pointer");
    $("#team").on('click', function (e) {
        e.preventDefault();
        window.open($("#hdd-team").val());

    });

});