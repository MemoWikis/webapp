﻿
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
        });
    }

    static HideFeatureInfo() {
        $("#needs-to-be-logged-in").hide();
    }

    static ShowFeatureInfo() {
        $("#needs-to-be-logged-in").show();
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
                Site.LoadValidPage();
        });
    }

    private static ShowErrorMsg(message: string) {
        $("#rowLoginMessage").show();
        $("#rowLoginMessage div").text(message);
    }

    public static onSignIn() {
        console.log("FB_Login_Successful");
        location.reload();
    }
}

$(() => {
    var login = new Login();
});
