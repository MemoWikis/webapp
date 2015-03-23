$(function () {
    $("#btnEnter").click(function (e) {
        e.preventDefault();

        $.post("/Beta/IsValidBetaUser", { betacode: $("#txtBetaCode").val() }, function (data) {
            if (data.IsValid) {
                window.location.href = "/";
            } else {
                $("#msgInvalidBetaCode").fadeIn(500);
            }
        });
    });
});
//# sourceMappingURL=BetaLogin.js.map
