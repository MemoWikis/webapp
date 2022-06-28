<script setup lang="ts">
import { ref } from 'vue'
import { FacebookMemuchoUser } from './FacebookMemuchoUser'
import { useUserStore } from '../user/userStore'

const eMail = ref('')
const password = ref('')
const persistentLogin = ref(false)
const userStore = useUserStore()

type UserLoginResult = {
    Success: boolean;
    Message: string;
}

async function login() {

    var data = {
        EmailAddress: eMail.value,
        Password: password.value,
        PersistentLogin: persistentLogin.value
        }

    var result = await $fetch<UserLoginResult>('/api/Login/Login', { method: 'POST', body: data, mode: 'cors', credentials: 'include' 
  }).catch((error) => console.log(error.data))

  if (!!result && result.Success)
    userStore.isLoggedIn = true
}

const passwordInputType = ref('password')

function facebookLogin() {
    FacebookMemuchoUser.LoginOrRegister(/*stayOnPage*/true, /*dissalowRegistration*/ false);
}

const errorMessage = ref('')

</script>

<template>
<div id="LoginModalComponent">
    <Modal showCloseButton="true" modalWidth="600" button1Text="Anmelden" action1Emit="login-clicked" isFullSizeButtons="true">
        <template v-slot:header>
            <span>Anmelden</span>
        </template>
        <template v-slot:body>

            <div class="form-group omb_login row">
                <div class="col-sm-12 omb_socialButtons">
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand socialMediaBtn" id="GoogleLogin">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="socialMediaBtnContainer" class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Google</div>
                        </a>
                    </div>
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand socialMediaBtn" id="FacebookLogin" @click="facebookLogin()">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin" class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Facebook</div>
                        </a>
                    </div>
                </div>
            </div>
            
            <p class="consentInfoText">Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren 
                <a href="/AGB">Nutzungsbedingungen</a> und unserer <a href="/Impressum">Datenschutzerklärung</a> 
                einverstanden. Du musst mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos!</a>
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
                            <input name="login" placeholder="" type="email" width="100%" class="loginInputs" v-model="eMail" @keydown.enter="login()" @click="errorMessage = ''"/>
                        </div>
                    </div>
                </form>
            </div>
            <div class="input-container">
                <div class="overline-s no-line">Passwort</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <input name="password" placeholder="" :type="passwordInputType" width="100%" class="loginInputs" v-model="password" @keydown.enter="login()" @click="errorMessage = ''"/>
                            <i class="fas fa-eye eyeIcon" v-if="passwordInputType == 'password'" @click="passwordInputType = 'text'"></i>
                            <i v-if="passwordInputType == 'text'" @click="passwordInputType = 'password'" class="fas fa-eye-slash eyeIcon"></i>
                        </div>
                    </div>
                </form>
                <div class="infoContainer col-sm-12 noPadding">
                    <div class="col-sm-4 noPadding">
                        <label class="cursor-hand">
                            <input type="checkbox" class="cursor-hand" v-model="persistentLogin"/>
                            <span class="checkboxText">Angemeldet bleiben</span>
                        </label>
                    </div>
                    <div class="col-sm-4 col-sm-offset-4 noPadding" style="text-align: right;">
                        <a href="/Login/PasswortZuruecksetzen">Passwort vergessen?</a>
                    </div>
                </div>
                <div class="errorMessage" v-if="errorMessage.length > 0">{{errorMessage}}</div>
            </div>

        </template>
        <template v-slot:footer-text>
            <div class="footerText">
                <p>
                    <strong style="font-weight: 700;">Noch kein Benutzer?</strong> <br/>
                    <a href="/Registrieren">Jetzt Registrieren!</a>
                </p>
            </div>
        </template>
    </Modal>
</div>


</template>

<style>
#lbtn {
    padding: 10px;
    cursor: pointer;
    background: lightskyblue;
    color: white;
    font-size: larger;
    font-weight: 600;
}
</style>