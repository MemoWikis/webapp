<script setup lang="ts">
import { ref } from 'vue'
import { useUserStore } from '../user/userStore'

const x = useState<boolean>(() => false)

const eMail = ref('')
const password = ref('')
const persistentLogin = ref(false)
const userStore = useUserStore()

async function login() {

    var data = {
        EmailAddress: eMail.value,
        Password: password.value,
        PersistentLogin: persistentLogin.value
    }

    userStore.login(data)
}
const passwordInputType = ref('password')

const googleLoginComponent = ref()

function googleLogin() {
    showLoginIsInProgress.value = true
    googleLoginComponent.value.login()
}
const errorMessage = ref('')
const showLoginIsInProgress = ref(false)

const showGooglePluginInfo = ref(false)
const allowGooglePlugin = ref(false)

function loadGooglePlugin(login = false) {
    document.cookie = "allowGooglePlugin=true";
    allowGooglePlugin.value = true
    showLoginIsInProgress.value = true
    googleLoginComponent.value.loadPlugin(login)
}

const facebookLoginComponent = ref()

function facebookLogin() {
    showLoginIsInProgress.value = true
    facebookLoginComponent.value.login()
}

const showFacebookPluginInfo = ref(false)
const allowFacebookPlugin = ref(false)

function loadFacebookPlugin(login = false) {
    document.cookie = "allowFacebookPlugin=true";
    allowFacebookPlugin.value = true
    showLoginIsInProgress.value = true
    facebookLoginComponent.value.loadPlugin(login)
}
const button1Text = ref('Anmelden' as string | null) 

watch([showLoginIsInProgress, showGooglePluginInfo, showFacebookPluginInfo], ([inProgress, googleInfo, fbInfo]) => {
    if (inProgress || googleInfo || fbInfo)
        button1Text.value = null
    else button1Text.value = 'Anmelden'
})

watch(() => userStore.showLoginModal, () => {
    showLoginIsInProgress.value = false
})

onMounted(() => {
    var googleCookie = document.cookie.match('(^|;)\\s*' + "allowGooglePlugin" + '\\s*=\\s*([^;]+)')?.pop() || ''
    if (googleCookie == "true")
        loadGooglePlugin()

    var facebookCookie = document.cookie.match('(^|;)\\s*' + "allowFacebookPlugin" + '\\s*=\\s*([^;]+)')?.pop() || ''
    if (facebookCookie == "true")
        loadFacebookPlugin()
})
</script>

