var Rollbar: any;

class Facebook {
    constructor(onInit: () => void) {
        window.fbAsyncInit = () => {
            FB.init({
                appId: '1789061994647406',
                cookie: true,  // enable cookies to allow the server to access 
                // the session
                xfbml: true,  // parse social plugins on this page
                version: 'v2.8' // use graph api version 2.8
            });

            onInit();
        };

        // Load the Facebook-SDK asynchronously
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));
    }

    static GetUser(facebookId: string, accessToken: string, continuation: (user: FacebookUserFields)=> void){
        FB.api(
            "/" + facebookId,
            { access_token: accessToken, fields: 'name, email' },
            response => {
                if (response && !response.error) {
                    console.log(response);
                    continuation(response);
                } else {    
                    throw (response);
                }
            });
    }

    static RevokeUserAuthorization(facebookId: string, accessToken: string) {
        FB.api("/me/permissions", "DELETE", response => {
            console.log(response); //return true on "app delete" success 
        });
    }
}

class FacebookMemuchoUser {

    static Exists(facebookId: string): boolean {

        var doesExist = false;

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { facebookId: facebookId },
            url: "/Api/Users/FacebookUserExists",
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

        var success = false;

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { facebookUserCreateParameter: user },
            url: "/Api/Users/CreateAndLogin/",
            error(error) {
                Rollbar.error("Something went wrong", error);
                success = false;
            },
            success(result) {

                if (result.Success == "false") {

                    Facebook.RevokeUserAuthorization(user.id, facebookAccessToken);

                    var reason = result.EmailAlreadyInUse == "true" ? "Die Email-Adresse ist bereits in Verwendung" : "";
                    alert("Die Registrierung konnte nicht abgeschlossen werden." + reason);

                    success = false;
                }

                success = true;
            } 
        });

        return success;
    }

    static Login(facebookId: string, facebookAccessToken) {

        FacebookMemuchoUser.Throw_if_not_exists(facebookId);

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { facebookUserId: facebookId, facebookAccessToken: facebookAccessToken },
            url: "/Api/Users/Login/",
            error(error) { throw error },
        });
    }

    static Logout(onLogout : () => void) {
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

interface FacebookUserFields {
    id;
    email;
    name;
}