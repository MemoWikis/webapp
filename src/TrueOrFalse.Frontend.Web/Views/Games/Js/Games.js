var Games = (function () {
    function Games() {
        this.InitializeCountdown();
        this.InitializeButtons();

        this._hub = $.connection.playHub;

        if (this._hub == null)
            return;

        this._hub.client.JoinedGame = function (customer) {
            window.console.log(customer);
        };

        $.connection.hub.start(function () {
            window.console.log("connection started:");
        });
    }
    Games.prototype.InitializeButtons = function () {
        var me = this;

        $("[data-joinGameId]").click(function (e) {
            e.preventDefault();
            var gameId = $(this).attr("data-joinGameId");

            window.alert(gameId);
            me._hub.server.joinGame(gameId).done(function () {
                //window.alert("success");
            }).fail(function (error) {
                //window.alert(error);
            });
        });
    };

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

var Player = (function () {
    function Player() {
    }
    return Player;
})();

$(function () {
    new Games();
});
//# sourceMappingURL=Games.js.map
