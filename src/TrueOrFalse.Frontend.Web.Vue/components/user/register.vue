<script lang="ts" setup>
import { Google } from '~~/components/user/Google'
import { FacebookMemuchoUser } from '~~/components/user/FacebookMemuchoUser'
import { AlertType, useAlertStore, messages } from '~~/components/alert/alertStore'
import { useUserStore } from '~~/components/user/userStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'

const alertStore = useAlertStore()
const userStore = useUserStore()
const spinnerStore = useSpinnerStore()

const awaitingConsent = ref(null)

const allowGooglePlugin = ref(false)

function googleRegister() {
    if (allowGooglePlugin.value)
        FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false)
    else {
        awaitingConsent.value = 'google'
        alertStore.openAlert(AlertType.Default, { text: '', customHtml: messages.info.googleLogin }, 'Einverstanden', true, 'Registrierung mit Google')
    }
}

function loadGooglePlugin(toRegister = false) {
    allowGooglePlugin.value = true
    awaitingConsent.value = null

    const gapiClientElement = document.getElementById('gapiClient')

    if (gapiClientElement == null) {
        const gapiScript = document.createElement('script')
        gapiScript.setAttribute('id', 'gapiClient')
        gapiScript.src = 'https://apis.google.com/js/api:client.js'
        gapiScript.onload = () => {
            loadGapiLoader(toRegister)
        }
        document.head.appendChild(gapiScript)
    } else if (toRegister)
        loadGapiLoader(toRegister)
}

function loadGapiLoader(toRegister) {
    const gapiLoaderElement = document.getElementById('gapiLoader')

    if (gapiLoaderElement == null) {
        const jsApi = document.createElement('script')
        jsApi.setAttribute('id', 'gapiLoader')
        jsApi.onload = () => {
            var g = new Google()

            setTimeout(() => {
                if (toRegister)
                    Google.SignIn()
            }, 500)
        }
        jsApi.src = 'https://www.google.com/jsapi'
        document.head.appendChild(jsApi)
    } else if (toRegister)
        Google.SignIn()
}

const allowFacebookPlugin = ref(false)
function facebookRegister() {
    if (allowFacebookPlugin.value)
        FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false)
    else {
        awaitingConsent.value = 'facebook'
        alertStore.openAlert(AlertType.Default, { text: '', customHtml: messages.info.facebookLogin }, 'Einverstanden', true, 'Registrierung mit Facebook')
    }
}

function loadFacebookPlugin(toRegister = false) {
    allowFacebookPlugin.value = true
    awaitingConsent.value = null

    const fbScriptElement = document.getElementById('facebook-jssdk')
    if (fbScriptElement == null) {

        window.fbAsyncInit = function () {
            FB.init({
                appId: '1789061994647406',
                xfbml: true,
                version: 'v2.8'
            })
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0]
            if (d.getElementById(id)) { return }
            js = d.createElement(s)
            js.id = id
            js.src = "//connect.facebook.net/de_DE/sdk.js"
            fjs.parentNode.insertBefore(js, fjs)
        }(document, 'script', 'facebook-jssdk'))

        if (toRegister) {
            setTimeout(() => {
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false)
            }, 500);
        }
    }
}

onMounted(() => {
    alertStore.$onAction(({ name, after }) => {
        if (name == 'closeAlert')
            after(() => {
                handleAlertClosing()
            })
    })
})

function handleAlertClosing() {
    if (!alertStore.cancelled) {
        if (awaitingConsent.value == 'google')
            loadGooglePlugin()
        else if (awaitingConsent.value == 'facebook')
            loadFacebookPlugin()
    }
}

onBeforeMount(() => {
    history.pushState(null, 'Registrieren', `/Registrieren`)

    var googleCookie = document.cookie.match('(^|;)\\s*' + "allowGooglePlugin" + '\\s*=\\s*([^;]+)')?.pop() || ''
    if (googleCookie == "true")
        loadGooglePlugin()

    var facebookCookie = document.cookie.match('(^|;)\\s*' + "allowFacebookPlugin" + '\\s*=\\s*([^;]+)')?.pop() || ''
    if (facebookCookie == "true")
        loadFacebookPlugin()
})
const errorMessage = ref('')
const userName = ref('')
const eMail = ref('')
const password = ref('')
const passwordInputType = ref('password')

