class BetaRequest {
    constructor() {
        $("#btnBetaRequest").click(e => {
            window.alert("sdfsdf");
            this.SendBetaRequest(e);
        });

        $("#txtEmailRequester").keypress(e => {
            if (e.which === 13) {
                this.SendBetaRequest(e);
            }
        });                
    }

    SendBetaRequest(e) {
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
    }
}


$(() => { new BetaRequest() });