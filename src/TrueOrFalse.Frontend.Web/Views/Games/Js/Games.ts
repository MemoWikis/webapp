class Games {
    constructor() {
        $('[data-countdown]').each(function() {
            var $this = $(this), finalDate = $(this).data('countdown');
            $this.countdown(finalDate, event => {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });        
    }
}

$(() => {
    new Games();
}); 