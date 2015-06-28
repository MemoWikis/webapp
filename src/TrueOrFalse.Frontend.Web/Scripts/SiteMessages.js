var SiteMessages = (function () {
    function SiteMessages() {
        SiteMessages.Init();
    }
    SiteMessages.ShowStartBox = function () {
        $.get("/messages/gameInfo", function (htmlResult) {
            $("#IsInGameMessage").empty().animate({ opacity: 0.00 }, 0).append(htmlResult).animate({ opacity: 1.00 }, 600).after(function () {
                SiteMessages.Init();
            });
        });
    };

    SiteMessages.Init = function () {
        window.console.log("init");
        $('[data-countdown-game]').each(function () {
            var $this = $(this), finalDate = $(this).data('countdown-game');

            window.console.log("huhu");

            $this.countdown(finalDate, function (event) {
                var dateStartMinus60Secs = new Date(event.finalDate);
                dateStartMinus60Secs.setSeconds(event.finalDate.getSeconds() - 60);

                var dateStartMinus15Secs = new Date(event.finalDate);
                dateStartMinus15Secs.setSeconds(event.finalDate.getSeconds() - 15);

                var dateStartMinus3Secs = new Date(event.finalDate);
                dateStartMinus3Secs.setSeconds(event.finalDate.getSeconds() - 10);

                var divMsg = $("#divMsgPartOfGame");
                var currentTime = new Date();

                if (currentTime > event.finalDate) {
                    $(event.target).parent().hide();
                } else if (currentTime > dateStartMinus3Secs) {
                    var url = $(event.target).data("game-url");
                    if (!window.location.href.match(new RegExp(url + "$", 'g')))
                        window.location.href = url;
                } else if (currentTime > dateStartMinus15Secs) {
                    Utils.Hightlight(divMsg);
                } else if (currentTime > dateStartMinus60Secs) {
                    divMsg.removeClass("alert-success").addClass("alert-warning");
                    if (currentTime == dateStartMinus60Secs) {
                        Utils.Hightlight(divMsg);
                    }
                }

                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    };

    SiteMessages.Hide = function () {
        $("#divMsgPartOfGame").fadeOut(600);
    };
    return SiteMessages;
})();

$(function () {
    new SiteMessages();
});
//# sourceMappingURL=SiteMessages.js.map
