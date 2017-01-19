﻿class FacebookRegistration {

    constructor() {

        FB.getLoginStatus(response => {
            if (response.status === 'connected') {
                FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken);
                Site.RedirectToDashboard();
            }
        });

        $("#btn-login-with-facebook").click(() => {
            FacebookMemuchoUser.LoginOrRegister();
        });
    }
}

$(() => { new Facebook(() => { new FacebookRegistration() }); });
