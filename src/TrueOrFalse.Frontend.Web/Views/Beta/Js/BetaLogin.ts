class BetaLogin
{
    constructor() {
        $("#btnEnter").click(e => {
            this.Login(e);
        });

        $("#txtBetaCode").keypress(e => {
            if (e.which === 13) {
                this.Login(e);
            }
        });        
    }    

    Login(e) {
        e.preventDefault();

        $.post("/Beta/IsValidBetaUser", { betacode: $("#txtBetaCode").val() }, (data) => {
            if (data.IsValid) {
                window.location.href = "/";
            } else {
                $("#msgInvalidBetaCode").fadeIn(500);
            }
        });        
    }
}

$(() => {
    new BetaLogin();
});