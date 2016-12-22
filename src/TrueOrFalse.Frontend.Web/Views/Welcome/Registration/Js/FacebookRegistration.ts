class FacebookRegistration {

    constructor() {

        FB.getLoginStatus(response => {

            if (response.status === 'connected') {
                $("#btn-logout-from-facebook").show();

                FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken);
                Site.RedirectToDashboard();
            }

            $("#btn-logout-from-facebook").hide();
        });

        $("#btn-login-with-facebook").click(() => {

            FB.getLoginStatus(response => {

                var facebookId = response.authResponse.userID;
                var facebookAccessToken = response.authResponse.accessToken;

                if (response.status === 'connected') {

                    FacebookMemuchoUser.Login(response.authResponse.userID, facebookAccessToken);
                    Site.RedirectToDashboard();

                } else if (response.status === 'not_authorized' || response.status === 'unknown') {

                    FB.login(response => {

                        if (response.status !== "connected")
                            return;

                        if (FacebookMemuchoUser.Exists(facebookId)) {
                            FacebookMemuchoUser.Login(facebookId, facebookAccessToken);
                            Site.RedirectToDashboard();
                            return;
                        }

                        Facebook.GetUser(facebookId, facebookAccessToken, (user: FacebookUserFields) => {
                            if (FacebookMemuchoUser.CreateAndLogin(user, facebookAccessToken)) {
                                Site.RedirectToRegisterSuccess();
                            } else {
                                alert("Leider ist ein Fehler ist aufgetreten.");
                            }
                        });

                    }, { scope: 'email' });
                }
            });
        });

        $("#btn-logout-from-facebook").click(() => {
            FB.getLoginStatus(response => {
                if (response.status === 'connected') {
                    FB.logout(responseLogout => {
                        console.log(responseLogout);
                    });
                }
            });
        });
    }
}

new Facebook(() => {new FacebookRegistration()});