import { Site } from '../shared/site';
import { GoogleMemuchoUser } from './GoogleMemuchoUser'
import { nextTick } from 'vue'

export class Google {

    private static _auth2: gapi.auth2.GoogleAuth;

    constructor() {
        gapi.load('auth2', () => {
            Google._auth2 = this.InitApi();
        });
        

    }

    static async AttachClickHandler(selector : string) {
        
        await nextTick()
        var element = document.getElementById(selector);

        if (element == null)
            return;

        Google._auth2.attachClickHandler(element, {},
            googleUser => Google.OnLoginSuccess(googleUser),
            error => Google.OnLoginError(error)
        );        
    }

    private InitApi() {
        return gapi.auth2.init(({
            client_id: '290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com',
            cookiepolicy: 'single_host_origin',
        }) as any);
    }

    public static SignIn() {
        Google._auth2.signIn().then(
            (googleUser) => {
                Google.OnLoginSuccess(googleUser);
            },
            (error) => {
                Google.OnLoginError(error);
            });
     }

    private static OnLoginSuccess(googleUser : gapi.auth2.GoogleUser) {

        var googleId = googleUser.getBasicProfile().getId();
        var googleIdToken = googleUser.getAuthResponse().id_token;

        if (GoogleMemuchoUser.Exists(googleId)) {
            GoogleMemuchoUser.Login(googleId, googleIdToken);
            Site.loadValidPage();
            return;
        }

       GoogleMemuchoUser.CreateAndLogin(googleUser);
       Site.loadValidPage();
    }

    private static OnLoginError(error) {
        alert(JSON.stringify(error, undefined, 2));
    }

}
