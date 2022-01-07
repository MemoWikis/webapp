class FacebookMemuchoUser {

    static Exists(facebookId: string): boolean {

        var doesExist = false;

        $.ajax({
            type: 'POST',
            async: false,
            cache: false,
            data: { facebookId: facebookId },
            url: "/Api/FacebookUsers/UserExists",
            error(error) { console.log(error); },
            success(result) {
                doesExist = result;
            }
        });

        return doesExist;
    }

    static Throw_if_not_exists(facebookId: string): boolean {

        if (!this.Exists(facebookId)) {
            throw new Error("user with facebookId '" + facebookId + "' does not exist");
        }
        return false;
    }

    static CreateAndLogin(user: FacebookUserFields, facebookAccessToken: string) {
        Utils.ShowSpinner();
        $.ajax({
            type: 'POST',
            async: false,
            cache: false,
            data: { facebookUser: user },
            url: "/Api/FacebookUsers/CreateAndLogin/",
            error(error) {
                Utils.HideSpinner();
                Rollbar.error("Something went wrong", error);
            },
            success(result) {
                Utils.HideSpinner();

                if (result.Success == true)
                    Site.RedirectToRegistrationSuccess();
                else {
                    Facebook.RevokeUserAuthorization(user.id, facebookAccessToken);
                    if (result.EmailAlreadyInUse == true) {
                        Alerts.showError({
                            text: messages.error.user.emailInUse
                        });
                    }
                }
            }
        });
    }

    static Login(facebookId: string, facebookAccessToken, stayOnPage: boolean = true) {

        FacebookMemuchoUser.Throw_if_not_exists(facebookId);

        $.ajax({
            type: 'POST',
            async: false,
            cache: false,
            data: { facebookUserId: facebookId, facebookAccessToken: facebookAccessToken },
            url: "/Api/FacebookUsers/Login/",
            error(error) { throw error },
            success() {
                window.location.reload();
            }
        });

    }

    static LoginOrRegister(stayOnPage = false, disallowRegistration = false) {
        FB.getLoginStatus(response => {
            this.LoginOrRegister_(response, stayOnPage, disallowRegistration);
        });
    }

    private static LoginOrRegister_(
        response: FB.LoginStatusResponse,
        stayOnPage = false,
        disallowRegistration = false) {

        if (response.status === 'connected') {

            FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken, stayOnPage);
            Site.ReloadPage_butNotTo_Logout();

        } else if (response.status === 'not_authorized' || response.status === 'unknown') {

            FB.login(response => {

                    var facebookId = response.authResponse.userID;
                    var facebookAccessToken = response.authResponse.accessToken;

                    if (response.status !== "connected")
                        return;

                    if (FacebookMemuchoUser.Exists(facebookId)) {
                        FacebookMemuchoUser.Login(facebookId, facebookAccessToken, stayOnPage);

                        return;
                    }

                    if (disallowRegistration) {
                        Site.RedirectToRegistration();
                        return;
                    }

                    Facebook.GetUser(facebookId,
                        facebookAccessToken,
                        (user: FacebookUserFields) => {
                            FacebookMemuchoUser.CreateAndLogin(user, facebookAccessToken);
                        });

                },
                { scope: 'email' });
        }

    }

    static Logout(onLogout: () => void) {
        FB.getLoginStatus(response => {
            if (response.status === 'connected') {
                FB.logout(responseLogout => {
                    onLogout();
                });
            } else {
                onLogout();
            }
        });
    }
}