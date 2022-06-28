import { UserCreateResult } from './UserCreateResult';
import { useUtilsStore } from '../utils/utilsStore'
const utilsStore = useUtilsStore()

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
        utilsStore.showSpinner();

        var result = await $fetch<UserCreateResult>('/api/FacebookUsers/UserExists', { 
            method: 'POST', 
            body: { facebookUser: user }, 
            credentials: 'include', 
            cache: 'no-cache'})
            .catch((error) => {
                utilsStore.hideSpinner()
                Rollbar.error("Something went wrong", error.data)
            })

        if (!!result) {
            utilsStore.hideSpinner();

            if (result.Success)
                Site.LoadValidPage();
            else {
                Facebook.RevokeUserAuthorization(user.id, facebookAccessToken);
                if (result.EmailAlreadyInUse) {
                    Alerts.showError({
                        text: messages.error.user.emailInUse
                    });
                }
            }
        }
    }

    static async Login(facebookId: string, facebookAccessToken, stayOnPage: boolean = true) {

        FacebookMemuchoUser.Throw_if_not_exists(facebookId);

        utilsStore.showSpinner();

        var result = await $fetch<UserCreateResult>('/api/FacebookUsers/Login', { 
            method: 'POST', 
            body: { facebookUserId: facebookId, facebookAccessToken: facebookAccessToken }, 
            credentials: 'include', 
            cache: 'no-cache' })
            .catch((error) => {
                utilsStore.hideSpinner()
                Rollbar.error("Something went wrong", error.data)
            })

        if (!!result && result.Success)                    
            Site.LoadValidPage();

    }

    static LoginOrRegister(stayOnPage = false, disallowRegistration = false) {
        FB.getLoginStatus(response => {
            this.LoginOrRegister_(response, stayOnPage, disallowRegistration);
        });
    }

    private static LoginOrRegister_(
        response: FB.LoginStatusResponse,
        stayOnPage = false,
        disallowRegistration = false) {

        if (response.status === 'connected') {

            FacebookMemuchoUser.Login(response.authResponse.userID, response.authResponse.accessToken, stayOnPage);
            Site.LoadValidPage();

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
                        Site.RedirectToRegistration();
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
        });
    }
}