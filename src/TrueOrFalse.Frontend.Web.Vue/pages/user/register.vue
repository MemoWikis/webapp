<script lang="ts" setup>

onBeforeMount(() => {
    history.pushState(null, 'Registrieren', `/Registrieren`);
})
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
                        <a class="btn btn-block cursor-hand socialMediaBtn" id="GoogleRegister">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="GoogleRegister"
                                class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Google</div>
                        </a>
                    </div>
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand socialMediaBtn" id="FacebookRegister"
                            onclick="eventBus.$emit('login-Facebook')">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin"
                                class="socialMediaLogo">
                            <div class="socialMediaLabel">weiter mit Facebook</div>
                        </a>
                    </div>
                </div>
            </div>


            <fieldset>
                <div class="col-sm-offset-2">
                    <!-- <%= Html.ValidationSummary(true, "Bitte überprüfe deine Eingaben") %> -->
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
                                <input type="text" />
                                <!-- <%: Html.TextBoxFor(model => model.UserName, new { @class = "form-control", placeholder = Model.UserName }) %>
                                    <%: Html.ValidationMessageFor(model => model.UserName) %> -->
                            </div>
                        </div>
                    </form>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8">
                        <div class="overline-s no-line">E-Mail</div>
                        <input type="email" />

                        <!-- <%: Html.TextBoxFor(model => model.Login, new { @class = "form-control" }) %>
                            <%: Html.ValidationMessageFor(model => model.Login) %> -->
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8">
                        <div class="overline-s no-line">Passwort</div>
                        <input type="password" />

                        <!-- <%: Html.PasswordFor(model => model.Password, new { @class = "form-control" }) %>
                            <%: Html.ValidationMessageFor(model => model.Password) %> -->
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                        <a href="#" onclick="$(this).closest('form').submit();return false;"
                            class="btn btn-primary memo-button col-sm-12">Registrieren</a>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                        <p href="#" style="text-align: center;">
                            Ich bin schon Nutzer!
                            <br />
                            <a data-btn-login="true" class="cursor-hand" style="text-align: center;"
                                onclick="eventBus.$emit('show-login-modal')">Anmelden</a>
                        </p>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8"
                        style="font-size: 12px; padding-top: 20px; text-align: center;">
                        Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a
                            href="<%= Links.TermsAndConditions %>">Nutzungsbedingungen</a>
                        und unserer <a href="<%= Links.Imprint %>">Datenschutzerklärung</a> einverstanden. Du musst
                        mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos!</a>
                    </div>
                </div>

            </fieldset>
        </div>
    </div>

</template>

<style lang="less" scoped>
@import '~~/assets/shared/register.less';
</style>