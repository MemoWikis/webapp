var GameReady = (function () {
    function GameReady() {
        this.InitCountDown();
    }
    GameReady.prototype.InitCountDown = function () {
        window.console.log("init");
        $('[data-willStartIn]').each(function () {
            var $this = $(this), finalDate = $(this).attr('data-willStartIn');

            window.console.log(finalDate);

            $this.countdown(finalDate, function (event) {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    };
    return GameReady;
})();
//# sourceMappingURL=GameReady.js.map
