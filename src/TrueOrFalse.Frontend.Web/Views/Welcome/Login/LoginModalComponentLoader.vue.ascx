<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="LoginModalComponentLoader">
    <default-modal-component showCloseButton="true">
        <template v-slot:header>
            <span>Login</span>
        </template>
        <template v-slot:body>
            <div class="form-group omb_login row">
                <div class="col-sm-offset-2 col-sm-8 omb_socialButtons">
                    <div class="col-xs-12 col-sm-5 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand" id="btn-login-with-facebook-modal">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin" class="socialMediaLogo"><span>mit Facebook</span>
                        </a>
                    </div>
                    <div class="col-xs-12 col-sm-5 col-sm-offset-2 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand" id="btn-login-with-google-modal">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="GoogleLogin" class="socialMediaLogo"><span>mit Google</span>
                        </a>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-top: 20px; margin-bottom: 5px;">
                <div class="col-xs-10 col-xs-offset-1 xxs-stack">
                    <div class="row">
                        <div class="col-xs-5"><div class="Divider" style="margin-right: -10px;"></div></div>
                        <div class="col-xs-2" style="text-align: center"><span style="position: relative; top: -9px;">oder</span></div>
                        <div class="col-xs-5"><div class="Divider" style="margin-left: -10px;"></div></div>
                    </div>
                </div>
                    
            </div>


        </template>
        <template v-slot:footer></template>
    </default-modal-component>
</div>