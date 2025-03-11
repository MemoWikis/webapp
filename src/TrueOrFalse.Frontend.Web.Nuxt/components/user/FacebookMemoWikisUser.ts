import { useLoadingStore } from "../loading/loadingStore"
import { useAlertStore, AlertType } from "../alert/alertStore"
import { Facebook, FacebookUserFields } from "./Facebook"
import { useUserStore, CurrentUser } from "../user/userStore"

export class FacebookMemoWikisUser {
    static async Exists(facebookId: string): Promise<boolean> {
        const doesExist = await $api<boolean>(
            "/apiVue/FacebookUsers/UserExists",
            {
                method: "GET",
                body: { facebookId: facebookId },
                credentials: "include",
                cache: "no-cache",
            }
        ).catch((error) => console.log(error.data))

        return !!doesExist
    }

    static Throw_if_not_exists(facebookId: string): boolean {
        if (!this.Exists(facebookId)) {
            throw new Error(
                "user with facebookId '" + facebookId + "' does not exist"
            )
        }
        return false
    }

    static async CreateAndLogin(
        user: FacebookUserFields,
        facebookAccessToken: string
    ) {
        const loadingStore = useLoadingStore()

        loadingStore.startLoading()

        const nuxtApp = useNuxtApp()
        const { $i18n } = nuxtApp

        const result = await $api<FetchResult<CurrentUser>>(
            "/apiVue/FacebookUsers/CreateAndLogin",
            {
                method: "POST",
                body: {
                    facebookUser: user,
                    facebookAccessToken,
                    language: $i18n.locale.value,
                },
                credentials: "include",
                cache: "no-cache",
            }
        ).catch((error) => {
            loadingStore.stopLoading()
            // Rollbar.error("Something went wrong", error.data)
        })

        loadingStore.stopLoading()

        if (result && "success" in result && result?.success == true) {
            const userStore = useUserStore()
            userStore.initUser(result.data)
            userStore.apiLogin(userStore.isLoggedIn)
        } else if (result && "success" in result && result?.success == false) {
            Facebook.RevokeUserAuthorization(user.id, facebookAccessToken)
            const alertStore = useAlertStore()
            const { t } = useI18n()
            alertStore.openAlert(AlertType.Error, {
                text: t(result.messageKey),
            })
        }
    }

    static async Login(
        facebookId: string,
        facebookAccessToken: string,
        stayOnPage: boolean = true
    ) {
        const loadingStore = useLoadingStore()

        FacebookMemoWikisUser.Throw_if_not_exists(facebookId)
        loadingStore.startLoading()

        const result = await $api<FetchResult<CurrentUser>>(
            "/apiVue/FacebookUsers/Login",
            {
                method: "POST",
                body: {
                    facebookUserId: facebookId,
                    facebookAccessToken: facebookAccessToken,
                },
                credentials: "include",
                cache: "no-cache",
            }
        ).catch((error) => {
            loadingStore.stopLoading()
            // Rollbar.error("Something went wrong", error.data)
        })

        loadingStore.stopLoading()
        if (result && "success" in result && result?.success == true) {
            const userStore = useUserStore()
            userStore.initUser(result.data)
            userStore.apiLogin(userStore.isLoggedIn)
        } else if (result && "success" in result && result?.success == false) {
            const alertStore = useAlertStore()
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp
            alertStore.openAlert(AlertType.Error, {
                text: $i18n.t(result.messageKey),
            })
        }
    }

    static LoginOrRegister(stayOnPage = false, disallowRegistration = false) {
        FB.getLoginStatus((response) => {
            this._LoginOrRegister(response, stayOnPage, disallowRegistration)
        })
    }

    private static _LoginOrRegister(
        response: FB.LoginStatusResponse,
        stayOnPage = false,
        disallowRegistration = false
    ) {
        if (response.status === "connected") {
            FacebookMemoWikisUser.Login(
                response.authResponse!.userID,
                response.authResponse!.accessToken,
                stayOnPage
            )
        } else if (
            response.status === "not_authorized" ||
            response.status === "unknown"
        ) {
            FB.login(
                (response) => {
                    const facebookId = response.authResponse!.userID
                    const facebookAccessToken =
                        response.authResponse!.accessToken

                    if (response.status !== "connected") return

                    if (disallowRegistration) {
                        navigateTo("/Registrieren")
                        return
                    }

                    this.handleResponse(
                        facebookId,
                        facebookAccessToken,
                        stayOnPage
                    )
                },
                { scope: "email" }
            )
        }
    }

    private static async handleResponse(
        facebookId: string,
        facebookAccessToken: string,
        stayOnPage: boolean
    ) {
        if (await FacebookMemoWikisUser.Exists(facebookId)) {
            FacebookMemoWikisUser.Login(
                facebookId,
                facebookAccessToken,
                stayOnPage
            )
            return
        } else {
            Facebook.GetUser(
                facebookId,
                facebookAccessToken,
                (user: FacebookUserFields) => {
                    FacebookMemoWikisUser.CreateAndLogin(
                        user,
                        facebookAccessToken
                    )
                }
            )
        }
    }

    static Logout(onLogout: () => void) {
        FB.getLoginStatus((response) => {
            if (response.status === "connected") {
                FB.logout((responseLogout) => {
                    onLogout()
                })
            } else {
                onLogout()
            }
            const userStore = useUserStore()
            userStore.reset()
        })
    }
}
