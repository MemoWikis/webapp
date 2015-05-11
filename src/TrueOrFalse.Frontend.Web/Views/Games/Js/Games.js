var Games = (function () {
    function Games() {
        $('[data-countdown]').each(function () {
            var $this = $(this), finalDate = $(this).data('countdown');
            $this.countdown(finalDate, function (event) {
                $this.html(event.strftime('%-Mm %Ss'));
            });
        });
    }
    return Games;
})();

$(function () {
    new Games();
});
//# sourceMappingURL=Games.js.map
