<%@ Page Title="Registrieren" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<RegisterModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/bundles/RegistrationJs") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm("Register", "Register", null, FormMethod.Post, new { id = "RegistrationForm", enctype = "multipart/form-data" }))
        {%>

    <div class="row">
        <div class="form-horizontal col-md-12">
            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                <div class="col-xs-8 col-xs-offset-2">
                    <h1>Registrieren</h1>
                </div>
                <div class="col-xs-8 col-xs-offset-2">
                    Dein Wiki ist noch einen Klick entfernt.
                </div>
            </div>

            <div class="form-group omb_login row">
                <div class="col-sm-offset-2 col-sm-8 omb_socialButtons" style="padding-right: 5px;">
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                        <a class="btn btn-block cursor-hand" id="btn-login-with-facebook-modal" onclick="eventBus.$emit('login-Facebook')">
                            <img src="/Images/SocialMediaIcons/Facebook_logo_F.svg" alt="FacebookLogin" class="socialMediaLogo"><span style="text-transform: uppercase;">weiter mit Facebook</span>
                        </a>
                    </div>
                    <div class="col-xs-12 col-sm-6 socialMediaBtnContainer" style="padding-left: 5px;">
                        <a class="btn btn-block cursor-hand" id="btn-login-with-google-modal" onclick="eventBus.$emit('login-Google')">
                            <img src="/Images/SocialMediaIcons/Google__G__Logo.svg" alt="GoogleLogin" class="socialMediaLogo"><span style="text-transform: uppercase;">weiter mit Google</span>
                        </a>
                    </div>
                </div>
            </div>



            <fieldset>
                <div  class="col-sm-offset-2">
                    <%= Html.ValidationSummary(true, "Bitte überprüfe deine Eingaben") %>
                </div>

                <div class="row" style="margin-top: 30px; margin-bottom: 5px;">
                    <div class="col-sm-offset-3 col-sm-2 col-xs-5" style="border-bottom: 1px solid silver"></div>
                    <div class="col-sm-2 col-xs-2" style="text-align: center"><span style="position: relative; top: -9px;">oder</span></div>
                    <div class="col-sm-2 col-xs-5" style="border-bottom: 1px solid silver"></div>
                </div>
                <div class="input-container">
                    <form class="form-horizontal">
                        <div class="form-group">

                            <div class="col-sm-offset-2 col-sm-8">
                                <div class="overline-s no-line">Benutzername</div>
                                <%: Html.TextBoxFor(model => model.UserName, new { @class="form-control", placeholder = Model.UserName }) %>
                                <%: Html.ValidationMessageFor(model => model.UserName) %>
                            </div>
                        </div>
                    </form>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8">
                        <div class="overline-s no-line">E-Mail</div>
                        <%: Html.TextBoxFor(model => model.Login, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Login) %>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8">
                        <div class="overline-s no-line">Passwort</div>
                        <%: Html.PasswordFor(model => model.Password, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Password) %>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                        <a href="#" onclick="$(this).closest('form').submit(); return false;" class="btn btn-primary memo-button col-sm-12">Registrieren</a>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                        <p href="#" style="text-align: center;">
                            Ich bin schon Nutzer!
                            <br />
                            <a data-btn-login="true" class="cursor-hand" style="text-align: center;" onclick="eventBus.$emit('show-login-modal')">Anmelden</a>
                        </p>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-8" style="font-size: 12px; padding-top: 20px; text-align: center;">
                        Durch die Registrierung mit Google oder Facebook erklärst du dich mit unseren <a href="<%=Links.TermsAndConditions %>">Nutzungsbedingungen</a>
                        und unserer <a href="<%=Links.Imprint %>">Datenschutzerklärung</a> einverstanden. Du musst mind. 16 Jahre alt sein, <a href="/Impressum#under16">hier mehr Infos!</a>
                    </div>
                </div>

            </fieldset>
        </div>
    </div>
    <% } %>
</asp:Content>
