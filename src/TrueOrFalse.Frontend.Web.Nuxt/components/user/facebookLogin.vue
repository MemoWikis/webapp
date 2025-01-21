<script setup lang="ts">
import { FacebookMemoWikisUser } from './FacebookMemoWikisUser'

function login() {
    FacebookMemoWikisUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false)
}
const config = useRuntimeConfig()

function loadPlugin(toLogin = false) {
    const fbScriptElement = document.getElementById('facebook-jssdk')
    if (fbScriptElement == null) {

        window.fbAsyncInit = function () {
            FB.init({
                appId: config.public.facebookAppId,
                autoLogAppEvents: true,
                xfbml: true,
                version: 'v17.0'
            })
        };
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0]
            if (d.getElementById(id)) { return }
            js = d.createElement(s) as HTMLScriptElement
            js.id = id
            js.src = "//connect.facebook.net/de_DE/sdk.js"
            fjs.parentNode?.insertBefore(js, fjs)
        }(document, 'script', 'facebook-jssdk'))

        if (toLogin) {
            setTimeout(() => {
                FacebookMemoWikisUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false)
            }, 500)
        }
    }
}

function loadFbSdk(toLogin = false) {
    const fbsdkElement = document.getElementById('facebook-jssdk')
    if (fbsdkElement == null) {

        const fbsdkScript = document.createElement('script')
        fbsdkScript.setAttribute('id', 'facebook-jssdk')
        fbsdkScript.setAttribute('crossorigin', 'anonymous')
        fbsdkScript.src = '//connect.facebook.net/de_DE/sdk.js'
        fbsdkScript.onload = () => {
            window.fbAsyncInit = function () {
                FB.init({
                    appId: config.public.facebookAppId,
                    autoLogAppEvents: true,
                    xfbml: true,
                    version: 'v17.0'
                })
            }
            if (toLogin) {
                login()
            }
        }
        document.head.appendChild(fbsdkScript)
    }
}

defineExpose({
    loadPlugin,
    login,
    loadFbSdk
})

</script>