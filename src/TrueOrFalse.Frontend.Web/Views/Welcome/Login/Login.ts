class Login {
    constructor() {
        $("[data-btn-login=true]").click(this.LoginClick);
    }

    private LoginClick(e) {
        e.preventDefault();

        $.post("/Login/LoginModal", (modal) => {
            $("#modalLogin").remove();
            $("#modalLoginContainer").append(modal);
            $("#modalLogin").modal('show');
        });
    }
}

$(() => {
    var login = new Login();
});
