class SendRequestNewsletter {
    constructor() {
        $("#btnNewsletterRequest").click(e => {
            this.SendNewsletterRequest(e);
        });

        $("#txtNewsletterRequesterEmail").keypress(e => {
            if (e.which === 13) {
                this.SendNewsletterRequest(e);
            }
        });
    }

    SendNewsletterRequest(e) {
        e.preventDefault();

        $("#msgInvalidEmail").hide();
        $("#msgEmailSend").hide();

        var requesterEmail = $("#txtNewsletterRequesterEmail").val();

        if (!Validations.IsEmail(requesterEmail)) {
            $("#msgInvalidEmail").fadeIn(500);
            return;
        }

        $.post("/Welcome/SendNewsletterRequest", { "requesterEmail": requesterEmail });
        $("#msgEmailSend").fadeIn(500);
    }
    
}