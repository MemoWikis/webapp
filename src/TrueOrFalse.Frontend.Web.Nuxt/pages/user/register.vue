<script lang="ts" setup>
import { Site } from '~/components/shared/siteEnum'
import { useUserStore } from '~~/components/user/userStore'
import { Google } from '~~/components/user/Google'
import { FacebookMemuchoUser } from '~~/components/user/FacebookMemuchoUser'
import { AlertType, useAlertStore, messages } from '~~/components/alert/alertStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { isValidEmail } from '~/components/shared/utils'

const userStore = useUserStore()
const alertStore = useAlertStore()
const spinnerStore = useSpinnerStore()
interface Props {
    site: Site
}
const props = defineProps<Props>()
const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Site.Register)

    if (userStore.isLoggedIn)
        navigateTo('/')
})

watch(() => userStore.isLoggedIn, (isLoggedIn) => {
    if (isLoggedIn)
        return navigateTo('/')
})


const awaitingConsent = ref(null as null | string)

const allowGooglePlugin = ref(false)

function googleRegister() {
    if (allowGooglePlugin.value)
        Google.SignIn()
    else {
        awaitingConsent.value = 'google'
        alertStore.openAlert(AlertType.Default, { text: '', customHtml: messages.info.googleLogin }, 'Einverstanden', true, 'Registrierung mit Google')
    }
}

function loadGooglePlugin(toLogin = false) {
    allowGooglePlugin.value = true
    awaitingConsent.value = null

    Google.loadGsiClient()
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

const config = useRuntimeConfig()

function loadFacebookPlugin(toRegister = false) {
    allowFacebookPlugin.value = true
    awaitingConsent.value = null

    const fbScriptElement = document.getElementById('facebook-jssdk')
    if (fbScriptElement == null) {

        window.fbAsyncInit = function () {
            FB.init({
                appId: config.public.facebookAppId,
                xfbml: true,
                version: 'v2.8'
            })
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0] as any
            if (d.getElementById(id)) { return }
            js = d.createElement(s) as HTMLScriptElement
            js.id = id
            js.src = "//connect.facebook.net/de_DE/sdk.js"
            fjs.parentNode.insertBefore(js, fjs)
        }(document, 'script', 'facebook-jssdk'))

        if (toRegister) {
            setTimeout(() => {
                FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/false, /*dissalowRegistration*/ false)
            }, 500)
        }
    }
}

onMounted(() => {
    alertStore.$onAction(({ name, after }) => {
        if (name == 'closeAlert')
            after((result) => {
                handleAlertClosing(result.cancelled)
            })
    })
})

function handleAlertClosing(cancelled: boolean) {
    if (!cancelled) {
        if (awaitingConsent.value == 'google')
            loadGooglePlugin(true)
        else if (awaitingConsent.value == 'facebook')
            loadFacebookPlugin(true)
    }
}

onBeforeMount(() => {
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

const { $urlHelper } = useNuxtApp()
async function register() {
    errorMessage.value = ''

    if (!isValidEmail(eMail.value)) {
        errorMessage.value = messages.error.user.emailIsInvalid(eMail.value)
        return
    }

    spinnerStore.showSpinner()

    const registerData = {
        Name: userName.value,
        Email: eMail.value,
        Password: password.value
    }
    const result = await userStore.register(registerData)
    spinnerStore.hideSpinner()
    if (result == 'success' && userStore.personalWiki)
        return navigateTo($urlHelper.getPageUrl(userStore.personalWiki.name, userStore.personalWiki.id))
    else if (result)
        errorMessage.value = result
}

</script>

<template>
    <div class="container">
        <div class="register-container row main-page">
            <div class="col-xs-12 container main-content">
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
                                        <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg"
                                            alt="GoogleRegister" class="socialMediaLogo">
                                        <div class="socialMediaLabel">weiter mit Google</div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                                    <div class="btn btn-block cursor-hand socialMediaBtn" id="FacebookRegister"
                                        @click="facebookRegister()">
                                        <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg"
                                            alt="FacebookLogin" class="socialMediaLogo">
                                        <div class="socialMediaLabel">weiter mit Facebook</div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <fieldset>
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

                            <div class="col-sm-offset-2 alert alert-danger col-sm-8" v-if="errorMessage.length > 0">
                                {{ errorMessage }}
                            </div>

                            <div class="input-container">
                                <form class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-sm-offset-2 col-sm-8">
                                            <div class="overline-s no-line">Benutzername</div>
                                        </div>
                                        <div class="col-sm-offset-2 col-sm-8">
                                            <input name="login" placeholder="" type="text" width="100%"
                                                class="login-inputs" v-model="userName" @keydown.enter="register()"
                                                @click="errorMessage = ''" />
                                        </div>
                                    </div>
                                </form>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8">
                                    <div class="overline-s no-line">E-Mail</div>
                                </div>
                                <div class="col-sm-offset-2 col-sm-8">
                                    <input name="login" placeholder="" type="email" width="100%" class="login-inputs"
                                        v-model="eMail" @keydown.enter="register()" @click="errorMessage = ''" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8">
                                    <div class="overline-s no-line">Passwort</div>
                                </div>

                                <div class="col-sm-offset-2 col-sm-8">
                                    <input name="password" placeholder="" :type="passwordInputType" width="100%"
                                        class="login-inputs" v-model="password" @keydown.enter="register()"
                                        @click="errorMessage = ''" />
                                    <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon"
                                        v-if="passwordInputType == 'password'" @click="passwordInputType = 'text'" />
                                    <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                                        v-if="passwordInputType == 'text'" @click="passwordInputType = 'password'" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                                    <button @click="register()"
                                        class="btn btn-primary memo-button col-sm-12">Registrieren</button>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                                    <p href="#" style="text-align: center;">
                                        Ich bin schon Nutzer!
                                        <br />
                                        <button style="text-align: center;" class="btn btn-link"
                                            @click="userStore.openLoginModal()">
                                            Anmelden</button>
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
            </div>
        </div>

    </div>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/register.less';

.register-container {
    padding-bottom: 45px;
}

#Sidebar {
    display: flex;
    align-items: stretch;
    flex-grow: 1;

    #SidebarDivider {
        margin-top: 20px;
        margin-bottom: 20px;
        border-left: 1px solid @memo-grey-light;
        top: 0;
        flex-grow: 1;
    }
}
</style>