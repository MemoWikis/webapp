<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div>
    <default-modal-component showCloseButton="true">
        <template v-slot:header>
            <span>Login</span>
        </template>
        <template v-slot:body>
            <div class="form-group omb_login">
                <div class="omb_socialButtons">
                    <div class="col-xs-offset-1 col-xs-5 xxs-stack" style="padding-top: 7px;">
	                           
                        <div class="g-signin2" data-onsuccess="onSignIn"></div>
                        <a href="#" class="btn btn-block omb_btn-facebook" id="btn-login-with-facebook-modal" style="width: 100%">
                            <span>Facebook</span>
                        </a>
                    </div>
                    <div class="col-xs-5 xxs-stack" style="padding-top: 7px;">
                        <a href="#" class="btn btn-block omb_btn-google" id="btn-login-with-google-modal" >
                            <span>Google+</span>
                        </a>
                    </div>	
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-offset-1 col-xs-10 xxs-stack" style="font-size: 12px; padding-top: 7px;">
                    *Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a href="<%=Links.TermsAndConditions %>">Nutzungsbedingungen</a>
                    und unserer <a href="<%=Links.Imprint %>">Datenschutzerklärung</a> einverstanden. 
                    <br/><br/>
                    Du musst mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos</a>!
                </div>
            </div>
        </template>
        <template v-slot:footer></template>
    </default-modal-component>
</div>