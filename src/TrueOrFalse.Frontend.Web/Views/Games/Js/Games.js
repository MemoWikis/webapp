var Games = (function () {
    function Games() {
        this.InitializeCountdown();
    }
    Games.prototype.InitializeCountdown = function () {
        $('[data-countdown]').each(function () {
            var $this = $(this), finalDate = $(this).data('countdown');
            $this.countdown(finalDate, function (event) {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    };
    return Games;
})();

$(function () {
    new Games();
});
//# sourceMappingURL=Games.js.map
