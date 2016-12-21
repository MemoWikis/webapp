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

    static GetUser(facebookId: string, accessToken: string, continuation: (user: any)=> void){
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
        FB.api("/me/permissions", "DELETE", function (response) {
            console.log(response); //gives true on app delete success 
        });
    }
}

class FacebookMemuchoUser {

    static Exists(facebookId: string): boolean {

        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { facebookId: facebookId },
            url: "/Api/Users/FacebookUserExists",
            error(error) { console.log(error); },
            success(result) {
                return (result == 'true');
            }
        });

        return false;
    }

    static Throw_if_not_exists(facebookId: string): boolean {

        if (!this.Exists(facebookId)) {
            throw new Error("user with facebookId '" + facebookId + "' does not exist");
        }
        return false;
    }

    static CreateAndLogin(user : any, facebookAccessToken : string) {
        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { user: user },
            url: "/Api/Users/CreateAndLogin/",
            error(error) { throw error },
            success(result) {
                if (result.Data.Success == "false") {

                    //

                    var reason = result.EmailAlreadyInUse == "true" ? "Die Email-Adresse ist bereits in Verwendung" : "";
                    alert("Die Registrierung konnte nicht abgeschlossen werden." + reason)
                }
            } 
        });
    }

    static Login(facebookId: string) {
        $.ajax({
            type: 'POST', async: false, cache: false,
            data: { facebookId: facebookId },
            url: "/Api/Users/Login/",
            error(error) { throw error }
        });
    }
}