class FacebookRegistration {

    constructor() {

        FB.getLoginStatus(response => {
            if (response.status === 'connected') {
                FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken);
                Site.RedirectToDashboard();
            }
        });

        $("#btn-login-with-facebook").click(() => {

            FB.getLoginStatus(response => {

                if (response.status === 'connected') {

                    FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken);
                    Site.RedirectToDashboard();

                } else if (response.status === 'not_authorized' || response.status === 'unknown') {

                    FB.login(response => {

                        var facebookId = response.authResponse.userID;
                        var facebookAccessToken = response.authResponse.accessToken;

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
    }
}

new Facebook(() => {new FacebookRegistration()});