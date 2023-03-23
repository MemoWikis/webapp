import { AlertType, useAlertStore } from "../alert/alertStore"
import { useUserStore, CurrentUser } from "./userStore"

declare var window: any

interface LoginResult {
    success: boolean
    currentUser?: CurrentUser
}

export class Google {
    public static SignIn() {
        window.google.accounts.id.prompt()
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
                    callback: this.handleCredentialResponse
                });
                this.SignIn()
            }
            document.head.appendChild(gsiClientScript)
        }
    }

    public static async handleCredentialResponse(e: any) {

        var result = await $fetch<LoginResult>('/apiVue/Google/Login', {
            method: 'POST', body: { token: e.credential }, mode: 'cors', credentials: 'include', cache: 'no-cache'
        }).catch((error) => console.log(error.data))
        if (result?.success && result.currentUser) {
            const userStore = useUserStore()
            userStore.initUser(result.currentUser)
            // if (window.location.pathname == '/Registrieren')
            //     navigateTo('/')
        } else {
            const alertStore = useAlertStore()
            alertStore.openAlert(AlertType.Error, { text: "Fehler" })
        }
    }
}
