var BetaRequest = (function () {
    function BetaRequest() {
        var _this = this;
        $("#btnBetaRequest").click(function (e) {
            _this.SendBetaRequest(e);
        });

        $("#txtEmailRequester").keypress(function (e) {
            if (e.which === 13) {
                _this.SendBetaRequest(e);
            }
        });
    }
    BetaRequest.prototype.SendBetaRequest = function (e) {
        e.preventDefault();

        $("#msgInvalidEmail").hide();
        $("#msgEmailSend").hide();

        var requesterEmail = $("#txtEmailRequester").val();

        if (!Validations.IsEmail(requesterEmail)) {
            $("#msgInvalidEmail").fadeIn(500);
            return;
        }

        $.post("/Beta/SendBetaRequest", { "email": requesterEmail });
        $("#msgEmailSend").fadeIn(500);
    };
    return BetaRequest;
})();

$(function () {
    new BetaRequest();
});
//# sourceMappingURL=BetaRequest.js.map
