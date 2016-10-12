class GameReady
{
    constructor() {
        this.InitCountDown();
    }

    public InitCountDown() {
        $('[data-remainingSeconds]').each(function () {

            var $this = $(this);
            var remainingSeconds = +$(this).attr('data-remainingSeconds');

            var finalDate = new Date();
            finalDate.setSeconds(finalDate.getSeconds() + remainingSeconds);

            $this.countdown(finalDate, event => {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    }
} 