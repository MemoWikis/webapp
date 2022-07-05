<script setup lang="ts">
import { ref } from 'vue'
import { useUserStore } from '../user/userStore'

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

const facebookLoginMounted = ref(false)
const facebookLoginComponent = ref(null);

function facebookLogin() {
    if (facebookLoginMounted.value)
        facebookLoginComponent.login
    else
        facebookLoginMounted.value = true
}

const errorMessage = ref('')
</script>

<template>
    <button @click="userStore.showLoginModal = true">open login modal</button>

    <div id="LoginModalComponent">
        <LazyModal :showCloseButton="true" :modalWidth="600" button1Text="Anmelden" action1Emit="login-clicked"
            :isFullSizeButtons="true" @close="userStore.showLoginModal = false" @mainBtn="login()"
            :show="userStore.showLoginModal">
            <template v-slot:header>
                <span>Anmelden</span>
            </template>
            <template v-slot:body>

                <div class="form-group omb_login row">
                    <div class="col-sm-12 omb_socialButtons">
                        <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                            <a class="btn btn-block cursor-hand socialMediaBtn" id="GoogleLogin">
                                <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg"
                                    alt="socialMediaBtnContainer" class="socialMediaLogo">
                                <div class="socialMediaLabel">weiter mit Google</div>
                            </a>
                        </div>
                        <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                            <a class="btn btn-block cursor-hand socialMediaBtn" id="FacebookLogin"
                                @click="facebookLogin()">
                                <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin"
                                    class="socialMediaLogo">
                                <div class="socialMediaLabel">weiter mit Facebook</div>
                            </a>
                        </div>
                    </div>
                </div>

                <p class="consentInfoText">Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren
                    <NuxtLink to="/AGB">Nutzungsbedingungen</NuxtLink> und unserer <NuxtLink to="/Impressum">
                        Datenschutzerklärung</NuxtLink>
                    einverstanden. Du musst mind. 16 Jahre alt sein, <NuxtLink to="/Impressum#under16">hier mehr Infos!
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
                                <i class="fas fa-eye eyeIcon" v-if="passwordInputType == 'password'"
                                    @click="passwordInputType = 'text'"></i>
                                <i v-if="passwordInputType == 'text'" @click="passwordInputType = 'password'"
                                    class="fas fa-eye-slash eyeIcon"></i>
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
            <template v-slot:footer-text>
                <div class="footerText">
                    <p>
                        <strong style="font-weight: 700;">Noch kein Benutzer?</strong> <br />
                        <a href="/Registrieren">Jetzt Registrieren!</a>
                    </p>
                </div>
            </template>
        </LazyModal>
    </div>
    <LazyUserFacebookLogin v-if="facebookLoginMounted" ref="facebookLoginComponent" />



</template>

<style scoped>
</style>