<template>
    <div id="LoginModalComponent">
        <LazyModal :showCloseButton="true" :modalWidth="600" :button1Text="button1Text" :isFullSizeButtons="true"
            @close="userStore.showLoginModal = false" @mainBtn="login()" :show="userStore.showLoginModal"
            @keydown.esc="userStore.showLoginModal = false">
            <template v-slot:header>
                <span v-if="showGooglePluginInfo && !allowGooglePlugin">Login mit Google</span>
                <span v-else-if="showFacebookPluginInfo && !allowFacebookPlugin">Login mit Facebook</span>
                <span v-else>Anmelden</span>
            </template>
            <template v-slot:body>
                <div v-if="showLoginIsInProgress">
                    Die Anmeldung / Registrierung wird in einem neuen Fenster fortgesetzt.
                </div>

                <div v-else-if="(showGooglePluginInfo && !allowGooglePlugin) || (showFacebookPluginInfo && !allowFacebookPlugin)"
                    class="row">
                    <div v-if="showGooglePluginInfo && !allowGooglePlugin" class="col-xs-12">
                        <p>
                            Beim Login mit Google werden Daten mit den Servern von Google ausgetauscht. Dies geschieht
                            nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer
                            <NuxtLink href="/Impressum">Datenschutzerklärung</NuxtLink> .
                        </p>
                    </div>

                    <div v-else-if="showFacebookPluginInfo && !allowFacebookPlugin" class="col-xs-12">
                        <p>
                            Beim Login mit Facebook werden Daten mit den Servern von Facebook ausgetauscht. Dies
                            geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in
                            unserer <NuxtLink href="/Impressum">Datenschutzerklärung</NuxtLink>.
                        </p>
                    </div>
                </div>
                <template v-else>
                    <div class="form-group omb_login row">
                        <LazyClientOnly>
                            <div class="col-sm-12 omb_socialButtons">

                                <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                                    <div class="btn btn-block cursor-hand socialMediaBtn" id="GoogleLogin"
                                        v-if="allowGooglePlugin" @click="googleLogin()">
                                        <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg"
                                            alt="socialMediaBtnContainer" class="socialMediaLogo">
                                        <div class="socialMediaLabel">weiter mit Google</div>
                                    </div>
                                    <div class="btn btn-block cursor-hand socialMediaBtn" v-else
                                        @click="showGooglePluginInfo = true">
                                        <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg"
                                            alt="socialMediaBtnContainer" class="socialMediaLogo">
                                        <div class="socialMediaLabel">weiter mit Google</div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                                    <div class="btn btn-block cursor-hand socialMediaBtn" id="FacebookLogin"
                                        v-if="allowFacebookPlugin" @click="facebookLogin()">
                                        <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg"
                                            alt="FacebookLogin" class="socialMediaLogo">
                                        <div class="socialMediaLabel">weiter mit Facebook</div>
                                    </div>
                                    <div class="btn btn-block cursor-hand socialMediaBtn" v-else
                                        @click="showFacebookPluginInfo = true">
                                        <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg"
                                            alt="FacebookLogin" class="socialMediaLogo">
                                        <div class="socialMediaLabel">weiter mit Facebook</div>
                                    </div>
                                </div>
                            </div>

                        </LazyClientOnly>

                    </div>

                    <p class="consentInfoText">
                        Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren
                        <NuxtLink to="/AGB">Nutzungsbedingungen</NuxtLink> und unserer <NuxtLink to="/Impressum">
                            Datenschutzerklärung</NuxtLink>
                        einverstanden. Du musst mind. 16 Jahre alt sein, <NuxtLink to="/Impressum#under16">hier mehr
                            Infos!
                        </NuxtLink>
                    </p>

                    <div class="row" style="margin-bottom: 10px;">
                        <div class="col-xs-12">
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
                        <div class="overline-s no-line">E-Mail</div>
                        <form class="form-horizontal">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <input name="login" placeholder="" type="email" width="100%" class="loginInputs"
                                        v-model="eMail" @keydown.enter="login()" @click="errorMessage = ''" />
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">Passwort</div>
                        <form class="form-horizontal">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <input name="password" placeholder="" :type="passwordInputType" width="100%"
                                        class="loginInputs" v-model="password" @keydown.enter="login()"
                                        @click="errorMessage = ''" />
                                    <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon"
                                        v-if="passwordInputType == 'password'" @click="passwordInputType = 'text'" />
                                    <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                                        v-if="passwordInputType == 'text'" @click="passwordInputType = 'password'" />
                                </div>
                            </div>
                        </form>
                        <div class="infoContainer col-sm-12 noPadding">
                            <div class="col-sm-4 noPadding">
                                <label class="cursor-hand">
                                    <input type="checkbox" class="cursor-hand" v-model="persistentLogin" />
                                    <span class="checkboxText">Angemeldet bleiben</span>
                                </label>
                            </div>
                            <div class="col-sm-4 col-sm-offset-4 noPadding" style="text-align: right;">
                                <a href="/Login/PasswortZuruecksetzen">Passwort vergessen?</a>
                            </div>
                        </div>
                        <div class="errorMessage" v-if="errorMessage.length > 0">{{ errorMessage }}</div>
                    </div>
                </template>


            </template>
            <template v-slot:footer-text>
                <div class="row" v-if="showLoginIsInProgress">
                    <p>
                        <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px"
                            @click="showLoginIsInProgress = false">
                            Zurück
                        </button>
                    </p>
                </div>
                <div class="footerText" v-else-if="!showGooglePluginInfo && !showFacebookPluginInfo">
                    <p>
                        <strong style="font-weight: 700;">Noch kein Benutzer?</strong> <br />
                        <NuxtLink href="/Registrieren">Jetzt Registrieren!</NuxtLink>
                    </p>
                </div>
                <div class="row" v-else-if="showGooglePluginInfo">
                    <p>
                        <button type="button" class="btn btn-primary pull-right memo-button"
                            @click="loadGooglePlugin(true)">
                            Einverstanden
                        </button>
                        <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px"
                            @click="showGooglePluginInfo = false">
                            Abbrechen
                        </button>
                    </p>
                </div>

                <div class="row" v-else-if="showFacebookPluginInfo">
                    <p>
                        <button type="button" class="btn btn-primary pull-right memo-button"
                            @click="loadFacebookPlugin()">
                            Einverstanden
                        </button>
                        <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px"
                            @click="showFacebookPluginInfo = false">
                            Abbrechen
                        </button>
                    </p>
                </div>
            </template>
        </LazyModal>
    </div>
    <UserFacebookLogin ref="facebookLoginComponent" />
    <UserGoogleLogin ref="googleLoginComponent" />

</template>

<style scoped lang="less">
@import '~~/assets/shared/register.less';
</style>