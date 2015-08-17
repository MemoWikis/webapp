class SiteMessages
{
    constructor() {
        SiteMessages.Init();
    }

    static ShowStartBox() {
        $.get("/messages/gameInfo",
            htmlResult => {
                $("#IsInGameMessage")
                    .empty()
                    .animate({ opacity: 0.00 }, 0)
                    .append(htmlResult)
                    .animate({ opacity: 1.00 }, 600)
                    .after(function () { SiteMessages.Init()});
            }
        );
    }

    static Init() {

        var countDowns = $('#divMsgPartOfGame [data-countdown-game]');

        if (countDowns.length > 0) {
            GameHub.OnChangeStartTime((changeStartTime: ChangeStartTimeEvent) => {
                countDowns.countdown(changeStartTime.WillStartAt.toString());
            });            
        }

        countDowns.each(function () {
            var $this = $(this), finalDate = $(this).data('countdown-game');

            $this.countdown(finalDate, event => {

                var dateStartMinus60Secs = new Date(event.finalDate);
                dateStartMinus60Secs.setSeconds(event.finalDate.getSeconds() - 60);

                var dateStartMinus15Secs = new Date(event.finalDate);
                dateStartMinus15Secs.setSeconds(event.finalDate.getSeconds() - 15);

                var dateStartMinus10Secs = new Date(event.finalDate);
                dateStartMinus10Secs.setSeconds(event.finalDate.getSeconds() - 10);

                var divMsg = $("#divMsgPartOfGame");
                var currentTime = new Date();

                if (currentTime > event.finalDate) {
                    $(event.target).parent().hide();
                } else if (currentTime > dateStartMinus10Secs) {
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
    }

    static StopAndHide() {
        $('#divMsgPartOfGame [data-countdown-game]').countdown('stop');
        $("#divMsgPartOfGame").fadeOut(600);
    }
}


$(() => {
    new SiteMessages();
});