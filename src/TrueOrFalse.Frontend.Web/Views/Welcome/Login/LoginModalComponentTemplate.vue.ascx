<div id="LoginModalComponent">
    <default-modal-component showCloseButton="true" modalWidth="600" :button1Text="button1Text" wbutton2Text="abbrechen" action1Emit="login-clicked" isFullSizeButtons="true">
        <template v-slot:header>
            <span v-if="showGooglePluginInfo && !allowGooglePlugin">Login mit Google</span>
            <span v-else-if="showFacebookPluginInfo && !allowFacebookPlugin">Login mit Facebook</span>
            <span v-else>Anmelden</span>
        </template>
        <template v-slot:body>
            
            <div v-if="showLoginIsInProgress">
                Die Anmeldung / Registrierung wird in einem neuem Fenster fortgesetzt.
            </div>

            <div v-else-if="(showGooglePluginInfo && !allowGooglePlugin) || (showFacebookPluginInfo && !allowFacebookPlugin)" class="row">  
                <div v-if="showGooglePluginInfo && !allowGooglePlugin" class="col-xs-12">
                    <p>
                        Beim Login mit Google werden Daten mit den Servern von Google ausgetauscht. Dies geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer <a href="/Impressum">Datenschutzerklärung</a> .
                    </p>
                </div>

                <div v-else-if="showFacebookPluginInfo && !allowFacebookPlugin" class="col-xs-12">
                    <p>
                        Beim Login mit Facebook werden Daten mit den Servern von Facebook ausgetauscht. Dies geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer <a href="/Impressum">Datenschutzerklärung</a>.
                    </p>
                </div>
            </div>

            <template v-else>
                <div class="form-group omb_login row">
                <div class="col-sm-12 omb_socialButtons">
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand socialMediaBtn" id="GoogleLogin" v-if="allowGooglePlugin" @click="GoogleLogin()">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="socialMediaBtnContainer" class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Google</div>
                        </a>
                        <a class="btn btn-block cursor-hand socialMediaBtn" v-else @click="showGooglePluginInfo = true">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="socialMediaBtnContainer" class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Google</div>
                        </a>
                    </div>
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand socialMediaBtn" id="FacebookLogin" v-if="allowFacebookPlugin" @click="FacebookLogin()">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin" class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Facebook</div>
                        </a>
                        <a class="btn btn-block cursor-hand socialMediaBtn" v-else @click="showFacebookPluginInfo = true">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin" class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Facebook</div>
                        </a>
                    </div>
                </div>
            </div>

                <p class="consentInfoText">
                Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a href="/AGB">Nutzungsbedingungen</a> und unserer <a href="/Impressum">Datenschutzerklärung</a> einverstanden. Du musst mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos!</a>
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

        </template>
        <template v-slot:footer-text>
            <div class="row" v-if="showLoginIsInProgress">
                <p>
                    <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px" @click="showLoginIsInProgress = false">
                        Zurück
                    </button>
                </p>
            </div>
            <div class="footerText" v-else-if="!showGooglePluginInfo && !showFacebookPluginInfo">
                <p>
                    <strong style="font-weight: 700;">Noch kein Benutzer?</strong> <br/>
                    <a href="/Registrieren">Jetzt Registrieren!</a>
                </p>
            </div>
            <div class="row" v-else-if="showGooglePluginInfo">
                <p>
                    <button type="button" class="btn btn-primary pull-right memo-button" @click="loadGooglePlugin()">
                        Einverstanden
                    </button>
                    <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px" @click="showGooglePluginInfo = false">
                        Abbrechen
                    </button>
                </p>
            </div>
            
            <div class="row" v-else-if="showFacebookPluginInfo">
                <p>
                    <button type="button" class="btn btn-primary pull-right memo-button" @click="loadFacebookPlugin()">
                        Einverstanden
                    </button>
                    <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px" @click="showFacebookPluginInfo = false">
                        Abbrechen
                    </button>
                </p>
            </div>
        </template>
    </default-modal-component>
</div>
