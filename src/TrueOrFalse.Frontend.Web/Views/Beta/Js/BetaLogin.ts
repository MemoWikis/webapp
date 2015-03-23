$(() => {

    function Login(e) {
        e.preventDefault();

        $.post("/Beta/IsValidBetaUser", { betacode: $("#txtBetaCode").val() }, (data) => {
            if (data.IsValid) {
                window.location.href = "/";
            } else {
                $("#msgInvalidBetaCode").fadeIn(500);
            }
        });        
    }

    $("#btnEnter").click(e => {
         Login(e);
    });

    $("#txtBetaCode").keypress(e => {
        if (e.which === 13) {
            Login(e);
        }
    });
});