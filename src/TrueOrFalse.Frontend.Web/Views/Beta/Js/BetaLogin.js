$(function () {
    function Login(e) {
        e.preventDefault();

        $.post("/Beta/IsValidBetaUser", { betacode: $("#txtBetaCode").val() }, function (data) {
            if (data.IsValid) {
                window.location.href = "/";
            } else {
                $("#msgInvalidBetaCode").fadeIn(500);
            }
        });
    }

    $("#btnEnter").click(function (e) {
        Login(e);
    });

    $("#txtBetaCode").keypress(function (e) {
        if (e.which === 13) {
            Login(e);
        }
    });
});
//# sourceMappingURL=BetaLogin.js.map
