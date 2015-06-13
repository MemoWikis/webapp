var SiteMessages = (function () {
    function SiteMessages() {
        $('[data-countdown-game]').each(function () {
            var $this = $(this), finalDate = $(this).data('countdown-game');

            $this.countdown(finalDate, function (event) {
                var dateStartMinus60Secs = new Date(event.finalDate);
                dateStartMinus60Secs.setSeconds(event.finalDate.getSeconds() - 60);

                var dateStartMinus10Secs = new Date(event.finalDate);
                dateStartMinus10Secs.setSeconds(event.finalDate.getSeconds() - 10);

                var dateStartMinus3Secs = new Date(event.finalDate);
                dateStartMinus3Secs.setSeconds(event.finalDate.getSeconds() - 3);

                var divMsg = $("#divMsgPartOfGame");
                var currentTime = new Date();

                if (currentTime > event.finalDate) {
                    $(event.target).parent().hide();
                } else if (currentTime > dateStartMinus3Secs) {
                    var url = $(event.target).data("game-url");
                    if (!window.location.href.match(new RegExp(url + "$", 'g')))
                        window.location.href = url;
                } else if (currentTime > dateStartMinus10Secs) {
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
    }
    return SiteMessages;
})();

$(function () {
    new SiteMessages();
});
//# sourceMappingURL=SiteMessages.js.map
