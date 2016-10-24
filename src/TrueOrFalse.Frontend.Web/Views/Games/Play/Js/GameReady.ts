class GameReady
{
    constructor() {
        this.InitCountDown();
    }

    public InitCountDown() {
        $('[data-remainingSeconds]').each(function () {

            var $this = $(this);
            var remainingSeconds = +$(this).attr('data-remainingSeconds');

            $this.countdown(SiteMessages.GetFinalDate(remainingSeconds), event => {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    }
} 