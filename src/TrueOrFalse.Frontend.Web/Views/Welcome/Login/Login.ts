class Login {

    constructor() {
        $("[data-btn-login=true]").click((e) => this.OpenModal(e));
    }

    private OpenModal(e) {

        Site.CloseAllModals();

        var self = this;

        e.preventDefault();

        $.post("/Login/LoginModal", (modal) => {
            $("#modalLogin").remove();
            $("#modalLoginContainer").append(modal);
            $("#modalLogin").modal('show');

            self.InitializeForm();
            self.InitializeFacebook();
        });
    }

    private InitializeFacebook() {

        $("#btn-login-with-facebook-modal").click(() => {
            FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);            
        });

        Google.AttachClickHandler('btn-login-with-google-modal');
    }

    private InitializeForm() {

        var validationRules = {
            rules: {
                EmailAddress: { required: true, email: true },
                Password: { required: true }
            }
        };

        var formSelector = "#LoginForm";
        fnValidateForm(formSelector, validationRules, false);

        $("#btnModalLogin").click((e) => {
            e.preventDefault();

            if ($(formSelector).valid()) {
                this.SubmitForm();
            }

        });
    }

    private SubmitForm() {

        var data = {
            EmailAddress: $("#EmailAddress").val(),
            Password: $("#Password").val(),
            PersistentLogin : $("#PersistentLogin").val()
        }

        $.post("/Login/Login", data, (result) => {

            if (!result.Success) {
                this.ShowErrorMsg(result.Message);
                return;
            }

            Site.ReloadPage_butNotTo_Logout();
        });
    } 

    private ShowErrorMsg(message : string) {
        $("#rowLoginMessage").show();
        $("#rowLoginMessage div").text(message);
    }
}

$(() => {
    var login = new Login();
});