async function register() {
    spinnerStore.showSpinner()

    let registerData = {
        Name: userName.value,
        Email: eMail.value,
        Password: password.value
    }
    let result = await userStore.register(registerData)

    spinnerStore.hideSpinner()

    if (result == 'success')
        navigateTo(`/${userStore.personalWiki.Name}'/${userStore.personalWiki.Id}`)
    else
        errorMessage.value = messages.error.user[result]
}

</script>

<template>

    <div class="row login-register">
        <div class="form-horizontal col-md-12">
            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                <h1 class="col-sm-offset-2 col-sm-8 register-title">
                    Registrieren
                </h1>
                <div class="col-sm-offset-2 col-sm-8">
                    Dein Wiki ist noch einen Klick entfernt.
                </div>
            </div>

            <div class="form-group omb_login row">
                <div class="col-sm-offset-2 col-sm-8 omb_socialButtons">
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <div class="btn btn-block cursor-hand socialMediaBtn" id="GoogleRegister"
                            @click="googleRegister()">
                            <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg" alt="GoogleRegister"
                                class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Google</div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <div class="btn btn-block cursor-hand socialMediaBtn" id="FacebookRegister"
                            @click="facebookRegister()">
                            <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin"
                                class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Facebook</div>
                        </div>
                    </div>
                </div>
            </div>


            <fieldset>
                <div class="col-sm-offset-2" v-if="errorMessage.length > 0">
                    Bitte überprüfe deine Eingaben
                </div>

                <div class="row" style="margin-bottom: 10px;">
                    <div class="col-xs-12 col-sm-8 col-sm-offset-2">
                        <div class="register-divider-container">
                            <div class="register-divider">
                                <div class="register-divider-line"></div>
                            </div>
                            <div class="register-divider-label-container">
                                <div class="register-divider-label">
                                    oder
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="input-container">
                    <form class="form-horizontal">
                        <div class="form-group">

                            <div class="col-sm-offset-2 col-sm-8">
                                <div class="overline-s no-line">Benutzername</div>
                                <input name="login" placeholder="" type="text" width="100%" class="loginInputs"
                                    v-model="userName" @keydown.enter="register()" @click="errorMessage = ''" />
                            </div>
                        </div>
                    </form>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8">
                        <div class="overline-s no-line">E-Mail</div>
                        <input name="login" placeholder="" type="email" width="100%" class="loginInputs" v-model="eMail"
                            @keydown.enter="register()" @click="errorMessage = ''" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8">
                        <div class="overline-s no-line">Passwort</div>
                        <input name="password" placeholder="" :type="passwordInputType" width="100%" class="loginInputs"
                            v-model="password" @keydown.enter="register()" @click="errorMessage = ''" />
                        <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon" v-if="passwordInputType == 'password'"
                            @click="passwordInputType = 'text'" />
                        <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                            v-if="passwordInputType == 'text'" @click="passwordInputType = 'password'" />
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                        <div @click="register()" class="btn btn-primary memo-button col-sm-12">Registrieren</div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                        <p href="#" style="text-align: center;">
                            Ich bin schon Nutzer!
                            <br />
                        <div style="text-align: center;" class="btn btn-link" @click="userStore.openLoginModal()">
                            Anmelden</div>
                        </p>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8"
                        style="font-size: 12px; padding-top: 20px; text-align: center;">
                        Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren
                        <NuxtLink to="/AGB">
                            Nutzungsbedingungen
                        </NuxtLink>
                        und unserer
                        <NuxtLink to="/Impressum">
                            Datenschutzerklärung
                        </NuxtLink>
                        einverstanden. Du musst mind. 16 Jahre alt sein,
                        <NuxtLink to="/Impressum#under16">hier mehr
                            Infos!
                        </NuxtLink>
                    </div>
                </div>

            </fieldset>
        </div>
    </div>

</template>


<style lang="less" scoped>
@import '~~/assets/shared/register.less';
</style>