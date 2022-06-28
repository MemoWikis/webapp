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

interface FacebookUserFields {
    id;
    email;
    name;
}