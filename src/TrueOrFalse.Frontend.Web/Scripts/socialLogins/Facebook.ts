var Rollbar: any;

class Facebook {
    constructor(onInit: () => void) {
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1789061994647406',
                xfbml: true,
                version: 'v2.8'
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/de_DE/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
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

interface FacebookUserFields {
    id;
    email;
    name;
}