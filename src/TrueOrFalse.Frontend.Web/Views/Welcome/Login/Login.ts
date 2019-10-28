class Login {

    private static IsClicked = false;
    constructor() {
        $("[data-btn-login=true], [data-btn-login=True] ").click(
            (e) => {
                Login.HideFeatureInfo();
                Login.OpenModal(e);
            });
    }

    static OpenModal(e: JQueryEventObject = null, afterLoad: () => void = null) {

        Site.CloseAllModals();

        var self = this;

        if (e != null)
            e.preventDefault();

        $.post("/Login/LoginModal", (modal) => {
            $("#modalLogin").remove();
            $("#modalLoginContainer").append(modal);
            $('#modalLogin').on('shown.bs.modal',
                e => {
                    $("#EmailAddress").focus();
                });

            $("#modalLogin").modal('show');

            if (afterLoad != null)
                afterLoad();

            self.InitializeForm();
            self.InitializeFacebook();
            self.InitializeGoogleLogin();
        });
    }

    static HideFeatureInfo() {
        $("#needs-to-be-logged-in").hide();
    }

    static ShowFeatureInfo() {
        $("#needs-to-be-logged-in").show();
    }

    private static InitializeFacebook() {

        $("#btn-login-with-facebook-modal").click(() => {
            FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);
        });
    }

    private static InitializeGoogleLogin() {
        $("#btn-login-with-google-modal").on("click", () => {
            new Google();
                setTimeout(() => {
                        Google.AttachClickHandler('btn-login-with-google-modal');
                    },
                    500);

            if (!this.IsClicked) {
                setTimeout(() => {
                        this.IsClicked = true;
                        $("#btn-login-with-google-modal")[0].click();
                    },
                    1000);
            }
        });
    }

    private static InitializeForm() {

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

    private static SubmitForm() {

        var data = {
            EmailAddress: $("#EmailAddress").val(),
            Password: $("#Password").val(),
            PersistentLogin: $("#PersistentLogin").val()
        }

        $.post("/Login/Login", data, (result) => {

            if (!result.Success) {
                this.ShowErrorMsg(result.Message);
                return;
            }

            var backToLocation = Utils.GetQueryString().backTo;
            if (backToLocation != undefined)
                location.href = backToLocation;
            else
                Site.ReloadPage_butNotTo_Logout();
        });
    }

    private static ShowErrorMsg(message: string) {
        $("#rowLoginMessage").show();
        $("#rowLoginMessage div").text(message);
    }
}

$(() => {
    var login = new Login();
});
