<%@ Page Title="Registrieren" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<RegisterModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <%= Scripts.Render("~/Views/Welcome/Js/Validation.js") %>
    <%= Styles.Render("~/bundles/Registration") %>
    
    <script type="text/javascript">
        
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1789061994647406',
                cookie: true,  // enable cookies to allow the server to access 
                // the session
                xfbml: true,  // parse social plugins on this page
                version: 'v2.8' // use graph api version 2.8
            });

            FB.getLoginStatus(function (response) {
                if (response.status === 'connected') {
                    RedirectToDashboard();
                }
            });
        };

        // Load the Facebook-SDK asynchronously
        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));

        function RedirectToDashboard() {
            location.href = "/Wissenszentrale";
        }

        $(function() {
            $("#btn-login-with-facebook").click(function() {    
                FB.getLoginStatus(function (response) {
                    if (response.status === 'connected') {
                        RedirectToDashboard();
                    } else if (response.status === 'not_authorized') {
                        FB.login(function (response) {

                            var facebook_user_id = response.userId;
                            var is_new_user = false;

                            /*    


                                if(!is_registerred(facebook_user_id)){
                                    is_new_user = register user();
                                }

                                login();

                                if(is_new_user)
                                    redirect_to_registration_success_page();
                                else
                                    redirect_to_welcome();
                            */
                            
                        });
                    }
                });
            });
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% using (Html.BeginForm("Register", "Register", null, FormMethod.Post, new { id = "RegistrationForm", enctype = "multipart/form-data" })){%>
    
    <div class="row">
        <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
        </div>
        
        <div class="form-horizontal col-md-9">            
            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                <div class="col-xs-12" >
                    <h1>Jetzt registrieren. Memucho ist kostenlos.</h1>                 
                </div>
                <div class="col-xs-12" >
                    Wir schützen und respektieren deine Privatsphäre.
                    Wir gehen sorgfältig mit deinen Daten um. 
                </div>
                <div class="col-xs-12" >
                    <i class="fa fa-clock-o"></i> <b>Noch 20 Sekunden</b> und du kannst vollständig memucho nutzen :-)
                </div>
            </div>

            <fieldset>
                <%= Html.ValidationSummary(true, "Bitte überprüfe deine Eingaben") %>
                                
                <div class="form-group omb_login">
                    <div class="row omb_socialButtons">
   	                    <div class="col-xs-12 col-sm-offset-2 col-sm-3" style="padding-top: 7px;">
		                    <a href="#" class="btn btn-block omb_btn-facebook" id="btn-login-with-facebook" style="width: 100%">
			                    <span>Facebook</span>
		                    </a>
	                    </div>
        	            <div class="col-xs-12 col-sm-3" style="padding-top: 7px;">
		                    <a href="#" class="btn btn-block omb_btn-google" >
			                    <span>Google+</span>
		                    </a>
	                    </div>	
                    </div>
                </div>
                
                <div class="row" style="margin-top: 30px; margin-bottom: 5px;">
                    <div class="col-sm-offset-2  col-sm-2 col-xs-5" style="border-bottom: 1px solid silver"></div>
                    <div class="col-sm-2 col-xs-2" style="text-align: center"><span style="position: relative; top: -9px;">oder</span></div>
                    <div class="col-sm-2 col-xs-5" style="border-bottom: 1px solid silver"></div>
                </div>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.Name, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.TextBoxFor(model => model.Name, new { @class="form-control", placeholder = Model.Name }) %>
                        <%: Html.ValidationMessageFor(model => model.Name) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <%: Html.LabelFor(model => model.Email, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.TextBoxFor(model => model.Email, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Email) %>
                    </div>
                </div>

                <div class="form-group">
                    <%: Html.LabelFor(model => model.Password, new { @class = "col-sm-2 control-label" }) %>
                    <div class="col-sm-6">
                        <%: Html.PasswordFor(model => model.Password, new { @class="form-control" }) %>
                        <%: Html.ValidationMessageFor(model => model.Password) %>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <label class="checkbox" style="white-space:nowrap;">
                            <%: Html.CheckBoxFor(model => model.TermsAndConditionsApproved, new { @class="" }) %>
                            Ich akzeptiere die <%= Html.ActionLink("Nutzungsbedingungen (AGBs)", Links.TermsAndConditions, Links.VariousController)%>.
                        </label>
                        <%: Html.ValidationMessageFor(model => model.TermsAndConditionsApproved) %>
                    </div>
                </div>
                
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-6" style="border-top:0px; margin-top: 10px;">
                        
                        <a href="<%= Url.Action("Login", "Welcome") %>" class="btn btn-link">Ich bin schon Nutzer!</a>
                        <a href="#" onclick="$(this).closest('form').submit(); return false;" class="btn btn-success"><i class="fa fa-chevron-circle-right">&nbsp;</i> Jetzt Registrieren</a>

                    </div>
                </div>

            </fieldset>            
        </div>
    </div>
<% } %>

</asp:Content>