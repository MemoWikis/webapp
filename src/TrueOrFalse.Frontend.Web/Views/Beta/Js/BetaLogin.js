var BetaLogin = (function () {
    function BetaLogin() {
        var _this = this;
        $("#btnEnter").click(function (e) {
            _this.Login(e);
        });

        $("#txtBetaCode").keypress(function (e) {
            if (e.which === 13) {
                _this.Login(e);
            }
        });
    }
    BetaLogin.prototype.Login = function (e) {
        e.preventDefault();

        $.post("/Beta/IsValidBetaUser", { betacode: $("#txtBetaCode").val() }, function (data) {
            if (data.IsValid) {
                window.location.href = "/";
            } else {
                $("#msgInvalidBetaCode").fadeIn(500);
            }
        });
    };
    return BetaLogin;
})();

$(function () {
    new BetaLogin();
});
//# sourceMappingURL=BetaLogin.js.map
