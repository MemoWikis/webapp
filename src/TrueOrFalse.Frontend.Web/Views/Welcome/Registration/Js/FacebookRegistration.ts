class FacebookRegistration {

    constructor() {

        FB.getLoginStatus(response => {

            if (response.status === 'connected') {
                $("#btn-logout-from-facebook").show();

                FacebookMemuchoUser.Throw_if_not_exists(response.authResponse.userID);
                Site.RedirectToDashboard();
            }

            $("#btn-logout-from-facebook").hide();
        });

        $("#btn-login-with-facebook").click(() => {

            FB.getLoginStatus(response => {

                console.log(response);

                if (response.status === 'connected') {

                    FacebookMemuchoUser.Throw_if_not_exists(response.authResponse.userID);
                    Site.RedirectToDashboard();

                } else if (response.status === 'not_authorized' || response.status === 'unknown') {

                    FB.login(response => {

                        if (response.status !== "connected")
                            return;

                        var facebookId = response.authResponse.userID;
                        var facebookAccessToken = response.authResponse.accessToken;

                        if (FacebookMemuchoUser.Exists(facebookId)) {
                            FacebookMemuchoUser.Login(facebookId);
                            Site.RedirectToDashboard();
                            return;
                        }

                        Facebook.GetUser(facebookId, facebookAccessToken, (user) => {
                            FacebookMemuchoUser.CreateAndLogin(user, facebookAccessToken);
                            Site.RedirectToRegisterSuccess();                            
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