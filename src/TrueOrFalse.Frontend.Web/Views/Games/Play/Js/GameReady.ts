class GameReady
{
    constructor() {
        this.InitCountDown();
    }

    public InitCountDown() {
        $('[data-willStartIn]').each(function () {

            var $this = $(this), finalDate = $(this).attr('data-willStartIn');

            window.console.log(finalDate);

            $this.countdown(finalDate, event => {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    }    
} 