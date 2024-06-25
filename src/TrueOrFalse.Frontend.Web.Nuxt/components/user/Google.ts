import { AlertType, messages, useAlertStore } from "../alert/alertStore"
import { useUserStore, CurrentUser } from "./userStore"

declare const window: any

export class Google {
    public static SignIn() {
        document.cookie = "g_state=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;"
        const config = useRuntimeConfig()

        window.google?.accounts.id.prompt((res: any) => {
            if (res.isNotDisplayed()) {
                window.google.accounts.oauth2.initTokenClient({
                    client_id: config.public.gsiClientKey,
                    itp_support: false,
                    scope: 'openid',
                    callback: this.handleCredentialResponse,
                })
            }
        });
    }

    public static loadGsiClient() {
        const gsiClientElement = document.getElementById('gsiClient')
        if (gsiClientElement == null) {
            const config = useRuntimeConfig()

            const gsiClientScript = document.createElement('script')
            gsiClientScript.setAttribute('id', 'gsiClient')
            gsiClientScript.src = 'https://accounts.google.com/gsi/client'
            gsiClientScript.onload = () => {
                window.google.accounts.id.initialize({
                    client_id: config.public.gsiClientKey,
                    itp_support: false,
                    scope: 'openid',
                    callback: this.handleCredentialResponse
                });
                this.SignIn()
            }
            document.head.appendChild(gsiClientScript)
        }
    }

    public static async handleCredentialResponse(e: any) {

        const result = await $fetch<FetchResult<CurrentUser>>('/apiVue/Google/Login', {
            method: 'POST', body: { token: e.credential }, mode: 'cors', credentials: 'include', cache: 'no-cache'
        }).catch((error) => console.log(error.data))
        if (result && 'success' in result && result.success === true) {
            const userStore = useUserStore()
            userStore.initUser(result.data)
            userStore.apiLogin(userStore.isLoggedIn)
        } else if (result && 'success' in result && result.success === false) {
            const alertStore = useAlertStore()
            alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) ?? messages.error.default })
        }
    }
}
if (process.client)
    window.handleGoogleCredentialResponse = Google.handleCredentialResponse