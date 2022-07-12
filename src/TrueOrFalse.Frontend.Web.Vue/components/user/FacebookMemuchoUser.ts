import { UserCreateResult } from './UserCreateResult'
import { Site } from '../utils/site'
import { useSpinnerStore } from '../spinner/spinnerStore'
import { useAlertStore, AlertType, messages } from '../alert/alertStore'
import { Facebook, FacebookUserFields } from './Facebook'
import { useUserStore } from '../user/userStore'

const spinnerStore = useSpinnerStore()
const alertStore = useAlertStore()
const userStore = useUserStore()

export class FacebookMemuchoUser {

    static async Exists(facebookId: string): Promise<boolean> {

        var doesExist = await $fetch<boolean>('/api/FacebookUsers/UserExists', { method: 'POST', body: { facebookId: facebookId }, credentials: 'include', cache: 'no-cache' 
        }).catch((error) => console.log(error.data))

        return !!doesExist;
    }

    static Throw_if_not_exists(facebookId: string): boolean {

        if (!this.Exists(facebookId)) {
            throw new Error("user with facebookId '" + facebookId + "' does not exist");
        }
        return false;
    }

    static async CreateAndLogin(user: FacebookUserFields, facebookAccessToken: string) {
        spinnerStore.showSpinner();

        var result = await $fetch<UserCreateResult>('/api/FacebookUsers/UserExists', { 
            method: 'POST', 
            body: { facebookUser: user }, 
            credentials: 'include', 
            cache: 'no-cache'})
            .catch((error) => {
                spinnerStore.hideSpinner()
                // Rollbar.error("Something went wrong", error.data)
            })

        if (!!result) {
            spinnerStore.hideSpinner();

            if (result.Success){
                userStore.isLoggedIn = true
                Site.loadValidPage();
            }
            else {
                Facebook.RevokeUserAuthorization(user.id, facebookAccessToken);
                if (result.EmailAlreadyInUse) {
                    alertStore.openAlert(AlertType.Error, {
                        text: messages.error.user.emailInUse
                    })
                }
            }
        }
    }

    static async Login(facebookId: string, facebookAccessToken, stayOnPage: boolean = true) {

        FacebookMemuchoUser.Throw_if_not_exists(facebookId);

        spinnerStore.showSpinner();

        var result = await $fetch<UserCreateResult>('/api/FacebookUsers/Login', { 
            method: 'POST', 
            body: { facebookUserId: facebookId, facebookAccessToken: facebookAccessToken }, 
            credentials: 'include', 
            cache: 'no-cache' })
            .catch((error) => {
                spinnerStore.hideSpinner()
                // Rollbar.error("Something went wrong", error.data)
            })

        if (!!result && result.Success){
            userStore.isLoggedIn = true
            Site.loadValidPage();
        }

    }

    static LoginOrRegister(stayOnPage = false, disallowRegistration = false) {
        FB.getLoginStatus(response => {
            this._LoginOrRegister(response, stayOnPage, disallowRegistration);
        });
    }

    private static _LoginOrRegister(
        response: FB.LoginStatusResponse,
        stayOnPage = false,
        disallowRegistration = false) {

        if (response.status === 'connected') {

            FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken, stayOnPage);
            Site.loadValidPage();

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
                        Site.redirectToRegistration();
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
            userStore.isLoggedIn = false
        });
    }
}