<div id="LoginModalComponent">
    <default-modal-component showCloseButton="true" modalWidth="600px" button1Text="Anmelden" action1Emit="login-clicked" isFullSizeButtons="true">
        <template v-slot:header>
            <span>Anmelden</span>
        </template>
        <template v-slot:body>
            <div class="form-group omb_login row">
                <div class="col-sm-12 omb_socialButtons">
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer" style="padding-right: 5px;">
                        <a class="btn btn-block cursor-hand" id="FacebookLogin" @click="FacebookLogin()">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin" class="socialMediaLogo"><span style="text-transform: uppercase; font-size: 12px;">weiter mit Facebook</span>
                        </a>
                    </div>
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer" style="padding-left: 5px;">
                        <a class="btn btn-block cursor-hand" id="GoogleLogin">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="GoogleLogin" class="socialMediaLogo"><span style="text-transform: uppercase; font-size: 12px;">weiter mit Google</span>
                        </a>
                    </div>
                </div>
            </div>
            
            <p class="infoText">Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a href="/AGB">Nutzungsbedingungen</a> und unserer <a href="/Impressum">Datenschutzerklärung</a> einverstanden. Du musst mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos!</a></p>

            <div class="row" style="margin-top: 20px; margin-bottom: 5px;">
                <div class="col-xs-10 col-xs-offset-1 xxs-stack">
                    <div class="row">
                        <div class="col-xs-5">
                            <div class="Divider" style="margin-right: -10px;"></div>
                        </div>
                        <div class="col-xs-2" style="text-align: center"><span style="position: relative; top: -9px;">oder</span></div>
                        <div class="col-xs-5">
                            <div class="Divider" style="margin-left: -10px;"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="input-container">
                <div class="overline-s no-line">E-Mail</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <input name="login" placeholder="" type="email" width="100%" class="loginInputs" v-model="eMail" @keydown.enter="SubmitForm()" @click="errorMessage = ''"/>
                        </div>
                    </div>
                </form>
            </div>
            <div class="input-container">
                <div class="overline-s no-line">Passwort</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12">
                            <input name="password" placeholder="" :type="passwordInputType" width="100%" class="loginInputs" v-model="password" @keydown.enter="SubmitForm()" @click="errorMessage = ''"/>
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
    </default-modal-component>
</div>
